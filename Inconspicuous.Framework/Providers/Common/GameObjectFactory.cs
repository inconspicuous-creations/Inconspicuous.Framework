using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using UnityEngine;

namespace Inconspicuous.Framework {
	public class PrefabGameObjectData {
		public GameObject Prefab { get; set; }
		public string Name { get; set; }
		public Transform Parent { get; set; }
		public bool Pooled { get; set; }
	}

	[Export(typeof(IFactory<Func<PrefabGameObjectData, GameObject>>))]
	public sealed class PrefabGameObjectFactory : IFactory<Func<PrefabGameObjectData, GameObject>> {
		private readonly IViewMediationBinder viewMediationBinder;
		private readonly Dictionary<GameObject, List<GameObject>> pools;

		public PrefabGameObjectFactory(IViewMediationBinder viewMediationBinder) {
			this.viewMediationBinder = viewMediationBinder;
			this.pools = new Dictionary<GameObject, List<GameObject>>();
		}

		public Func<PrefabGameObjectData, GameObject> Create() {
			return prefabGameObjectData => {
				GameObject gameObject = null;
				if(prefabGameObjectData.Pooled) {
					gameObject = Retrieve(prefabGameObjectData.Prefab);
				}
				if(gameObject == null) {
					gameObject = UnityEngine.Object.Instantiate(prefabGameObjectData.Prefab) as GameObject;
					gameObject.name = string.IsNullOrEmpty(prefabGameObjectData.Name) ? prefabGameObjectData.Prefab.name : prefabGameObjectData.Name;
					SetParent(gameObject, prefabGameObjectData.Parent);
					viewMediationBinder.Mediate(gameObject.GetComponent(typeof(IView)) as IView);
					if(prefabGameObjectData.Pooled) {
						pools[prefabGameObjectData.Prefab].Add(gameObject);
					}
				}
				return gameObject;
			};
		}

		private void SetParent(GameObject obj, Transform parent) {
			if(parent != null) {
				var rectTransform = obj.GetComponent<RectTransform>();
				if(rectTransform != null) {
					rectTransform.SetParent(parent, false);
				} else {
					obj.transform.parent = parent;
				}
			}
		}

		private GameObject Retrieve(GameObject prefab) {
			List<GameObject> pool;
			if(pools.TryGetValue(prefab, out pool)) {
				foreach(var obj in pool) {
					if(!obj.activeSelf) {
						return obj;
					}
				}
			} else {
				pools[prefab] = new List<GameObject>();
			}
			return null;
		}
	}

	public class GameObjectData {
		public string PrefabName { get; set; }
		public string Name { get; set; }
		public Transform Parent { get; set; }
		public bool Pooled { get; set; }
	}

	[Export(typeof(IFactory<Func<GameObjectData, GameObject>>))]
	public sealed class GameObjectFactory : IFactory<Func<GameObjectData, GameObject>> {
		private readonly IViewMediationBinder viewMediationBinder;
		private readonly Dictionary<string, List<GameObject>> pools;

		public GameObjectFactory(IViewMediationBinder viewMediationBinder) {
			this.viewMediationBinder = viewMediationBinder;
			this.pools = new Dictionary<string, List<GameObject>>();
		}

		public Func<GameObjectData, GameObject> Create() {
			return gameObjectData => {
				GameObject gameObject = null;
				if(gameObjectData.Pooled) {
					gameObject = Retrieve(gameObjectData.PrefabName);
				}
				if(gameObject == null) {
					var prefab = Resources.Load("Prefabs/" + gameObjectData.PrefabName);
					gameObject = UnityEngine.Object.Instantiate(prefab) as GameObject;
					gameObject.name = string.IsNullOrEmpty(gameObjectData.Name) ? prefab.name : gameObjectData.Name;
					SetParent(gameObject, gameObjectData.Parent);
					viewMediationBinder.Mediate(gameObject.GetComponent(typeof(IView)) as IView);
					if(gameObjectData.Pooled) {
						pools[gameObjectData.PrefabName].Add(gameObject);
					}
				}
				return gameObject;
			};
		}

		private void SetParent(GameObject obj, Transform parent) {
			if(parent != null) {
				var rectTransform = obj.GetComponent<RectTransform>();
				if(rectTransform != null) {
					rectTransform.SetParent(parent, false);
				} else {
					obj.transform.parent = parent;
				}
			}
		}

		private GameObject Retrieve(string name) {
			List<GameObject> pool;
			if(pools.TryGetValue(name, out pool)) {
				foreach(var obj in pool) {
					if(!obj.activeSelf) {
						return obj;
					}
				}
			} else {
				pools[name] = new List<GameObject>();
			}
			return null;
		}
	}
}
