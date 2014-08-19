#pragma warning disable 0649

using System;
using System.Linq;
using UnityEngine;

namespace Inconspicuous.Framework {
	[Scene("Main")]
	public sealed class MainContextView : ContextView {
		[SerializeField]
		[Type(typeof(IContext))]
		private string firstContext;

		public override void Initialize() {
			if(!CheckAndRemoveDuplicate()) {
				GameObject.DontDestroyOnLoad(GameObject);
				var sceneName = Type.GetType(firstContext)
					.GetCustomAttributes(false)
					.OfType<SceneAttribute>().First().SceneName;
				var context = new MainContext(sceneName);
				context.Start();
			}
		}
	}
}
