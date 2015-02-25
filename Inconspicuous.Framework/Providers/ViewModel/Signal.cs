using System;
using System.ComponentModel.Composition;
using UniRx;

namespace Inconspicuous.Framework {
	[Export(typeof(Signal<>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public sealed class Signal<T> : IObservable<T>, IDisposable {
		private readonly Subject<T> subject;

		public Signal() {
			this.subject = new Subject<T>();
		}

		public void Dispatch() {
			subject.OnNext(default(T));
		}

		public void Dispatch(T value) {
			subject.OnNext(value);
		}

		public IDisposable Subscribe(IObserver<T> observer) {
			return subject.Subscribe(observer);
		}

		public void Dispose() {
			subject.Dispose();
		}
	}
}
