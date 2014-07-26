using UnityEngine;

namespace Inconspicuous.Framework {
	public interface IInputManager {
		void SetBlock(bool block, LayerMask layerMask = default(LayerMask));
	}
}
