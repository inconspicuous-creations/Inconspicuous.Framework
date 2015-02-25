using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using UniRx;

namespace Inconspicuous.Framework {
	public enum MacroCommandType {
		Parallel,
		Sequence
	}

	public class MacroCommand : List<ICommand>, ICommand<ICollection<object>> {
		public MacroCommand() { }

		public MacroCommand(MacroCommandType macroCommandType) {
			MacroCommandType = macroCommandType;
		}

		public MacroCommandType MacroCommandType { get; set; }
	}

	[Export(typeof(ICommandHandler<MacroCommand, ICollection<object>>))]
	public class MacroCommandHandler : CommandHandler<MacroCommand, ICollection<object>> {
		private readonly ICommandDispatcher commandDispatcher;
		private readonly IContextScheduler contextScheduler;

		public MacroCommandHandler(ICommandDispatcher commandDispatcher, IContextScheduler contextScheduler) {
			this.commandDispatcher = commandDispatcher;
			this.contextScheduler = contextScheduler;
		}

		public override IObservable<ICollection<object>> Handle(MacroCommand command) {
			if(command.Count > 0) {
				switch(command.MacroCommandType) {
					case MacroCommandType.Parallel:
						var results = command.Select(c => commandDispatcher.Dispatch(c)).ToArray();
						return Observable.WhenAll(results).ObserveOn(contextScheduler).Select(r => r.ToList() as ICollection<object>);
					case MacroCommandType.Sequence:
						var queue = new ObservableQueue<object>(contextScheduler);
						queue.AddRange(command.Select(c => Observable.Defer(() => commandDispatcher.Dispatch(c))));
						return queue.Buffer(command.Count).Select(r => r.ToList() as ICollection<object>);
				}
			}
			return Observable.Return<ICollection<object>>(new List<object>());
		}
	}
}
