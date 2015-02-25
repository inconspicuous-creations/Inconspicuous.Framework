using System;
using System.ComponentModel.Composition;
using UniRx;

namespace Inconspicuous.Framework.Example.Test {
	public class Test {
		public string Name { get; set; }
	}

	[Export(typeof(TestViewModel))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public sealed class TestViewModel : ViewModel, IDisposable {
		private readonly Signal<Unit> testSignal;
		private readonly Signal<Unit> buttonSignal;
		private string name;

		public TestViewModel() {
			this.testSignal = new Signal<Unit>();
			this.buttonSignal = new Signal<Unit>();
			this.AsObservable()
				.Where(x => x == "Name")
				.Subscribe(x => UnityEngine.Debug.Log(x));
			testSignal
				.Select(_ => "Test").Subscribe(n => name = n);
		}

		public string Name {
			get { return name; }
			set { SetProperty(ref name, value, "Name"); }
		}

		public Signal<Unit> TestSignal {
			get { return testSignal; }
		}

		public Signal<Unit> ButtonSignal {
			get { return buttonSignal; }
		}

		public void Dispose() {
			testSignal.Dispose();
			buttonSignal.Dispose();
		}
	}
}
