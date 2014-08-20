using System;

namespace Inconspicuous.Framework {
	public enum Reuse {
		Singleton,
		Transient
	}

	public interface IContainer : IDisposable {
		IContainer Parent { get; set; }
		void Register<TService, TImplementation>(Reuse reuse = Reuse.Transient);
		void Register<TService>(Type implementation, Reuse reuse = Reuse.Transient);
		void Register(Type service, Type implementation, Reuse reuse = Reuse.Transient);
		void Register<TService>(TService instance);
		void Register(Type service, object instance);
		void Register<TService>(Func<TService> factory);
		void Register(Type service, Func<object> factory);
		void RegisterDecorator<TService, TDecorator>();
		void RegisterDecorator<TService>(Type decorator);
		void RegisterDecorator(Type service, Type decorator);
		TService Resolve<TService>();
		object Resolve(Type service, int depth = 0);
		void Inject(object instance);
	}
}
