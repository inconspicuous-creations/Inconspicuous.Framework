using System;
using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public abstract class ContextView : View, IContextView {
		public IContext Context { get; protected set; }

		public sealed override void Start() {
			base.Start();
			Initialize();
		}

		protected T Initialize<T>(params IContext[] subContexts) where T : class, IContext {
			return Initialize(typeof(T), subContexts) as T;
		}

		protected IContext Initialize(Type type, params IContext[] subContexts) {
			if(!typeof(IContext).IsAssignableFrom(type)) {
				throw new ArgumentException("Not a context: " + type.FullName);
			}
			return Activator.CreateInstance(type, new object[] { this, subContexts.Cast<Context>().ToArray() }) as IContext;
		}

		protected bool CheckAndRemoveDuplicate() {
			var dupes = GameObject.FindObjectsOfType(typeof(ContextView))
				.Where(g => g.name == gameObject.name).ToList().Count;
			if(dupes >= 2) {
				Destroy(gameObject);
				return true;
			}
			return false;
		}

		protected IObservable<IContext> LoadSceneForContext(Type contextType) {
			var sceneName = contextType.GetCustomAttributes(false).OfType<SceneAttribute>().First().SceneName;
			return Observable.FromCoroutine<IContext>(observer => DoLoadSceneForContext(observer, sceneName)).DelayFrame(1);
		}

		private IEnumerator DoLoadSceneForContext(IObserver<IContext> observer, string sceneName) {
			yield return Application.LoadLevelAdditiveAsync(sceneName);
			IContextView contextView = null;
			while(contextView == null) {
				var gameObject = GameObject.Find("_" + sceneName + "ContextView");
				if(gameObject != null) {
					contextView = gameObject.GetComponent(typeof(IContextView)) as IContextView;
				}
				yield return null;
			}
			observer.OnNext(contextView.Context);
			observer.OnCompleted();
		}
	}
}
