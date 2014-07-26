using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework.Example.Test {
	[DataContract]
	public class Test {
		[DataMember]
		public string Name { get; set; }
	}

	[Export(typeof(TestViewModel))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class TestViewModel : ViewModel {
		private Property<string> name = new Property<string>();
		private Signal<Unit> testSignal = new Signal<Unit>();
		private Signal<Unit> buttonSignal = new Signal<Unit>();

		public TestViewModel() {
			testSignal.Select(_ => "Test").Subscribe(n => name.Value = n).DisposeWith(this);
			name.Subscribe(x => Debug.Log(x)).DisposeWith(this);
		}

		public string Name {
			get { return name.Value; }
			set { name.Value = value; }
		}

		public Signal<Unit> TestSignal {
			get { return testSignal; }
		}

		public Signal<Unit> ButtonSignal {
			get { return buttonSignal; }
		}
	}
}
