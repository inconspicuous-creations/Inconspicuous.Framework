using DryIoc;
using DryIoc.MefAttributedModel;
using UniRx;

namespace Inconspicuous.Framework {
	public abstract class Context : IContext {
		protected readonly Container container;
		private readonly CompositeDisposable disposable;

		protected Context(IContextView contextView, params Context[] subContexts) {
			container = new Container();
			disposable = new CompositeDisposable();
			container.RegisterInstance<IContext>(this);
			container.RegisterInstance<IContextView>(contextView);
			container.RegisterInstance<Container>(container);
			container.RegisterExports(AttributedModel.DiscoverExportsInAssemblies(new[] { GetType().Assembly }));
			foreach(var subContext in subContexts) {
				container.ResolveUnregisteredFrom(subContext.container);
			}
		}

		public abstract void Start();

		public void Dispose() {
			disposable.Dispose();
			container.Dispose();
		}

		public static implicit operator CompositeDisposable(Context context) {
			return context.disposable;
		}
	}
}
