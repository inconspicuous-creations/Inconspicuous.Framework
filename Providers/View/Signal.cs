using System;
using System.ComponentModel.Composition;
using UniRx;

namespace Inconspicuous.Framework {
	[Export(typeof(Signal<>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class Signal<T> : IObservable<T> {
		private ISubject<T> subject;

		public Signal() {
			subject = new SuperFastSubject<T>();
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
	}
}
