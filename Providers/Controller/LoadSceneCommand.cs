using System.Collections.Generic;
using System.ComponentModel.Composition;
using UniRx;

namespace Inconspicuous.Framework {
	public class LoadSceneCommand : ICommand<LoadSceneResult> {
		public string SceneName { get; set; }
		public ICollection<IContext> SubContexts { get; set; }
	}

	public class LoadSceneResult : IResult {
		public IContextView ContextView { get; set; }
	}

	[Export(typeof(ICommandHandler<LoadSceneCommand, LoadSceneResult>))]
	public class LoadSceneCommandHandler : CommandHandler<LoadSceneCommand, LoadSceneResult> {
		private readonly ILevelManager levelManager;

		public LoadSceneCommandHandler(ILevelManager levelManager) {
			this.levelManager = levelManager;
		}

		public override IObservable<LoadSceneResult> Handle(LoadSceneCommand command) {
			var observable = levelManager.Load(command.SceneName);
			observable.Subscribe(contextView => {
				var cv = contextView as ContextView;
				if(cv != null && command.SubContexts != null) {
					foreach(var additionalContext in command.SubContexts) {
						cv.SubContexts.Add(additionalContext);
					}
				}
			});
			return observable.Select(contextView => new LoadSceneResult {
				ContextView = contextView
			});
		}
	}
}
