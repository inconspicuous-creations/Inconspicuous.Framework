using System;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public abstract class View : FastObservableMonoBehaviour, IView {
		public event Action OnDispose = delegate { };

		protected IDisposable onDestroySubscription;
		private CompositeDisposable disposable = new CompositeDisposable();

		public GameObject GameObject {
			get { return gameObject; }
		}

		public override void Awake() {
			base.Awake();
			onDestroySubscription = OnDestroyAsObservable()
				.Subscribe(_ => Dispose()).DisposeWith(this);
		}

		public void Dispose() {
			OnDispose();
			disposable.Dispose();
		}

		public static implicit operator CompositeDisposable(View view) {
			return view.disposable;
		}
	}
}
