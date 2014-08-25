#pragma warning disable 0168

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Inconspicuous.Framework {
	[Export(typeof(IViewMediationBinder))]
	public class ViewMediationBinder : IViewMediationBinder {
		private readonly IContainer container;
		private IDictionary<Type, Type> mediatorTypeMap;

		public ViewMediationBinder(IContainer container) {
			this.container = container;
			mediatorTypeMap = new Dictionary<Type, Type>();
		}

		public void Mediate(IView rootView) {
			if(rootView != null) {
				var views = rootView.GameObject.transform.GetComponentsInChildren(typeof(IView), true).Cast<IView>().ToList();
				foreach(var view in views) {
					container.Inject(view);
					var type = view.GetType();
					Type mediatorType;
					if(!mediatorTypeMap.TryGetValue(type, out mediatorType)) {
						mediatorType = typeof(IMediator<>).MakeGenericType(view.GetType());
						mediatorTypeMap[type] = mediatorType;
					}
					try {
						var mediator = container.Resolve(mediatorType) as IMediator;
						if(mediator != null) {
							view.OnDispose += () => mediator.Dispose();
							mediator.Mediate(view);
						}
					} catch {
						// Do nothing.
					}
				}
			}
		}
	}
}
