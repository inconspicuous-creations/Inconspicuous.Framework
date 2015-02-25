using System.ComponentModel.Composition;
using UniRx;

namespace Inconspicuous.Framework {
	public class QuitApplicationCommand : ICommand<Unit> { }

	[Export(typeof(ICommandHandler<QuitApplicationCommand, Unit>))]
	public class QuitApplicationCommandHandler : CommandHandler<QuitApplicationCommand, Unit> {
#if UNITY_WEBPLAYER
		private readonly static string webPlayerQuitUrl = "http://www.inconspicuous.no";
#endif

		public override IObservable<Unit> Handle(QuitApplicationCommand macroCommand) {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
			UnityEngine.Application.OpenURL(webPlayerQuitUrl);
#else
			UnityEngine.Application.Quit();
#endif
			return Observable.Return(Unit.Default);
		}
	}
}
