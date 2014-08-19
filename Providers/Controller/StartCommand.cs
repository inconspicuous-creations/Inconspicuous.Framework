using System.ComponentModel.Composition;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public class StartCommand : ICommand<NullResult> { }

	[Export(typeof(ICommandHandler<StartCommand, NullResult>))]
	public class StartCommandHandler : CommandHandler<StartCommand, NullResult> {
		private readonly IApplicationManager applicationManager;
		private readonly ICommandDispatcher commandDispatcher;
		private readonly IContextScheduler contextScheduler;

		public StartCommandHandler(
			IApplicationManager applicationManager,
			ICommandDispatcher commandDispatcher,
			IContextScheduler contextScheduler) {
			this.applicationManager = applicationManager;
			this.commandDispatcher = commandDispatcher;
			this.contextScheduler = contextScheduler;
		}

		public override IObservable<NullResult> Handle(StartCommand command) {
			Observable.EveryUpdate()
				.ObserveOn(contextScheduler)
				.Subscribe(_ => {
					if(applicationManager.DebugMode) {
						if(UnityEngine.Input.GetKeyDown(KeyCode.F2)) {
							commandDispatcher.Dispatch(new RestartSceneCommand());
						}
					}
				});
			return Observable.Return(NullResult.Default);
		}
	}
}
