using System.ComponentModel.Composition;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public class QuitApplicationCommand : ICommand<Unit> { }

	[Export(typeof(ICommandHandler<QuitApplicationCommand, Unit>))]
	public class QuitApplicationCommandHandler : CommandHandler<QuitApplicationCommand, Unit> {
		public override IObservable<Unit> Handle(QuitApplicationCommand macroCommand) {
			Application.Quit();
			return Observable.Return(Unit.Default);
		}
	}
}
