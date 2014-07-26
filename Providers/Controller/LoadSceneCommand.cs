using System.ComponentModel.Composition;
using UniRx;

namespace Inconspicuous.Framework {
	public class LoadSceneCommand : ICommand<LoadSceneResult> {
		public string SceneName { get; set; }
	}

	public class LoadSceneResult : IResult {
		public string SceneName { get; set; }
	}

	[Export(typeof(ICommandHandler<LoadSceneCommand, LoadSceneResult>))]
	public class LoadSceneCommandHandler : CommandHandler<LoadSceneCommand, LoadSceneResult> {
		private readonly IContextScheduler contextScheduler;
		private readonly ILevelManager levelManager;

		public LoadSceneCommandHandler(IContextScheduler contextScheduler, ILevelManager levelManager) {
			this.contextScheduler = contextScheduler;
			this.levelManager = levelManager;
		}

		public override IObservable<LoadSceneResult> Handle(LoadSceneCommand command) {
			return Observable.Start(() => {
				levelManager.Load(command.SceneName);
			}, contextScheduler).Select(_ => new LoadSceneResult {
				SceneName = command.SceneName
			});
		}
	}
}
