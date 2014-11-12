using System.ComponentModel.Composition;
using System.Linq;
using UniRx;

namespace Inconspicuous.Framework {
	public abstract class Context : IContext {
		protected readonly IContainer container;
		private readonly CompositeDisposable disposable;

		protected Context() {
			container = new Container();
			disposable = new CompositeDisposable();
			container.Register<IContext>(this);
			container.Register<IContainer>(container);
		}

		protected Context(IContextView contextView, params Context[] subContexts)
			: this() {
			container.Register<IContextView>(contextView);
			RegisterExports();
			RegisterSubContexts(subContexts);
		}

		public abstract void Start();

		public void Dispose() {
			disposable.Dispose();
			container.Dispose();
		}

		protected void RegisterExports() {
			var types = GetType().Assembly.GetExportedTypes().Where(t => t.GetCustomAttributes(typeof(ExportAttribute), true).Any()).ToList();
			foreach(var type in types) {
				foreach(var export in type.GetCustomAttributes(typeof(ExportAttribute), true).Cast<ExportAttribute>()) {
					var singleton = type.GetCustomAttributes(typeof(PartCreationPolicyAttribute), true).Any() ?
						type.GetCustomAttributes(typeof(PartCreationPolicyAttribute), true).OfType<PartCreationPolicyAttribute>()
							.Select(p => p.CreationPolicy != CreationPolicy.NonShared).First() : true;
					var service = export.ContractType != null ? export.ContractType : type;
					container.Register(service, type, singleton ? Reuse.Singleton : Reuse.Transient);
				}
			}
		}

		private void RegisterSubContexts(Context[] subContexts) {
			IContainer lastContainer = null;
			foreach(var subContext in subContexts) {
				if(lastContainer != null) {
					if(subContext.container.Parent == null) {
						subContext.container.Parent = lastContainer;
					}
				}
				lastContainer = subContext.container;
			}
			if(lastContainer != null) {
				container.Parent = lastContainer;
				subContexts.First().container.Parent = container;
			}
		}

		public static implicit operator CompositeDisposable(Context context) {
			return context.disposable;
		}
	}
}
