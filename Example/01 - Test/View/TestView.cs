using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework.Example.Test {
	public class TestView : View {
		public Signal<Unit> ButtonSignal { get; private set; }

		[Inject]
		public void Construct(Signal<Unit> buttonSignal) {
			ButtonSignal = buttonSignal;
			UpdateAsObservable()
				.Subscribe(_ => {
					if(UnityEngine.Input.GetKeyDown(KeyCode.B)) {
						ButtonSignal.Dispatch();
					}
				}).DisposeWith(this);
		}
	}
}
