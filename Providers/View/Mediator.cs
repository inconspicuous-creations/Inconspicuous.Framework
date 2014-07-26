using UniRx;

namespace Inconspicuous.Framework {
	public abstract class Mediator<T> : IMediator<T> where T : class, IView {
		private CompositeDisposable disposable;

		public Mediator() {
			disposable = new CompositeDisposable();
		}

		public abstract void Mediate(T view);

		public void Mediate(IView view) {
			Mediate(view as T);
		}

		public void Dispose() {
			disposable.Dispose();
		}

		public static implicit operator CompositeDisposable(Mediator<T> mediator) {
			return mediator.disposable;
		}
	}
}
