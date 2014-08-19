using DryIoc.MefAttributedModel;
using MugenInjection.Activators;
using MugenInjection.Core;
using UniRx;
using Container = MugenInjection.MugenInjector;

namespace Inconspicuous.Framework {
	public abstract class Context : IContext {
		protected readonly Container container;
		private readonly CompositeDisposable disposable;

		protected Context() {
			container = new Container(new DefaultInjectorSetting {
				IsAutoScanAssembly = false,
				DefaultActivatorFactory = () => new ReflectionActivator()
			});
			disposable = new CompositeDisposable();
			container.Bind<IContext>().ToConstant(this);
			container.Bind<Container>().ToConstant(container);
		}

		protected Context(IContextView contextView, params Context[] subContexts)
			: this() {
			container.Bind<IContextView>().ToConstant(contextView);
			RegisterExports();
			RegisterSubContexts(subContexts);
		}

		public abstract void Start();

		public void Dispose() {
			disposable.Dispose();
			container.Dispose();
		}

		protected void RegisterExports() {
			var exports = AttributedModel.DiscoverExportsInAssemblies(new[] { GetType().Assembly });
			foreach(var export in exports) {
				foreach(var e in export.Exports) {
					var binding = container.Bind(e.ServiceType).To(export.Type);
					if(export.IsSingleton) {
						binding.InSingletonScope();
					}
					if(e.ServiceType.IsGenericType && e.ServiceType.GetGenericTypeDefinition() == typeof(IFactory<>)) {
						foreach(var t in e.ServiceType.GetGenericArguments()) {
							container.Bind(t).ToMethod(_ => {
								var factory = container.Get(e.ServiceType);
								return factory.GetType().GetMethod("Create").Invoke(factory, null);
							});
						}
					}
				}
			}
		}

		private void RegisterSubContexts(Context[] subContexts) {
			Container lastContainer = null;
			foreach(var subContext in subContexts) {
				if(lastContainer != null) {
					if(subContext.container != container.GetRoot()) {
						lastContainer.AddChild(subContext.container);
					}
				}
				lastContainer = subContext.container;
			}
			if(lastContainer != null) {
				lastContainer.AddChild(container.GetRoot());
			}
		}

		public static implicit operator CompositeDisposable(Context context) {
			return context.disposable;
		}
	}
}
