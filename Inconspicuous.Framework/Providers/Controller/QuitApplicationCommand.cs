using System;
using System.ComponentModel.Composition;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public class QuitApplicationCommand : ICommand<Unit> { }

	public class QuitApplicationCommandHandler : CommandHandler<QuitApplicationCommand, Unit> {
		private const string webPlayerQuitUrl = "http://www.inconspicuous.no";

		public override IObservable<Unit> Handle(QuitApplicationCommand macroCommand) {
			if(Application.isEditor) {
				var editorApplicationType = AppDomain.CurrentDomain.GetAssemblies()
					.Select(x => x.GetType("UnityEditor.EditorApplication"))
					.Where(x => x != null)
					.FirstOrDefault();
				if(editorApplicationType != null) {
					editorApplicationType.GetProperty("isPlaying").SetValue(null, false, null);
				}
			} else if(Application.isWebPlayer) {
				Application.OpenURL(webPlayerQuitUrl);
			} else {
				Application.Quit();
			}
			return Observable.Return(Unit.Default);
		}
	}
}
