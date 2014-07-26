using System;
using UniRx;

namespace Inconspicuous.Framework {
	public class SuperFastSubject<T> : ISubject<T> {
		//private event Action onCompleted;
		//private event Action<Exception> onError;
		private event Action<T> onNext;

		public SuperFastSubject() {
			//onCompleted = delegate { };
			//onError = delegate { };
			onNext = delegate { };
		}

		public void OnCompleted() {
			//onCompleted();
		}

		public void OnError(Exception error) {
			//onError(error);
		}

		public void OnNext(T value) {
			onNext(value);
		}

		public IDisposable Subscribe(IObserver<T> observer) {
			//onCompleted += observer.OnCompleted;
			//onError += observer.OnError;
			onNext += observer.OnNext;
			return Disposable.Create(() => {
				//onCompleted -= observer.OnCompleted;
				//onError -= observer.OnError;
				onNext -= observer.OnNext;
			});
		}
	}
}
