#pragma warning disable 0649

using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public sealed class CustomContextView : ContextView {
		[SerializeField]
		[Type(typeof(IContext))]
		private string contextType;

		[SerializeField]
		[Type(typeof(IContext))]
		private string[] subContextTypes;

		public void Start() {
			if(!CheckAndRemoveDuplicate()) {
				Observable.Zip(WaitForMainContext(), WaitForReady(), (x, _) => x)
					.Subscribe(mainContext => {
						if(subContextTypes.Length == 0) {
							var type = Type.GetType(contextType);
							Context = Initialize(type, new[] { mainContext });
							var sceneName = type.GetCustomAttributes(false).OfType<SceneAttribute>().First().SceneName;
							if(sceneName == Application.loadedLevelName) {
								Context.Start();
							}
						} else {
							var loadingSubContexts = subContextTypes
								.Select(t => LoadSceneForContext(Type.GetType(t))).ToArray();
							Observable.WhenAll(loadingSubContexts)
								.Select(subContexts => subContexts.Cast<Context>().ToArray())
								.ObserveOnMainThread()
								.Subscribe(subContexts => {
									Context = Initialize(Type.GetType(contextType), new[] { mainContext }.Concat(subContexts).ToArray());
									foreach(var subContext in subContexts) {
										subContext.Start();
									}
									Context.Start();
								}).AddTo(this);
						}
					});
			}
		}

		private IObservable<IContext> WaitForMainContext() {
			var view = GameObject.Find("_MainContextView");
			if(view == null) {
				return LoadSceneForContext(typeof(MainContextView));
			}
			return Observable.Return(view.GetComponent<MainContextView>().Context);
		}

		private IObservable<long> WaitForReady() {
			return Observable.EveryUpdate()
				.Where(_ => IsReady)
				.First();
		}
	}
}
