namespace Inconspicuous.Framework {
	public interface IApplicationManager : INamed {
		bool DebugMode { get; }
		string Version { get; }
	}
}
