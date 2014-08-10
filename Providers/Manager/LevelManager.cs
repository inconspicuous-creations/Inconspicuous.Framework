using System.Collections;
using System.ComponentModel.Composition;
using UnityEngine;

namespace Inconspicuous.Framework {
	[Export(typeof(ILevelManager))]
	public class LevelManager : ILevelManager {
		private LevelManagerComponent levelManagerComponent;

		public LevelManager() {
			if(GameObject.Find("LevelManager") == null) {
				var obj = new GameObject("LevelManager");
				levelManagerComponent = obj.AddComponent<LevelManagerComponent>();
				Object.DontDestroyOnLoad(obj);
			} else {
				levelManagerComponent = GameObject.Find("LevelManager").GetComponent<LevelManagerComponent>();
			}
		}

		public void Load(string level) {
			levelManagerComponent.StopAllCoroutines();
			levelManagerComponent.StartCoroutine(levelManagerComponent.LoadInBackground(level));
		}
	}

	public class LevelManagerComponent : MonoBehaviour {
		private float alpha;
		private Texture2D blackTexture;

		public void Awake() {
			blackTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
			blackTexture.SetPixel(0, 0, Color.blue);
		}

		public void OnGUI() {
			if(alpha > 0f) {
				GUI.color = new Color(0, 0, 0, alpha);
				GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackTexture, ScaleMode.StretchToFill, false);
			}
		}

		public IEnumerator LoadInBackground(string level) {
			float fadeTime = 0.2f;
			float elapsedTime = 0f;
			while(elapsedTime <= fadeTime) {
				elapsedTime += Time.deltaTime;
				alpha = Mathf.Clamp01(elapsedTime / fadeTime);
				AudioListener.volume = 1f - alpha;
				yield return null;
			}
			//if(agentObject != null && level == "Game") {
			//	yield return StartCoroutine(agentObject.GetComponent<AdmobInterstitial>().Show());
			//}
			yield return Application.LoadLevelAsync(level);
			yield return new WaitForSeconds(0.2f);
			elapsedTime = 0f;
			while(elapsedTime <= fadeTime) {
				elapsedTime += Time.deltaTime;
				alpha = Mathf.Clamp01(1f - (elapsedTime / fadeTime));
				AudioListener.volume = 1f - alpha;
				yield return null;
			}
		}
	}
}
