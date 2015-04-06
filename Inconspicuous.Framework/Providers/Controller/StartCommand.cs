using System.Collections;
using System.ComponentModel.Composition;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public class StartCommand : ICommand<Unit> { }

	public class StartCommandHandler : CommandHandler<StartCommand, Unit> {
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

		public override IObservable<Unit> Handle(StartCommand command) {
			if(applicationManager.DebugMode) {
				return Observable.Create<Unit>(observer => {
					contextScheduler.StartCoroutine(DoCheckKey());
					return Disposable.Empty;
				});
			}
			return Observable.Return(Unit.Default);
		}

		private IEnumerator DoCheckKey() {
			while(true) {
				if(UnityEngine.Input.GetKeyDown(KeyCode.F2)) {
					commandDispatcher.Dispatch(new RestartSceneCommand()).Subscribe();
				}
				yield return null;
			}
		}
	}
}
