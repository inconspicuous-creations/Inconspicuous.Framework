using System.ComponentModel.Composition;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework.Example.Test {
	public class TestView : View {
		[Import]
		public Signal<Unit> ButtonSignal { get; set; }

		public override void Initialize() {
			UpdateAsObservable()
				.Subscribe(_ => {
					if(Input.GetKeyDown(KeyCode.B)) {
						ButtonSignal.Dispatch();
					}
				}).DisposeWith(this);
		}
	}
}
