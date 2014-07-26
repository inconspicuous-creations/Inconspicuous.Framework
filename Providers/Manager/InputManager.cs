using System.ComponentModel.Composition;
using UnityEngine;

namespace Inconspicuous.Framework {
	[Export(typeof(IInputManager))]
	public class InputManager : IInputManager {
		private GameObject blocker;

		public InputManager(IContextView contextView) {
			blocker = new GameObject("Blocker");
			var box = blocker.AddComponent<BoxCollider>();
			box.size = new Vector3(4f, 3f, 0f);
			blocker.transform.parent = contextView.GameObject.transform;
			blocker.transform.position = new Vector3(0f, 0f, -25f);
			blocker.SetActive(false);
		}

		public void SetBlock(bool block, LayerMask layerMask) {
			blocker.layer = layerMask;
			blocker.SetActive(block);
		}
	}
}
