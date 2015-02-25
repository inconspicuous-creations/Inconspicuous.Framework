using System;

namespace Inconspicuous.Framework {
	public abstract class ApplicationManager : IApplicationManager {
		public abstract string Name { get; }

		public virtual string FullName {
			get { return Name + " - " + Version; }
		}

		public abstract bool DebugMode { get; }

		public abstract Version Version { get; }
	}
}
