#pragma warning disable 0168

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using MugenInjection;
using MugenInjection.Exceptions;
using Container = MugenInjection.MugenInjector;

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
					container.Inject(view);
					if(view is View && !(view is ContextView)) {
						(view as View).Initialize();
					}
					var type = view.GetType();
					Type mediatorType;
					if(!mediatorTypeMap.TryGetValue(type, out mediatorType)) {
						mediatorType = typeof(IMediator<>).MakeGenericType(view.GetType());
						mediatorTypeMap[type] = mediatorType;
					}
					try {
						var mediator = container.Get(mediatorType) as IMediator;
						if(mediator != null) {
							view.OnDispose += () => mediator.Dispose();
							mediator.Mediate(view);
						}
					} catch(BindingNotFoundException _) {
						// Do nothing.
					}
				}
			}
		}
	}
}
