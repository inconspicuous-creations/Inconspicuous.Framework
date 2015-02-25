namespace Inconspicuous.Framework {
	public abstract class Mediator<T> : IMediator<T> where T : class, IView {
		public abstract void Mediate(T view);

		public void Mediate(IView view) {
			Mediate(view as T);
		}
	}
}
