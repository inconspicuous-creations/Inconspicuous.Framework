using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public class MainContext : Context {
		private readonly string sceneName;

		public MainContext(IContextView contextView, string sceneName)
			: base(contextView) {
			this.sceneName = sceneName;
			ContextConfiguration.Default.Configure(container);
			container.Register<IMainContextView>(contextView as IMainContextView);
			RegisterExports();
			try {
				container.Resolve<IMainContextConfiguration>().Configure(container);
			} catch {
				// Do nothing.
			}
		}

		public override void Start() {
			container.Resolve<IContextView>().Persist();
			try {
				container.Resolve<IMainContextConfiguration>().Start();
			} catch {
				// Do nothing.
			}
			container.Resolve<IViewMediationBinder>().Mediate(container.Resolve<IContextView>());
			container.Resolve<ICommandDispatcher>().Dispatch(new StartCommand()).Subscribe().AddTo(container.Resolve<IContextView>().GameObject);
			if(Application.loadedLevelName == "Main") {
				container.Resolve<ICommandDispatcher>().Dispatch(new LoadSceneCommand { SceneName = sceneName }).Subscribe().AddTo(container.Resolve<IContextView>().GameObject);
			}
		}
	}
}
