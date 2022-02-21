using UnityEngine;

namespace Examples
{
	public class ExampleCaller : MonoBehaviour
	{
		private async void Start()
		{
			NetExamples netExamples = new NetExamples();
			await netExamples.GetLoad("http://google.com/index.html");
			await netExamples.PostLoad("https://api.amplitude.com/2/httpapi");
		}
	}
}