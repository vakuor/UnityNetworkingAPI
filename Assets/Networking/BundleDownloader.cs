using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    /// <summary>
    /// Loader for downloading bundle objects.
    /// </summary>
    public class BundleDownloader : UnityWebRequestLoader<AssetBundle>
    {
        private readonly CachedAssetBundle cachedAssetBundle;

        public BundleDownloader(string url, CachedAssetBundle cachedAssetBundle, CancellationTokenSource cts = null)
        {
            this.url = url;
            this.cts = cts;
            this.cachedAssetBundle = cachedAssetBundle;
        }

        protected override UnityWebRequest GetWebRequest()
        {
            return UnityWebRequestAssetBundle.GetAssetBundle(url, cachedAssetBundle);
        }

        protected override AssetBundle GetResult(UnityWebRequest webRequest)
        {
            return DownloadHandlerAssetBundle.GetContent(webRequest);
        }
    }
}