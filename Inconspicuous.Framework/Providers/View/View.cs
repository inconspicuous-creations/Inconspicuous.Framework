using System;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public abstract class View : MonoBehaviour, IView {
		// private readonly CompositeDisposable disposable = new CompositeDisposable();

		public GameObject GameObject {
			get { return gameObject; }
		}

		// public virtual void OnDestroy() {
		// 	Dispose();
		// }

		// public void Dispose() {
		// 	Dispose(true);
		// 	GC.SuppressFinalize(this);
		// }

		// protected virtual void Dispose(bool disposing) {
		// 	if(!disposable.IsDisposed) {
		// 		if(disposing) {
		// 			// Do nothing;
		// 		}
		// 		disposable.Dispose();
		// 	}
		// }

		// public static implicit operator CompositeDisposable(View view) {
		// 	return view.disposable;
		// }
	}
}
