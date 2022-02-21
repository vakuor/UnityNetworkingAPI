using System;
using System.Threading.Tasks;
using Networking;
using UnityEngine;

namespace Examples
{
	public class NetExamples
	{
		private async Task GetLoad(string url)
		{
			HttpLoader<string> loader = new GetNetLoader(url);
			loader.SetProgressCallback(DisplayProgress);
			loader.SetHeaders(NetSettings.DefaultHeaders);
			await loader.Download();
			Debug.Log(loader.Result);
		}

		private async Task BundleDownload(string bundleName, Action<float> progressCallback = null)
		{
			if (IsBundleExists(bundleName))
			{
				Debug.LogWarning("The bundle you are trying to download already exists! It will not be redownloaded.");
				return;
			}

			NetLoader<AssetBundle> loader = new BundleDownloader("bundle.url", CreateUniqueCachedAssetBundle(bundleName));
			if(progressCallback!=null)
				loader.SetProgressCallback(progressCallback);
			await loader.Download();
		}
		private bool IsBundleExists(string bundleName)
		{
			var cachedAssetBundle = CreateUniqueCachedAssetBundle(bundleName);
			return IsCachedAssetBundleExists(cachedAssetBundle);
		}

		private bool IsCachedAssetBundleExists(CachedAssetBundle cachedAssetBundle)
		{
			return Caching.IsVersionCached(cachedAssetBundle);
		}

		private CachedAssetBundle CreateUniqueCachedAssetBundle(string bundleName)
		{
			return new CachedAssetBundle(bundleName, GetUniqueBundleHash(bundleName));
		}

		private Hash128 GetUniqueBundleHash(string bundleName)
		{
			return Hash128.Compute(bundleName);
		}

		private void DisplayProgress(float f)
		{
			Debug.Log("Progress: " + f);
		}
	}
}