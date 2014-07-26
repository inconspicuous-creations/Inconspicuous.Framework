using UniRx;

namespace Inconspicuous.Framework {
	public interface ICommandDispatcher {
		IObservable<IResult> Dispatch(ICommand command);
		IObservable<TResult> Dispatch<TResult>(ICommand<TResult> command) where TResult : class, IResult;
		IObservable<TResult> AsObservable<TResult>() where TResult : class, IResult;
	}
}
