using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    /// <summary>
    /// Loader for classes who use UnityWebRequest object for loading;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UnityWebRequestLoader<T> : NetLoader<T>
    {
        protected string url;
        protected CancellationTokenSource cts;

        protected abstract UnityWebRequest GetWebRequest();

        /// <summary>
        /// Method for getting result from UnityWebRequest object. Should be overriden on every inherited Loader class.
        /// </summary>
        /// <param name="webRequest"></param>
        /// <returns></returns>
        protected abstract T GetResult(UnityWebRequest webRequest);

        protected override async Task<T> DownloadInternal()
        {
            // ReSharper disable once ConvertToUsingDeclaration
            using (var webRequest = GetWebRequest())
            {
                Debug.Log("Await start: " + url);
                UnityWebRequestAsyncOperation asyncOperation = webRequest.SendWebRequest();
                asyncOperation.ObserveEveryValueChanged(operation => operation.progress).Subscribe(progress => Progress = progress);

                if (cts != null) await asyncOperation.WithCancellation(cts.Token); //TODO: протестить
                else await asyncOperation;

                Debug.Log("Await end: " + url);

                string[] pages = url.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        throw new Exception(pages[page] + ": Error: " + webRequest.error);
                    case UnityWebRequest.Result.ProtocolError:
                        throw new Exception(pages[page] + ": HTTP Error: " + webRequest.error);
                    case UnityWebRequest.Result.Success:
                        return GetResult(webRequest);
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}