using System;
using System.Diagnostics;

namespace Shared.Data
{
    public abstract class Singleton<T> where T : class
    {
        private static readonly Lazy<T> lazy = new Lazy<T>(() => Create());
        public static T Instance { get { return lazy.Value; } }

        protected Singleton()
        {
            Debug.WriteLine(string.Format("{0} instantiated.", typeof(T).GetType().Name));
        }

        private static T Create()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }
    }


}
