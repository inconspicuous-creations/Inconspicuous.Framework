using System;

namespace Inconspicuous.Framework {
	public interface IApplicationManager : INamed {
		bool DebugMode { get; }
		Version Version { get; }
	}
}
