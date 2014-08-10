using System;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public abstract class View : FastObservableMonoBehaviour, IView {
		public event Action OnDispose = delegate { };

		private CompositeDisposable disposable = new CompositeDisposable();

		public GameObject GameObject {
			get { return gameObject; }
		}

		public sealed override void Awake() {
			base.Awake();
			OnDestroyAsObservable()
				.Subscribe(_ => Dispose()).DisposeWith(this);
		}

		public void Dispose() {
			OnDispose();
			disposable.Dispose();
		}

		public abstract void Initialize();

		public static implicit operator CompositeDisposable(View view) {
			return view.disposable;
		}
	}
}
