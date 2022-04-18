using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Producer.API.Business
{
    public abstract class Disposable : IDisposable
    {
        #region [Member variales]
        protected bool _disposed;
        #endregion [Member variales]

        #region [Constructors]
        protected Disposable()
        {
            this._disposed = false;
        }
        #endregion [Constructors]

        /// <summary>
        /// Calls Dispose method on a given object if not null
        /// </summary>
        /// <typeparam name="T">Any IDisposable object</typeparam>
        /// <param name="objectToDispose">The object to dispose</param>
        public static void SafeDispose<T>(ref T objectToDispose) where T : class, IDisposable
        {
            if (objectToDispose != null)
            {
                objectToDispose.Dispose();
                objectToDispose = null;
            }
        }

        #region [IDisposable Members]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    cleanup();
                }
            }
            this._disposed = true;
        }
        /// <summary>
        /// Disposes all managed and unmanaged resources
        /// </summary>
        protected abstract void cleanup();
        #endregion [IDisposable Members]
    }
}
