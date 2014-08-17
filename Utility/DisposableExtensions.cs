using System;
using UniRx;

namespace Inconspicuous.Framework {
	public static class DisposableExtensions {
		public static T DisposeWith<T>(this T disposable, CompositeDisposable compositeDisposable) where T : IDisposable {
			compositeDisposable.Add(disposable);
			return disposable;
		}
	}
}
