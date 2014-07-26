using System;

namespace Inconspicuous.Framework {
	public interface IContext : IDisposable {
		void Start();
	}
}
