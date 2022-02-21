using System;
using System.Data;
using System.Threading.Tasks;
using UnityEngine;

namespace Networking
{
    /// <summary>
    /// Base class for all network loaders.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NetLoader<T>
    {
        /// <summary>
        /// The result of loading. Must be called only after await Download.
        /// </summary>
        public T Result => result;
        protected T result;
        private float progress;

        public virtual float Progress
        {
            get => progress;
            protected set {
                progress = value;
                progressCallback?.Invoke(progress);
            }
        }

        private Action<float> progressCallback;

        /// <summary>
        /// Attach callback that invoked on every progress value change.
        /// </summary>
        /// <param name="progressCallback"></param>
        public void SetProgressCallback(Action<float> progressCallback)
        {
            this.progressCallback = progressCallback;
        }

        /// <summary>
        /// Method for checks before loading started.
        /// </summary>
        /// <exception cref="DataException"></exception>
        protected virtual void OnPreDownload()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
                throw new DataException("Internet is not reachable");
        }

        /// <summary>
        /// Method for checks after loading completed.
        /// </summary>
        /// <exception cref="DataException"></exception>
        protected virtual void OnPostDownload()
        {
            if (result == null)
                throw new DataException("No result data");
        }

        /// <summary>
        /// Downloading logic for each currently created loader.
        /// </summary>
        /// <returns></returns>
        protected abstract Task<T> DownloadInternal();

        /// <summary>
        /// Common awaited Download logic. After creation you should call await NetLoader.Download() only then get result.
        /// </summary>
        /// <returns></returns>
        public async Task Download()
        {
            try
            {
                OnPreDownload();
                result = await DownloadInternal();
                OnPostDownload();
            }
            catch (DataException e) //TODO: неправильный эксепшн
            {
                Debug.Log("Data exception was thrown.");
                result = default;
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Request has been cancelled!");
                result = default;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                result = default;
            }
        }
    }
}