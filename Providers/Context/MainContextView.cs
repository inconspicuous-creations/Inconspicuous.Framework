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

		public override void Start() {
			base.Start();
			if(!CheckAndRemoveDuplicate()) {
				Persist();
				var sceneName = Type.GetType(firstContext)
					.GetCustomAttributes(false)
					.OfType<SceneAttribute>().First().SceneName;
				var context = new MainContext(this, sceneName);
				context.Start();
			}
		}
	}
}
