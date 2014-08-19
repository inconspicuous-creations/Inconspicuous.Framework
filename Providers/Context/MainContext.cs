using UnityEngine;

namespace Inconspicuous.Framework {
	public class MainContext : Context {
		private string sceneName;

		public MainContext(IContextView contextView, string sceneName)
			: base() {
			this.sceneName = sceneName;
			if(Mathf.Max(Screen.width, Screen.height) > 1280) {
				tk2dSystem.CurrentPlatform = "4x";
			} else if(Mathf.Max(Screen.width, Screen.height) > 800) {
				tk2dSystem.CurrentPlatform = "2x";
			} else {
				tk2dSystem.CurrentPlatform = "1x";
			}
			RegisterExports();
			container.Bind<IContextView>().ToConstant(contextView);
			container.Get<IViewMediationBinder>().Mediate(contextView);
		}

		public override void Start() {
			container.Get<ICommandDispatcher>().Dispatch(new StartCommand());
			container.Get<ICommandDispatcher>().Dispatch(new LoadSceneCommand { SceneName = sceneName });
		}
	}
}
