using System;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using DryIoc.MefAttributedModel;
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

	[ExportAll]
	public class GameObjectFactory : IFactory<Func<GameObjectData, GameObject>> {
		private readonly IViewMediationBinder viewMediationBinder;

		public GameObjectFactory(IViewMediationBinder viewMediationBinder) {
			this.viewMediationBinder = viewMediationBinder;
		}

		[Export]
		public Func<GameObjectData, GameObject> Create() {
			return gameObjectInfo => {
				var prefab = Resources.Load("Prefabs/" + gameObjectInfo.PrefabName);
				var gameObject = UnityEngine.Object.Instantiate(prefab) as GameObject;
				gameObject.name = string.IsNullOrEmpty(gameObjectInfo.Name) ? prefab.name : gameObjectInfo.Name;
				gameObject.transform.parent = gameObjectInfo.Parent;
				viewMediationBinder.Mediate(gameObject.GetComponent(typeof(IView)) as IView);
				return gameObject;
			};
		}
	}
}
