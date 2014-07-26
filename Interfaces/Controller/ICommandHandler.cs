using UniRx;

namespace Inconspicuous.Framework {
	public interface ICommandHandler {
		IObservable<IResult> Handle(ICommand command);
	}

	public interface ICommandHandler<TCommand, TResult> : ICommandHandler
		where TCommand : ICommand
		where TResult : IResult {
		IObservable<TResult> Handle(TCommand command);
	}
}
