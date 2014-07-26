using Holoville.HOTween;
using UnityEngine;

namespace Inconspicuous.Framework {
	public class ApplicationManager : IApplicationManager {
		private bool initialized;

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

		public virtual void Initialize() {
			if(!initialized) {
				initialized = true;
				Application.targetFrameRate = 60;
				HOTween.Init(false, false, false);
			}
		}
	}
}
