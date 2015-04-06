using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Inconspicuous.Framework {
	[Export(typeof(IViewMediationBinder))]
	public class ViewMediationBinder : IViewMediationBinder {
		private readonly IContainer container;
		private readonly IDictionary<Type, Type> mediatorTypeMap;

		public ViewMediationBinder(IContainer container) {
			this.container = container;
			this.mediatorTypeMap = new Dictionary<Type, Type>();
		}

		public void Mediate(IView rootView) {
			if(rootView != null) {
				foreach(var view in rootView.GameObject.transform.GetComponentsInChildren(typeof(IView), true).Cast<IView>().ToArray()) {
					container.Inject(view);
					var type = view.GetType();
					var mediatorType = default(Type);
					if(!mediatorTypeMap.TryGetValue(type, out mediatorType)) {
						mediatorType = typeof(IMediator<>).MakeGenericType(view.GetType());
						mediatorTypeMap[type] = mediatorType;
					}
					var mediator = container.Resolve(mediatorType, true) as IMediator;
					if(mediator != null) {
						mediator.Mediate(view);
					}
				}
			}
		}
	}
}
