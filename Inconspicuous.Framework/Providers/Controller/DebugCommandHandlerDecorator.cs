using UniRx;

namespace Inconspicuous.Framework {
	public class DebugCommandHandlerDecorator<TCommand, TResult> : CommandHandler<TCommand, TResult>
		where TCommand : class, ICommand<TResult>
		where TResult : class {
		private readonly ICommandHandler<TCommand, TResult> original;

		public DebugCommandHandlerDecorator(ICommandHandler<TCommand, TResult> original) {
			this.original = original;
			UnityEngine.Debug.Log(string.Format("{0}... created handler", typeof(TCommand)));
		}

		public override IObservable<TResult> Handle(TCommand command) {
			UnityEngine.Debug.Log(string.Format("{0}... initialized", typeof(TCommand)));
			return Observable.Defer(() => {
				UnityEngine.Debug.Log(string.Format("{0}... started", typeof(TCommand)));
				return original.Handle(command)
					.Dump()
					.Finally(() => UnityEngine.Debug.Log(string.Format("{0}... stopped", typeof(TCommand))));
			});
		}
	}
}
