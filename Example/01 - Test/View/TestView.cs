using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework.Example.Test {
	public class TestView : View {
		public Signal<Unit> ButtonSignal { get; private set; }

		[MugenInjection.Attributes.Inject]
		public void Construct(Signal<Unit> buttonSignal) {
			ButtonSignal = buttonSignal;
		}

		public override void Initialize() {
			UpdateAsObservable()
				.Subscribe(_ => {
					if(UnityEngine.Input.GetKeyDown(KeyCode.B)) {
						ButtonSignal.Dispatch();
					}
				}).DisposeWith(this);
		}
	}
}
