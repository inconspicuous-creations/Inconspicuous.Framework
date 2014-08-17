using UniRx;

namespace Inconspicuous.Framework {
	public interface ILevelManager {
		IObservable<IContextView> Load(string name);
	}
}
