using System;
using System.Collections;
using System.ComponentModel.Composition;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	[Export(typeof(ILevelManager))]
	public class LevelManager : ILevelManager {
		private readonly LevelManagerComponent levelManagerComponent;

		public LevelManager() {
			var obj = GameObject.Find("LevelManager");
			if(obj == null) {
				obj = new GameObject("LevelManager");
				levelManagerComponent = obj.AddComponent<LevelManagerComponent>();
			}
			UnityEngine.Object.DontDestroyOnLoad(obj);
			this.levelManagerComponent = obj.GetComponent<LevelManagerComponent>();
		}

		public IObservable<IContextView> Load(string level) {
			if(!levelManagerComponent.LoadingLevel) {
				return Observable.FromCoroutine<IContextView>(observer => levelManagerComponent.LoadInBackground(level, observer));
			}
			return Observable.Throw<IContextView>(new CustomException("Can't load level at the moment."));
		}
	}

	public class LevelManagerComponent : MonoBehaviour {
		public bool LoadingLevel { get; private set; }

		public IEnumerator LoadInBackground(string level, IObserver<IContextView> observer) {
			LoadingLevel = true;
			//var screenEffect = ScreenEffect;
			//if(screenEffect != null) {
			//	yield return screenEffect.Apply().StartAsCoroutine();
			//}
			yield return Application.LoadLevelAsync(level);
			var contextView = default(IContextView);
			yield return StartCoroutine(WaitForContextView(x => contextView = x));
			if(contextView != null) {
				observer.OnNext(contextView);
				yield return StartCoroutine(WaitForReady(contextView));
			}
			//if(screenEffect != null) {
			//	yield return screenEffect.Remove().StartAsCoroutine();
			//}
			LoadingLevel = false;
			observer.OnCompleted();
		}

		private IEnumerator WaitForContextView(Action<IContextView> contextView) {
			var gameObject = default(GameObject);
			var counter = 0f;
			while(gameObject == null) {
				counter += Time.deltaTime;
				gameObject = GameObject.Find("_" + Application.loadedLevelName + "ContextView");
				if(counter > 3f) {
					break;
				}
				yield return null;
			}
			contextView(gameObject.GetComponent(typeof(IContextView)) as IContextView);
		}

		private IEnumerator WaitForReady(IContextView contextView) {
			var concreteContextView = contextView as ContextView;
			var counter = 0f;
			while(concreteContextView == null || !concreteContextView.IsReady) {
				counter += Time.deltaTime;
				if(counter > 3f) {
					break;
				}
				yield return null;
			}
		}
	}
}
