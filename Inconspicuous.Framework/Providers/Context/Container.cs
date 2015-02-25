using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inconspicuous.Framework {
	public sealed class Container : IContainer {
		private readonly Dictionary<object, Type> genericMap;
		private readonly Dictionary<object, Func<object>> serviceMap;
		private readonly List<IContainer> children;
		private IContainer parent;

		public Container() {
			this.genericMap = new Dictionary<object, Type>();
			this.serviceMap = new Dictionary<object, Func<object>>();
			this.children = new List<IContainer>();
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
				if(service.IsGenericType && service.GetGenericTypeDefinition() == typeof(IFactory<>)) {
					Register(service.GetGenericArguments().First(), () => {
						var factory = Resolve(service);
						return factory.GetType().GetMethod("Create").Invoke(factory, null);
					});
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

		public void RegisterDecorator<TService, TDecorator>(Reuse reuse = Reuse.Transient) {
			RegisterDecorator(typeof(TService), typeof(TDecorator), reuse);
		}

		public void RegisterDecorator<TService>(Type decorator, Reuse reuse = Reuse.Transient) {
			RegisterDecorator(typeof(TService), decorator, reuse);
		}

		public void RegisterDecorator(Type service, Type decorator, Reuse reuse = Reuse.Transient) {
			var originalFactory = serviceMap[service];
			var constructor = ResolveConstructor(decorator);
			var parameterTypes = constructor.GetParameters().Select(p => p.ParameterType).ToList();
			switch(reuse) {
				case Reuse.Transient:
					serviceMap[service] = () => {
						var parameters = parameterTypes.Select(t => t == service ? originalFactory() : Resolve(t)).ToArray();
						return constructor.Invoke(parameters);
					};
					break;
				case Reuse.Singleton:
					object instance = null;
					serviceMap[service] = () => {
						if(instance == null) {
							var parameters = parameterTypes.Select(t => t == service ? originalFactory() : Resolve(t)).ToArray();
							instance = constructor.Invoke(parameters);
						}
						return instance;
					};
					break;
			}
		}

		public TService Resolve<TService>(bool canReturnNull = false) where TService : class {
			return Resolve(typeof(TService), canReturnNull) as TService;
		}

		public object Resolve(Type service, bool canReturnNull = false, ICollection<IContainer> searched = null) {
			if(serviceMap.ContainsKey(service)) {
				return serviceMap[service]();
			} else if(service.IsGenericType && genericMap.ContainsKey(service.GetGenericTypeDefinition())) {
				var implementation = genericMap[service.GetGenericTypeDefinition()].MakeGenericType(service.GetGenericArguments());
				Register(service, implementation, Reuse.Transient);
				return Resolve(service, canReturnNull);
			} else if(service.IsGenericType && service.GetGenericTypeDefinition() == typeof(Lazy<>)) {
				Func<object> factory = () => Resolve(service.GetGenericArguments().First());
				Register(service, Activator.CreateInstance(service, new[] { factory }));
				return Resolve(service, canReturnNull);
			} else if(parent != null && (searched == null || !searched.Contains(this))) {
				searched = searched ?? new List<IContainer>();
				searched.Add(this);
				return parent.Resolve(service, canReturnNull, searched);
			}
			if(!canReturnNull) {
				throw new CustomException("Service not found: " + service);
			}
			return null;
		}

		public void Inject(object instance) {
			var methods = instance.GetType().GetMethods().Where(m => m.GetCustomAttributes(typeof(InjectAttribute), false).Any()).ToList();
			foreach(var method in methods) {
				var parameterTypes = method.GetParameters().Select(p => p.ParameterType).ToList();
				var parameters = parameterTypes.Select(t => Resolve(t, true)).ToArray();
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
			genericMap.Clear();
			serviceMap.Clear();
			parent = null;
			children.Clear();
		}
	}
}
