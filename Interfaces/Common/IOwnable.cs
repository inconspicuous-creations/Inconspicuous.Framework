using System;

namespace Inconspicuous.Framework {
	public interface IOwnable<T> : ICloneable {
		T Owner { get; set; }
	}
}
