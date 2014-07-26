using System.ComponentModel.Composition;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public class QuitApplicationCommand : ICommand<NullResult> { }

	[Export(typeof(ICommandHandler<QuitApplicationCommand, NullResult>))]
	public class QuitApplicationCommandHandler : CommandHandler<QuitApplicationCommand, NullResult> {
		public override IObservable<NullResult> Handle(QuitApplicationCommand macroCommand) {
			Application.Quit();
			return Observable.Return(NullResult.Default);
		}
	}
}
