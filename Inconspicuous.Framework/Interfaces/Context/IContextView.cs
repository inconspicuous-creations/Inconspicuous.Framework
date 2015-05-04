namespace Inconspicuous.Framework {
	public interface IContextView : IView {
		IContext Context { get; }
		void Persist();
	}
}
