using System;
using UniRx;

namespace Inconspicuous.Framework {
	public abstract class CommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult>
		where TCommand : class, ICommand
		where TResult : class {
		public abstract IObservable<TResult> Handle(TCommand command);

		public IObservable<object> Handle(ICommand command) {
			var convertedCommand = command as TCommand;
			if(convertedCommand != null) {
				return Handle(convertedCommand as TCommand).Cast<TResult, object>();
			} else {
				throw new CommandHandlerMismatchException(this, command);
			}
		}

		[Serializable]
		private class CommandHandlerMismatchException : CustomException {
			public CommandHandlerMismatchException(ICommandHandler commandHandler, ICommand command)
				: base("Command handler mismatch: {0}, {1}", commandHandler.GetType(), command.GetType()) { }
		}
	}
}
