using System.Threading;
using UnityEngine.Networking;

namespace Networking
{
    /// <summary>
    /// Get method loader.
    /// </summary>
    public class GetNetLoader : HttpLoader<string>
    {
        public GetNetLoader(string url,  CancellationTokenSource cts = null) : base(url, cts){}

        protected override UnityWebRequest SetupRequest()
        {
            return UnityWebRequest.Get(url);
        }

        protected override string GetResult(UnityWebRequest webRequest)
        {
            return webRequest.downloadHandler.text;
        }
    }
}