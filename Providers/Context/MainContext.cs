using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public class MainContext : Context {
		private string sceneName;

		public MainContext(IContextView contextView, string sceneName)
			: base() {
			this.sceneName = sceneName;
			RegisterExports();
			container.Register<IContextView>(contextView);
			container.Resolve<IViewMediationBinder>().Mediate(contextView);
		}

		public override void Start() {
			container.Resolve<ICommandDispatcher>().Dispatch(new StartCommand());
			if(Application.loadedLevelName == "Main") {
				container.Resolve<ICommandDispatcher>().Dispatch(new LoadSceneCommand { SceneName = sceneName }).Subscribe();
			}
		}
	}
}
