using UniRx;

namespace Inconspicuous.Framework {
	public interface ICommandDispatcher {
		IObservable<object> Dispatch(ICommand command);
		IObservable<TResult> Dispatch<TResult>(ICommand<TResult> command) where TResult : class;
		IObservable<TResult> AsObservable<TResult>() where TResult : class;
	}
}
