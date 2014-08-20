using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inconspicuous.Framework {
	public class Container : IContainer {
		private const int maxSearchDepth = 20;

		private IContainer parent;
		private List<IContainer> children;
		private Dictionary<object, Type> genericMap;
		private Dictionary<object, Func<object>> serviceMap;

		public Container() {
			children = new List<IContainer>();
			genericMap = new Dictionary<object, Type>();
			serviceMap = new Dictionary<object, Func<object>>();
		}

		public IContainer Parent {
			get { return parent; }
			set {
				var concreteOldParent = parent as Container;
				if(concreteOldParent != null) {
					concreteOldParent.children.Remove(this);
				}
				parent = value;
				var concreteParent = value as Container;
				if(concreteParent != null) {
					concreteParent.children.Add(this);
				}
			}
		}

		public void Register<TService, TImplementation>(Reuse reuse = Reuse.Transient) {
			Register(typeof(TService), typeof(TImplementation), reuse);
		}

		public void Register<TService>(Type implementation, Reuse reuse = Reuse.Transient) {
			Register(typeof(TService), implementation, reuse);
		}

		public void Register(Type service, Type implementation, Reuse reuse = Reuse.Transient) {
			if(service.IsGenericTypeDefinition) {
				genericMap[service] = implementation;
			} else {
				var constructor = ResolveConstructor(implementation);
				var parameterTypes = constructor.GetParameters().Select(p => p.ParameterType).ToList();
				switch(reuse) {
					case Reuse.Transient:
						serviceMap[service] = () => {
							var parameters = parameterTypes.Select(t => Resolve(t)).ToArray();
							return constructor.Invoke(parameters);
						};
						break;
					case Reuse.Singleton:
						object instance = null;
						serviceMap[service] = () => {
							if(instance == null) {
								var parameters = parameterTypes.Select(t => Resolve(t)).ToArray();
								instance = constructor.Invoke(parameters);
							}
							return instance;
						};
						break;
				}
			}
		}

		public void Register<TService>(TService instance) {
			Register(typeof(TService), instance);
		}

		public void Register(Type service, object instance) {
			serviceMap[service] = () => instance;
		}

		public void Register<TService>(Func<TService> factory) {
			Register(typeof(TService), factory);
		}

		public void Register(Type service, Func<object> factory) {
			serviceMap[service] = factory;
		}

		public void RegisterDecorator<TService, TDecorator>() {
			RegisterDecorator(typeof(TService), typeof(TDecorator));
		}

		public void RegisterDecorator<TService>(Type decorator) {
			RegisterDecorator(typeof(TService), decorator);
		}

		public void RegisterDecorator(Type service, Type decorator) {
			var originalFactory = serviceMap[service];
			var constructor = ResolveConstructor(decorator);
			var parameterTypes = constructor.GetParameters().Select(p => p.ParameterType).ToList();
			serviceMap[service] = () => {
				var parameters = parameterTypes.Select(t => t == service ? originalFactory() : Resolve(t)).ToArray();
				return constructor.Invoke(parameters);
			};
		}

		public TService Resolve<TService>() {
			return (TService)Resolve(typeof(TService));
		}

		public object Resolve(Type service, int depth = 0) {
			if(serviceMap.ContainsKey(service)) {
				return serviceMap[service]();
			} else if(service.IsGenericType && genericMap.ContainsKey(service.GetGenericTypeDefinition())) {
				var implementation = genericMap[service.GetGenericTypeDefinition()].MakeGenericType(service.GetGenericArguments());
				Register(service, implementation, Reuse.Transient);
				return Resolve(service);
			} else if(parent != null && depth < maxSearchDepth) {
				return parent.Resolve(service, depth + 1);
			}
			throw new CustomException("Service not found: " + service);
		}

		public void Inject(object instance) {
			var methods = instance.GetType().GetMethods().Where(m => m.GetCustomAttributes(typeof(InjectAttribute), false).Any()).ToList();
			foreach(var method in methods) {
				var parameterTypes = method.GetParameters().Select(p => p.ParameterType).ToList();
				var parameters = parameterTypes.Select(t => Resolve(t)).ToArray();
				method.Invoke(instance, parameters);
			}
		}

		private ConstructorInfo ResolveConstructor(Type type) {
			ConstructorInfo selected = null;
			foreach(var constructor in type.GetConstructors()) {
				selected = constructor;
				if(selected.GetCustomAttributes(typeof(InjectAttribute), false).Any()) {
					break;
				}
			}
			return selected;
		}

		public void Dispose() {
			parent = null;
			foreach(var child in children) {
				child.Dispose();
			}
			children.Clear();
		}
	}
}
