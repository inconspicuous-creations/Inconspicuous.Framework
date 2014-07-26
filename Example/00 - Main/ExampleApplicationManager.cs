namespace Inconspicuous.Framework.Example.Common {
	//[Export(typeof(IApplicationManager))]
	public class ExampleApplicationManager : ApplicationManager {
		public override string Name {
			get { return "Example Application"; }
		}

		public override bool DebugMode {
			get { return true; }
		}

		public override string Version {
			get { return "1.0.0"; }
		}
	}
}
