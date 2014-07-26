using UniRx;

namespace Inconspicuous.Framework {
	public class NullResult : IResult {
		public static NullResult Default = new NullResult();
	}

	public abstract class CommandHandler<TCommand, TResult> : ICommandHandler<TCommand, TResult>
		where TCommand : class, ICommand
		where TResult : class, IResult {
		public abstract IObservable<TResult> Handle(TCommand command);

		public IObservable<IResult> Handle(ICommand command) {
			var convertedCommand = command as TCommand;
			if(convertedCommand != null) {
				return Handle(convertedCommand as TCommand).Cast<TResult, IResult>();
			} else {
				throw new CommandHandlerMismatchException(this, command);
			}
		}

		private class CommandHandlerMismatchException : CustomException {
			public CommandHandlerMismatchException(ICommandHandler commandHandler, ICommand command)
				: base("Command handler mismatch: {0}, {1}", commandHandler.GetType(), command.GetType()) { }
		}
	}
}
