namespace Inconspicuous.Framework {
	public interface IMediator {
		void Mediate(IView view);
	}

	public interface IMediator<T> : IMediator where T : IView {
		void Mediate(T view);
	}

	public interface IViewMediationBinder {
		void Mediate(IView view);
	}
}
