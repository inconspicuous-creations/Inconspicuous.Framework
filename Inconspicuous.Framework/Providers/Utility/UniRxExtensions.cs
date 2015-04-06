using System;
using System.Collections.Generic;
using System.ComponentModel;
using UniRx;

namespace Inconspicuous.Framework {
	public static class UniRxExtensions {
		public static IObservable<T> Dump<T>(this IObservable<T> observable) {
			return observable
				.Do(x => UnityEngine.Debug.Log(x.ToString()))
				.Finally(() => UnityEngine.Debug.Log("Completed"));
		}

		public static IObservable<string> AsObservable(this INotifyPropertyChanged notifyPropertyChanged) {
			return Observable.FromEvent<PropertyChangedEventHandler, string>(
				x => new PropertyChangedEventHandler((_, args) => x(args.PropertyName)),
				x => notifyPropertyChanged.PropertyChanged += x,
				x => notifyPropertyChanged.PropertyChanged -= x);
		}

		public static IObservable<Unit> AsObservable(this INotifyPropertyChanged notifyPropertyChanged, string name) {
			return Observable.FromEvent(
				x => new PropertyChangedEventHandler((_, args) => { if(name == args.PropertyName) { x(); } }),
				x => notifyPropertyChanged.PropertyChanged += x,
				x => notifyPropertyChanged.PropertyChanged -= x);
		}

		public static IObservable<T> AsObservable<T>(this INotifyPropertyChanged notifyPropertyChanged, string name, Func<T> selector) {
			return Observable.FromEvent(
				x => new PropertyChangedEventHandler((_, args) => { if(name == args.PropertyName) { x(); } }),
				x => notifyPropertyChanged.PropertyChanged += x,
				x => notifyPropertyChanged.PropertyChanged -= x)
				.Select(_ => selector());
		}

		public static T AddTo<T>(this T disposable, CompositeDisposable compositeDisposable) where T : IDisposable {
			compositeDisposable.Add(disposable);
			return disposable;
		}

		public static void OnNext(this Subject<Unit> subject) {
			subject.OnNext(Unit.Default);
		}

		public static CompositeDisposable TryGetOrClearFromMap<T>(this Dictionary<T, CompositeDisposable> disposables, T key) {
			var disposable = default(CompositeDisposable);
			disposables.TryGetValue(key, out disposable);
			if(disposable != null) {
				disposable.Clear();
			} else {
				disposable = new CompositeDisposable();
				disposables[key] = disposable;
			}
			return disposable;
		}
	}
}
