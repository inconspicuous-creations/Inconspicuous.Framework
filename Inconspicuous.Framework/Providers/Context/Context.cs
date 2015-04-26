using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using UniRx;

namespace Inconspicuous.Framework {
	public abstract class Context : IContext {
		protected readonly IContainer container;
		private readonly BooleanDisposable disposable;

		protected Context() {
			this.disposable = new BooleanDisposable();
			this.container = new Container();
			container.Register<IContext>(this);
			container.Register<IContainer>(container);
		}

		protected Context(IContextView contextView)
			: this() {
			container.Register<IContextView>(contextView);
		}

		protected Context(IContextView contextView, Context[] subContexts, bool autoRegisterExports = true)
			: this(contextView) {
			if(autoRegisterExports) {
				RegisterExports();
			}
			RegisterSubContexts(subContexts);
		}

		public abstract void Start();

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if(!disposable.IsDisposed) {
				if(disposing) {
					container.Dispose();
				}
				disposable.Dispose();
			}
		}

		protected void RegisterExports(Assembly[] assemblies = null) {
			var types = (assemblies ?? AppDomain.CurrentDomain.GetAssemblies())
				.Where(x => !(x is System.Reflection.Emit.AssemblyBuilder) && x.GetType().FullName != "System.Reflection.Emit.InternalAssemblyBuilder")
				.SelectMany(x => x.GetExportedTypes().Where(t => t.GetCustomAttributes(typeof(ExportAttribute), true).Any()).ToList()).ToList();
			foreach(var type in types) {
				foreach(var export in type.GetCustomAttributes(typeof(ExportAttribute), true).Cast<ExportAttribute>()) {
					var serviceType = export.ContractType != null ? export.ContractType : type;
					var isSingleton = type.GetCustomAttributes(typeof(PartCreationPolicyAttribute), true).Any() ?
						type.GetCustomAttributes(typeof(PartCreationPolicyAttribute), true).OfType<PartCreationPolicyAttribute>()
							.Select(p => p.CreationPolicy != CreationPolicy.NonShared).First() : true;
					container.Register(serviceType, type, isSingleton ? Reuse.Singleton : Reuse.Transient);
				}
			}
		}

		private void RegisterSubContexts(Context[] subContexts) {
			var lastContainer = default(IContainer);
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
	}
}
