using System.ComponentModel.Composition;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public class RestartSceneCommand : ICommand<LoadSceneResult> { }

	[Export(typeof(ICommandHandler<RestartSceneCommand, LoadSceneResult>))]
	public class RestartSceneCommandHandler : CommandHandler<RestartSceneCommand, LoadSceneResult> {
		private readonly ICommandDispatcher commandDispatcher;

		public RestartSceneCommandHandler(ICommandDispatcher commandDispatcher) {
			this.commandDispatcher = commandDispatcher;
		}

		public override IObservable<LoadSceneResult> Handle(RestartSceneCommand command) {
			return commandDispatcher.Dispatch(new LoadSceneCommand {
				SceneName = Application.loadedLevelName
			});
		}
	}
}
