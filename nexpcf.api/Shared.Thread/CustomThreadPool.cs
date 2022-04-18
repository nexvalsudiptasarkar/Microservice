using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;

namespace Shared.Thread
{
    public class CustomThreadPool
    {
        #region [Configurable items]
        private const int MAX = 8;
        private const int MIN = 3;
        private const int MIN_WAIT = 10;
        private const int MAX_WAIT = 15000;
        private const int CLEANUP_INTERVAL = 60000;
        private const int SCHEDULING_INTERVAL = 10;
        #endregion [Configurable items]

        #region [Singleton instance of threadpool]
        private static readonly CustomThreadPool _instance = new CustomThreadPool();

        private CustomThreadPool()
        {
            InitializeThreadPool();
        }

        public static CustomThreadPool Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion [Singleton instance of threadpool]

        private Queue<TaskHandle> ReadyQueue = null;
        private List<TaskItem> Pool = null;
        private System.Threading.Thread taskScheduler = null;
        private object syncLock = new object();
        private object criticalLock = new object();

        private void InitializeThreadPool()
        {
            ReadyQueue = new Queue<TaskHandle>();
            Pool = new List<TaskItem>();

            InitPoolWithMinCapacity();

            DateTime LastCleanup = DateTime.Now;

            taskScheduler = new System.Threading.Thread(() =>
            {
                do
                {
                    lock (syncLock)
                    {
                        while (ReadyQueue.Count > 0 && ReadyQueue.Peek().task == null) ReadyQueue.Dequeue();

                        int itemCount = ReadyQueue.Count;
                        for (int i = 0; i < itemCount; i++)
                        {
                            TaskHandle readyItem = ReadyQueue.Peek();
                            bool Added = false;
                            lock (criticalLock)
                            {
                                foreach (TaskItem ti in Pool)
                                {
                                    lock (ti)
                                    {
                                        if (ti.taskState == TaskState.completed)
                                        {
                                            ti.taskHandle = readyItem;
                                            ti.taskState = TaskState.notstarted;
                                            Added = true;
                                            ReadyQueue.Dequeue();
                                            break;
                                        }
                                    }
                                }

                                if (!Added && Pool.Count < MAX)
                                {
                                    TaskItem ti = new TaskItem() { taskState = TaskState.notstarted };
                                    ti.taskHandle = readyItem;
                                    AddTaskToPool(ti);
                                    Added = true;
                                    ReadyQueue.Dequeue();
                                }
                            }
                            if (!Added) break;
                        }
                    }
                    if ((DateTime.Now - LastCleanup) > TimeSpan.FromMilliseconds(CLEANUP_INTERVAL))
                    {
                        CleanupPool();
                        LastCleanup = DateTime.Now;
                    }
                    else
                    {
                        System.Threading.Thread.Yield();
                        System.Threading.Thread.Sleep(SCHEDULING_INTERVAL);
                    }
                } while (true);
            });
            taskScheduler.Priority = ThreadPriority.AboveNormal;
            taskScheduler.Start();
        }

        private void InitPoolWithMinCapacity()
        {
            for (int i = 0; i <= MIN; i++)
            {
                TaskItem ti = new TaskItem() { taskState = TaskState.notstarted };
                ti.taskHandle = new TaskHandle() { task = () => { } };
                ti.taskHandle.callback = (taskStatus) => { };
                ti.taskHandle.Token = new ClientHandle() { ID = Guid.NewGuid() };
                AddTaskToPool(ti);
            }
        }

        private void AddTaskToPool(TaskItem taskItem)
        {
            taskItem.handler = new System.Threading.Thread(() =>
            {
                do
                {
                    bool Enter = false;
                    lock (taskItem)
                    {
                        if (taskItem.taskState == TaskState.aborted) break;

                        if (taskItem.taskState == TaskState.notstarted)
                        {
                            taskItem.taskState = TaskState.processing;
                            taskItem.startTime = DateTime.Now;
                            Enter = true;
                        }
                    }
                    if (Enter)
                    {
                        TaskStatus taskStatus = new TaskStatus();
                        try
                        {
                            taskItem.taskHandle.task.Invoke();
                            taskStatus.Success = true;
                        }
                        catch (Exception ex)
                        {
                            taskStatus.Success = false;
                            taskStatus.InnerException = ex;
                        }
                        lock (taskItem)
                        {
                            if (taskItem.taskHandle.callback != null && taskItem.taskState != TaskState.aborted)
                            {
                                try
                                {
                                    taskItem.taskState = TaskState.completed;
                                    taskItem.startTime = DateTime.MaxValue;

                                    taskItem.taskHandle.callback(taskStatus);
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    System.Threading.Thread.Yield(); System.Threading.Thread.Sleep(MIN_WAIT);
                } while (true);
            });
            taskItem.handler.Start();
            lock (criticalLock)
            {
                Pool.Add(taskItem);
            }
        }

        private void CleanupPool()
        {
            List<TaskItem> filteredTask = null;
            lock (criticalLock)
            {
                filteredTask = Pool.Where(ti => ti.taskHandle.Token.IsSimpleTask == true && (DateTime.Now - ti.startTime) > TimeSpan.FromMilliseconds(MAX_WAIT)).ToList();
            }
            foreach (var taskItem in filteredTask)
            {
                CancelUserTask(taskItem.taskHandle.Token);
            }
            lock (criticalLock)
            {
                filteredTask = Pool.Where(ti => ti.taskState == TaskState.aborted).ToList();
                foreach (var taskItem in filteredTask)
                {
                    try
                    {
                        taskItem.handler.Abort();
                        taskItem.handler.Priority = ThreadPriority.Lowest;
                        taskItem.handler.IsBackground = true;
                    }
                    catch { }
                    Pool.Remove(taskItem);
                }
                int total = Pool.Count;
                if (total >= MIN)
                {
                    filteredTask = Pool.Where(ti => ti.taskState == TaskState.completed).ToList();
                    foreach (var taskItem in filteredTask)
                    {
                        taskItem.handler.Priority = ThreadPriority.AboveNormal;
                        taskItem.taskState = TaskState.aborted;
                        Pool.Remove(taskItem);
                        total--;
                        if (total == MIN) break;
                    }
                }
                while (Pool.Count < MIN)
                {
                    TaskItem ti = new TaskItem() { taskState = TaskState.notstarted };
                    ti.taskHandle = new TaskHandle() { task = () => { } };
                    ti.taskHandle.Token = new ClientHandle() { ID = Guid.NewGuid() };
                    ti.taskHandle.callback = (taskStatus) => { };
                    AddTaskToPool(ti);
                }
            }
        }

        #region [Public interface]
        public ClientHandle QueueUserTask(UserTask task, Action<TaskStatus> callback)
        {
            TaskHandle th = new TaskHandle()
            {
                task = task,
                Token = new ClientHandle()
                {
                    ID = Guid.NewGuid()
                },
                callback = callback
            };
            lock (syncLock)
            {
                ReadyQueue.Enqueue(th);
            }
            return th.Token;
        }

        public static void CancelUserTask(ClientHandle clientToken)
        {
            lock (Instance.syncLock)
            {
                var thandle = Instance.ReadyQueue.FirstOrDefault((th) => th.Token.ID == clientToken.ID);
                if (thandle != null)
                {
                    thandle.task = null;
                    thandle.callback = null;
                    thandle.Token = null;
                }
                else
                {
                    int itemCount = Instance.ReadyQueue.Count;
                    TaskItem taskItem = null;
                    lock (Instance.criticalLock)
                    {
                        taskItem = Instance.Pool.FirstOrDefault(task => task.taskHandle.Token.ID == clientToken.ID);
                    }
                    if (taskItem != null)
                    {
                        lock (taskItem)
                        {
                            if (taskItem.taskState != TaskState.completed)
                            {
                                taskItem.taskState = TaskState.aborted;
                                taskItem.taskHandle.callback = null;
                            }
                            if (taskItem.taskState == TaskState.aborted)
                            {
                                try
                                {
                                    taskItem.handler.Abort(); // **** doesn't work ****
                                    taskItem.handler.Priority = ThreadPriority.BelowNormal;
                                    taskItem.handler.IsBackground = true;
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
        }
        #endregion [Public interface]

        #region [Nested private types]
        enum TaskState
        {
            notstarted,
            processing,
            completed,
            aborted
        }
        class TaskHandle
        {
            public ClientHandle Token;
            public UserTask task;
            public Action<TaskStatus> callback;
        }
        class TaskItem
        {
            public TaskHandle taskHandle;
            public System.Threading.Thread handler;
            public TaskState taskState = TaskState.notstarted;
            public DateTime startTime = DateTime.MaxValue;
        }
        #endregion [Nested private types]
    }

    public delegate void UserTask();
    public class ClientHandle
    {
        public Guid ID;
        public bool IsSimpleTask = false;
    }
    public class TaskStatus
    {
        public bool Success = true;
        public Exception InnerException = null;
    }
}
