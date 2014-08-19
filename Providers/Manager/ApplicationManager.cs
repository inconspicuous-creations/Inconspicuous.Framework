namespace Inconspicuous.Framework {
	public abstract class ApplicationManager : IApplicationManager {
		public virtual string Name {
			get { return "Unnamed Project"; }
		}

		public virtual string FullName {
			get { return Name + " - " + Version; }
		}

		public virtual bool DebugMode {
			get { return true; }
		}

		public virtual string Version {
			get { return "1.0.0"; }
		}
	}
}
