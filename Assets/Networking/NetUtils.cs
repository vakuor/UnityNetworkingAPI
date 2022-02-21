using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    public static class NetUtils
    {
        public static async Task<string> UriGetTask(string uri, CancellationTokenSource cts = null)
        {
            try
            {
                if (Application.internetReachability == NetworkReachability.NotReachable) return null;
                Debug.Log("Await start: " + uri);

                // ReSharper disable once ConvertToUsingDeclaration
                using (var webRequest = UnityWebRequest.Get(uri))
                {
                    if (cts != null) await webRequest.SendWebRequest().WithCancellation(cts.Token);
                    else await webRequest.SendWebRequest();
                    Debug.Log("Await end: " + uri);

                    string[] pages = uri.Split('/');
                    int page = pages.Length - 1;

                    switch (webRequest.result)
                    {
                        case UnityWebRequest.Result.ConnectionError:
                        case UnityWebRequest.Result.DataProcessingError:
                            Debug.LogWarning(pages[page] + ": Error: " + webRequest.error);
                            break;
                        case UnityWebRequest.Result.ProtocolError:
                            Debug.LogWarning(pages[page] + ": HTTP Error: " + webRequest.error);
                            break;
                        case UnityWebRequest.Result.Success:
                            return webRequest.downloadHandler.text;
                        default:
                            throw new NotImplementedException();
                    }
                }
                return null;
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Request has been cancelled!");
                return null;
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
                return null;
            }
        }

        public static async Task<Dictionary<string, Task<string>>> UriGetMultipleTask(string[] uris,
            CancellationTokenSource cts)
        {
            Dictionary<string, Task<string>> results = new Dictionary<string, Task<string>>();

            for (int i = 0; i < uris.Length; i++)
            {
                var request = UriGetTask(uris[i], cts);
                results.Add(uris[i], request);
            }

            try
            {
                await Task.WhenAll(results.Values);
                if (cts.IsCancellationRequested)
                {
                    Debug.Log("CANCELLED!");
                    throw new OperationCanceledException();
                }
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Net loading operation cancelled!");
                return null;
            }

            return results;
        }
    }
}