using UnityEngine;

namespace Core
{
	public class FPSCounter : MonoBehaviour
	{
		private float _fps;
		private float _smoothFPS;
		private float _avgFPS;
		private float _currentMax;
		private float _currentMin;

		private float[] _fpsHistory = new float[100];
		
		private void Awake()
		{
			
		}

		private void Update()
		{
			_fps = 1f / Time.unscaledDeltaTime; //current fps
			_smoothFPS = 1f / Time.smoothDeltaTime; //smooth fps
			_avgFPS = Time.frameCount / Time.time; //average fps
		}

		private void UpdateGraph()
		{
			var graphMaxFPS = _currentMax * 1.1f;
			var graphMinFPS = _currentMin * 0.9f;
		}
	}
}