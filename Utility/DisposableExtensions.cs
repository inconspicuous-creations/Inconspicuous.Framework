using System;
using UniRx;

namespace Inconspicuous.Framework {
	public static class DisposableExtensions {
		public static T DisposeWith<T>(this T disposable, CompositeDisposable compositeDisposable) where T : IDisposable {
			compositeDisposable.Add(disposable);
			return disposable;
		}
	}

	public static class ViewModelExtensions {
		//public static Property<T> ToProperty<T>(this IObservable<T> observable, ViewModel viewModel) where T : IEquatable<T> {
		//	var property = new Property<T>();
		//	observable.Subscribe(t => property.Value = t).DisposeWith(viewModel);
		//	return property;
		//}

		//	private static Dictionary<MemberExpression, Action<object>> assignCache;

		//	public static IDisposable Subscribe<T, TViewModel>(this IObservable<T> observable, TViewModel model, Expression<Func<TViewModel, T>> e) {
		//		assignCache = assignCache ?? new Dictionary<MemberExpression, Action<object>>();
		//		var me = e.Body as MemberExpression;
		//		Action<object> assign = null;
		//		if(assignCache.TryGetValue(me, out assign)) {
		//			var setter = model.GetType().GetProperty(me.Member.Name).GetSetMethod();
		//			var m = Expression.Constant(model, model.GetType());
		//			var v = Expression.Parameter(typeof(object), "v");
		//			var c = Expression.Convert(v, typeof(T));
		//			assign = (Action<object>)LambdaExpression.Lambda(Expression.Call(m, setter), v).Compile();
		//			assignCache.Add(me, assign);
		//		}
		//		return observable.Subscribe(x => assign(x));
		//	}
	}
}
