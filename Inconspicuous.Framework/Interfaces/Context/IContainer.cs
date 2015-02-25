using System;
using System.Collections.Generic;

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
		void RegisterDecorator<TService, TDecorator>(Reuse reuse = Reuse.Transient);
		void RegisterDecorator<TService>(Type decorator, Reuse reuse = Reuse.Transient);
		void RegisterDecorator(Type service, Type decorator, Reuse reuse = Reuse.Transient);
		TService Resolve<TService>(bool canReturnNull = false) where TService : class;
		object Resolve(Type service, bool canReturnNull = false, ICollection<IContainer> searched = null);
		void Inject(object instance);
	}
}
