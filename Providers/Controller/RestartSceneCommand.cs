using System.ComponentModel.Composition;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public class RestartSceneCommand : ICommand<LoadSceneResult> { }

	[Export(typeof(ICommandHandler<RestartSceneCommand, LoadSceneResult>))]
	public class RestartSceneCommandHandler : CommandHandler<RestartSceneCommand, LoadSceneResult> {
		private readonly ICommandDispatcher commandDispatcher;
		private readonly IContextView contextView;

		public RestartSceneCommandHandler(ICommandDispatcher commandDispatcher, IContextView contextView) {
			this.commandDispatcher = commandDispatcher;
			this.contextView = contextView;
		}

		public override IObservable<LoadSceneResult> Handle(RestartSceneCommand command) {
			return commandDispatcher.Dispatch(new LoadSceneCommand {
				SceneName = Application.loadedLevelName,
				SubContexts = contextView is ContextView ? (contextView as ContextView).SubContexts : null
			});
		}
	}
}
