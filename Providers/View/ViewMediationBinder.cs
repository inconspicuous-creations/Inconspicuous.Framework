using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using DryIoc;

namespace Inconspicuous.Framework {
	[Export(typeof(IViewMediationBinder))]
	public class ViewMediationBinder : IViewMediationBinder {
		private readonly Container container;
		private IDictionary<Type, Type> mediatorTypeMap;

		public ViewMediationBinder(Container container) {
			this.container = container;
			mediatorTypeMap = new Dictionary<Type, Type>();
		}

		public void Mediate(IView rootView) {
			if(rootView != null) {
				var views = rootView.GameObject.transform.GetComponentsInChildren(typeof(IView), true).Cast<IView>().ToList();
				foreach(var view in views) {
					container.ResolvePropertiesAndFields(view);
					if(view is View && !(view is ContextView)) {
						(view as View).Initialize();
					}
					var type = view.GetType();
					Type mediatorType;
					if(!mediatorTypeMap.TryGetValue(type, out mediatorType)) {
						mediatorType = typeof(IMediator<>).MakeGenericType(view.GetType());
						//var funcType = typeof(Func<,>).MakeGenericType(view.GetType(), type);
						mediatorTypeMap[type] = mediatorType;
					}
					var mediator = container.Resolve(mediatorType, IfUnresolved.ReturnNull) as IMediator;
					if(mediator != null) {
						view.OnDispose += () => mediator.Dispose();
						mediator.Mediate(view);
					}
				}
			}
		}
	}
}
