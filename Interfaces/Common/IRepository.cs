using System.Collections.ObjectModel;

namespace Inconspicuous.Framework {
	public interface IRepository<T> where T : class {
		ReadOnlyCollection<T> Items { get; }
		T Retrieve(string name);
		U Retrieve<U>(string name) where U : class, T;
		U Register<U>(U item) where U : class, T;
	}
}
