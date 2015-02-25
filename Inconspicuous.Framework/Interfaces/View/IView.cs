using System;
using UnityEngine;

namespace Inconspicuous.Framework {
	public interface IView : IDisposable {
		GameObject GameObject { get; }
	}
}
