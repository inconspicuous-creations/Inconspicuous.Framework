using System;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using UnityEngine;

namespace Inconspicuous.Framework {
	[DataContract]
	public class GameObjectData {
		[DataMember]
		public string PrefabName { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public Transform Parent { get; set; }
	}

	[Export(typeof(IFactory<Func<GameObjectData, GameObject>>))]
	public class GameObjectFactory : IFactory<Func<GameObjectData, GameObject>> {
		private readonly IViewMediationBinder viewMediationBinder;

		public GameObjectFactory(IViewMediationBinder viewMediationBinder) {
			this.viewMediationBinder = viewMediationBinder;
		}

		public Func<GameObjectData, GameObject> Create() {
			return gameObjectData => {
				var prefab = Resources.Load("Prefabs/" + gameObjectData.PrefabName);
				var gameObject = UnityEngine.Object.Instantiate(prefab) as GameObject;
				gameObject.name = string.IsNullOrEmpty(gameObjectData.Name) ? prefab.name : gameObjectData.Name;
				if(gameObjectData.Parent != null) {
					gameObject.transform.parent = gameObjectData.Parent;
				}
				viewMediationBinder.Mediate(gameObject.GetComponent(typeof(IView)) as IView);
				return gameObject;
			};
		}
	}
}
