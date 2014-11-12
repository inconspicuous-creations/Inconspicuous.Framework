using System;
using System.Collections;
using System.ComponentModel.Composition;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	[Export(typeof(ILevelManager))]
	public class LevelManager : ILevelManager {
		private LevelManagerComponent levelManagerComponent;

		public LevelManager() {
			if(GameObject.Find("LevelManager") == null) {
				var obj = new GameObject("LevelManager");
				levelManagerComponent = obj.AddComponent<LevelManagerComponent>();
				UnityEngine.Object.DontDestroyOnLoad(obj);
			} else {
				levelManagerComponent = GameObject.Find("LevelManager").GetComponent<LevelManagerComponent>();
			}
		}

		public IObservable<IContextView> Load(string level) {
			if(!levelManagerComponent.LoadingLevel) {
				levelManagerComponent.StartCoroutine_Auto(levelManagerComponent.LoadInBackground(level));
				return Observable.Create<IContextView>(observer => {
					var callback = new Action<IContextView>(contextView => observer.OnNext(contextView));
					var ready = new Action(() => observer.OnCompleted());
					levelManagerComponent.OnFinished += callback;
					levelManagerComponent.OnReady += ready;
					return Disposable.Create(() => {
						levelManagerComponent.OnFinished -= callback;
						levelManagerComponent.OnReady -= ready;
					});
				});
			}
			return Observable.Return<IContextView>(null);
		}
	}

	public class LevelManagerComponent : MonoBehaviour {
		public event Action<IContextView> OnFinished;
		public event Action OnReady;

		private float alpha;
		private Texture2D blackTexture;

		public bool LoadingLevel { get; private set; }

		public void Awake() {
			OnFinished = delegate { };
			OnReady = delegate { };
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
			LoadingLevel = true;
			float fadeTime = 0.5f;
			float elapsedTime = 0f;
			while(elapsedTime <= fadeTime) {
				elapsedTime += Time.deltaTime;
				alpha = Mathf.Clamp01(elapsedTime / fadeTime);
				yield return null;
			}
			//if(agentObject != null && level == "Game") {
			//	yield return StartCoroutine(agentObject.GetComponent<AdmobInterstitial>().Show());
			//}
			yield return Application.LoadLevelAsync(level);
			GameObject gameObject;
			var counter = 0f;
			do {
				counter += Time.deltaTime;
				gameObject = GameObject.Find("_" + Application.loadedLevelName + "ContextView");
				if(counter > 3f) {
					break;
				}
				yield return null;
			} while(gameObject == null);
			if(gameObject != null) {
				var contextView = gameObject.GetComponent(typeof(IContextView)) as IContextView;
				OnFinished(contextView);
				var concreteContextView = contextView as ContextView;
				if(concreteContextView != null) {
					var started = false;
					concreteContextView.UpdateAsObservable()
						.Skip(10).First()
						.Subscribe(_ => started = true);
					while(!started) {
						counter += Time.deltaTime;
						if(counter > 3f) {
							break;
						}
						yield return null;
					}
				}
			}
			elapsedTime = 0f;
			fadeTime = 0.8f;
			while(elapsedTime <= fadeTime) {
				elapsedTime += Time.deltaTime;
				alpha = Mathf.Clamp01(1f - (elapsedTime / fadeTime));
				yield return null;
			}
			LoadingLevel = false;
			OnReady();
		}
	}
}
