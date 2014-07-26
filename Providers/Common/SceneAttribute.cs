using System;

namespace Inconspicuous.Framework {
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class SceneAttribute : Attribute {
		private readonly string sceneName;

		public SceneAttribute(string sceneName) {
			this.sceneName = sceneName;
		}

		public string SceneName {
			get { return sceneName; }
		}
	}
}
