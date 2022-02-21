using System.Collections.Generic;
using System.Threading;
using Networking;
using UnityEngine.Networking;

namespace Networking
{
    public abstract class HttpLoader<T> : UnityWebRequestLoader<T>
    {
        private Dictionary<string, string> requestHeaders;

        protected HttpLoader(string url, CancellationTokenSource cts = null)
        {
            this.url = url;
            this.cts = cts;
        }

        protected sealed override UnityWebRequest GetWebRequest()
        {
            var request = SetupRequest();
            SetupHeaders(request); //TODO: подумать, может оно должно быть не здесь?
            return request;
        }

        public void SetHeaders(Dictionary<string, string> requestHeaders)
        {
            this.requestHeaders = requestHeaders;
        }
        public void AddHeader(string headerKey, string headerValue)
        {
            requestHeaders.Add(headerKey, headerValue);
        }

        private void SetupHeaders(UnityWebRequest request)
            //TODO: мы должны задавать headers извне, надо подумать о том, что озможно нужно хранить сам request как поле и обращаться к нему в этом объекте
        {
            if (requestHeaders == null || requestHeaders.Count <= 0) return;
            foreach (var header in requestHeaders)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }
        }

        protected abstract UnityWebRequest SetupRequest();
    }
}