using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using UniRx;

namespace Inconspicuous.Framework {
	public enum MacroCommandType {
		Parallel,
		Sequence
	}

	public class MacroCommand : List<ICommand>, ICommand<MacroResult> {
		public MacroCommand() { }

		public MacroCommand(MacroCommandType macroCommandType) {
			MacroCommandType = macroCommandType;
		}

		public MacroCommandType MacroCommandType { get; set; }
	}

	public class MacroResult : List<IResult>, IResult {
		public MacroResult() { }

		public MacroResult(ICollection<IResult> results) {
			AddRange(results);
		}
	}

	[Export(typeof(ICommandHandler<MacroCommand, MacroResult>))]
	public class MacroCommandHandler : CommandHandler<MacroCommand, MacroResult> {
		private readonly ICommandDispatcher commandDispatcher;
		private readonly IContextScheduler contextScheduler;

		public MacroCommandHandler(ICommandDispatcher commandDispatcher, IContextScheduler contextScheduler) {
			this.commandDispatcher = commandDispatcher;
			this.contextScheduler = contextScheduler;
		}

		public override IObservable<MacroResult> Handle(MacroCommand command) {
			if(command.Count > 0) {
				switch(command.MacroCommandType) {
					case MacroCommandType.Parallel:
						var results = command.Select(c => commandDispatcher.Dispatch(c)).ToArray();
						return Observable.WhenAll(results).ObserveOn(contextScheduler).Select(r => new MacroResult(r));
					case MacroCommandType.Sequence:
						var queue = new ObservableQueue<IResult>(contextScheduler);
						queue.AddRange(command.Select(c => Observable.Defer(() => commandDispatcher.Dispatch(c))));
						return queue.Buffer(command.Count).Select(r => new MacroResult(r));
				}
			}
			return Observable.Return(new MacroResult());
		}
	}
}
