using System.ComponentModel.Composition;
using UniRx;

namespace Inconspicuous.Framework.Example.Test {
	public class StartCommand : ICommand<NullResult> { }

	[Export(typeof(ICommandHandler<StartCommand, NullResult>))]
	public class StartCommandHandler : CommandHandler<StartCommand, NullResult> {
		private readonly ICommandDispatcher commandDispatcher;

		public StartCommandHandler(ICommandDispatcher commandDispatcher) {
			this.commandDispatcher = commandDispatcher;
		}

		public override IObservable<NullResult> Handle(StartCommand command) {
			return commandDispatcher.Dispatch(new MacroCommand(MacroCommandType.Sequence) {
				new TestCommand {
					FirstValue = 89654561,
					SecondValue = "Test"
				}
			}).Select(_ => NullResult.Default);
		}
	}
}
