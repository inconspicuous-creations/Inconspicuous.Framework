#pragma warning disable 0649

using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	[Scene("Main")]
	public sealed class MainContextView : ContextView, IMainContextView {
		[SerializeField]
		[Type(typeof(IContext))]
		private string firstContext;

		public Subject<Unit> ApplicationResumedSignal { get; private set; }

		[Inject]
		public void Construct() {
			this.ApplicationResumedSignal = new Subject<Unit>();
		}

		public void Start() {
			if(!CheckAndRemoveDuplicate()) {
				var sceneName = Type.GetType(firstContext)
					.GetCustomAttributes(false)
					.OfType<SceneAttribute>().First().SceneName;
				Context = new MainContext(this, sceneName);
				Context.Start();
			}
		}

		public void OnApplicationPause(bool paused) {
			if(!paused) {
				ApplicationResumedSignal = ApplicationResumedSignal ?? new Subject<Unit>();
				ApplicationResumedSignal.OnNext(Unit.Default);
			}
		}
	}
}
