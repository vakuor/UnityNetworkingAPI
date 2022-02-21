using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;

namespace Networking
{
    /// <summary>
    /// Loader for headers of specified URLs.
    /// </summary>
    public class HeadNetLoader : HttpLoader<Dictionary<string, string>>
    {
        public HeadNetLoader(string url, CancellationTokenSource cts = null) : base(url, cts){}

        protected override UnityWebRequest SetupRequest()
        {
            return UnityWebRequest.Head(url);
        }

        /// <summary>
        /// Returns headers from url.
        /// </summary>
        /// <param name="webRequest"></param>
        /// <returns></returns>
        protected override Dictionary<string, string> GetResult(UnityWebRequest webRequest)
        {
            return webRequest.GetResponseHeaders();
        }
    }
}