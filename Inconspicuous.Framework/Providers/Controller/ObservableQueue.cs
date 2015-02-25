using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Inconspicuous.Framework {
	public sealed class ObservableQueue<T> : ISubject<T>, IDisposable {
		private readonly List<IObservable<T>> queue;
		private readonly Subject<T> subject;
		private readonly SerialDisposable disposable;
		private readonly IScheduler scheduler;

		public ObservableQueue(IScheduler scheduler) {
			this.scheduler = scheduler;
			this.queue = new List<IObservable<T>>();
			this.subject = new Subject<T>();
			this.disposable = new SerialDisposable();
		}

		public int Count {
			get { return queue.Count; }
		}

		public void Add(IObservable<T> observable) {
			queue.Add(Observable.Defer(() => observable));
		}

		public void AddRange(IEnumerable<IObservable<T>> observables) {
			queue.AddRange(observables.Select(o => Observable.Defer(() => o)));
		}

		public void Clear() {
			queue.Clear();
		}

		public void OnCompleted() {
			if(queue.Count > 0) {
				queue.RemoveAt(0);
			}
			if(queue.Count > 0) {
				disposable.Disposable = Start(queue[0]);
			} else {
				disposable.Disposable = null;
				subject.OnCompleted();
			}
		}

		public void OnError(Exception error) {
			disposable.Dispose();
			subject.OnError(error);
		}

		public void OnNext(T value) {
			subject.OnNext(value);
		}

		public IDisposable Subscribe(IObserver<T> observer) {
			if(queue.Any()) {
				disposable.Disposable = Start(queue[0]);
				return subject.Subscribe(observer);
			}
			return Disposable.Empty;
		}

		public void Dispose() {
			subject.Dispose();
			disposable.Disposable = null;
		}

		private IDisposable Start(IObservable<T> observable) {
			return observable
				.ObserveOn(scheduler)
				.Subscribe(this);
		}
	}
}
