using UniRx;

namespace Inconspicuous.Framework {
	public interface ICommandHandler {
		IObservable<object> Handle(ICommand command);
	}

	public interface ICommandHandler<TCommand, TResult> : ICommandHandler
		where TCommand : ICommand {
		IObservable<TResult> Handle(TCommand command);
	}
}
