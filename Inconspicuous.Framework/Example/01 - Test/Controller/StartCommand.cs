using System.ComponentModel.Composition;
using UniRx;

namespace Inconspicuous.Framework.Example.Test {
	public class StartCommand : ICommand<Unit> { }

	[Export(typeof(ICommandHandler<StartCommand, Unit>))]
	public class StartCommandHandler : CommandHandler<StartCommand, Unit> {
		private readonly ICommandDispatcher commandDispatcher;

		public StartCommandHandler(ICommandDispatcher commandDispatcher) {
			this.commandDispatcher = commandDispatcher;
		}

		public override IObservable<Unit> Handle(StartCommand command) {
			return commandDispatcher.Dispatch(new MacroCommand(MacroCommandType.Sequence) {
				new TestCommand {
					FirstValue = 89654561,
					SecondValue = "Test"
				}
			}).Select(_ => Unit.Default);
		}
	}
}
