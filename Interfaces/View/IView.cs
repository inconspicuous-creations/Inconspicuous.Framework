using System;
using UnityEngine;

namespace Inconspicuous.Framework {
	public interface IView : IDisposable {
		event Action OnDispose;
		GameObject GameObject { get; }
	}
}
