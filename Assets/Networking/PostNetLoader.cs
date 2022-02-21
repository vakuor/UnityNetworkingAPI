using System.Text;
using System.Threading;
using UnityEngine.Networking;

namespace Networking
{
    /// <summary>
    /// Get method loader.
    /// </summary>
    public class PostNetLoader : HttpLoader<string>
    {
        protected readonly string postData;

        public PostNetLoader(string url, string postData, CancellationTokenSource cts = null) : base(url, cts)
        {
            this.postData = postData;
        }

        protected override UnityWebRequest SetupRequest()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            var request = UnityWebRequest.Put(url, bytes);
            request.method = "POST";
            return request;
        }

        protected override string GetResult(UnityWebRequest webRequest)
        {
            return webRequest.downloadHandler.text;
        }
    }
}