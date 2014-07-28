using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Inconspicuous.Framework {
	public class ObservableQueue<T> : ISubject<T> {
		private List<IObservable<T>> queue;
		private ISubject<T> subject;
		private IDisposable subscription;
		private IScheduler scheduler;

		public ObservableQueue(IScheduler scheduler) {
			this.scheduler = scheduler;
			queue = new List<IObservable<T>>();
			subject = new FastSubject<T>();
			subscription = new SerialDisposable();
		}

		public void Add(IObservable<T> observable) {
			queue.Add(Observable.Defer(() => observable));
		}

		public void AddRange(IEnumerable<IObservable<T>> observables) {
			queue.AddRange(observables.Select(o => Observable.Defer(() => o)));
		}

		protected IDisposable Start(IObservable<T> observable) {
			return observable
				.ObserveOn(scheduler)
				.Subscribe(this);
		}

		public void OnCompleted() {
			queue.RemoveAt(0);
			if(queue.Count > 0) {
				subscription = Start(queue[0]);
			} else {
				subscription.Dispose();
				subject.OnCompleted();
			}
		}

		public void OnError(Exception error) {
			subscription.Dispose();
			subject.OnError(error);
		}

		public void OnNext(T value) {
			subject.OnNext(value);
		}

		public IDisposable Subscribe(IObserver<T> observer) {
			subscription = Start(queue[0]);
			return subject.Subscribe(observer);
		}
	}
}
