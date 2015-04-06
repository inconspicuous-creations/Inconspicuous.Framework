using System.Collections.Generic;
using System.ComponentModel.Composition;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inconspicuous.Framework {
	public class LoadSceneCommand : ICommand<IContextView> {
		public string SceneName { get; set; }
		public ICollection<IContext> SubContexts { get; set; }
	}

	public class LoadSceneCommandHandler : CommandHandler<LoadSceneCommand, IContextView> {
		private readonly ILevelManager levelManager;

		public LoadSceneCommandHandler(ILevelManager levelManager) {
			this.levelManager = levelManager;
		}

		public override IObservable<IContextView> Handle(LoadSceneCommand command) {
			return levelManager.Load(command.SceneName).Do(contextView => {
				var cv = contextView as ContextView;
				if(cv != null && command.SubContexts != null) {
					foreach(var additionalContext in command.SubContexts) {
						cv.SubContexts.Add(additionalContext);
					}
				}
				var eventSystemGameObject = GameObject.Find("EventSystem");
				EventSystem.current = eventSystemGameObject != null ? eventSystemGameObject.GetComponent<EventSystem>() : null;
			});
		}
	}
}
