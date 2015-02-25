using System;

namespace Inconspicuous.Framework.Example.Common {
	//[Export(typeof(IApplicationManager))]
	public class ExampleApplicationManager : ApplicationManager {
		public override string Name {
			get { return "Example Application"; }
		}

		public override bool DebugMode {
			get { return true; }
		}

		public override Version Version {
			get { return new Version(1, 0, 0); }
		}
	}
}
