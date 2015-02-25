using System;
using UnityEditor;
using UnityEngine;

namespace Inconspicuous.Framework.Editor {
	[CustomPropertyDrawer(typeof(TypeAttribute))]
	public class TypePropertyDrawer : PropertyDrawer {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			var type = property.stringValue.Length > 0 ? Type.GetType(property.stringValue) : null;
			var typeName = type != null ? type.Name : "null";
			EditorGUI.LabelField(new Rect(position.x, position.y, position.width / 2, position.height), typeName);
			if(GUI.Button(new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height), "Select Context")) {
				OpenSetTypeWindow(property);
			}
		}

		private void OpenSetTypeWindow(SerializedProperty property) {
			SelectTypeWindow.Open(((TypeAttribute)attribute).Type, (_, e) => {
				property.stringValue = e.Type.AssemblyQualifiedName;
				property.serializedObject.ApplyModifiedProperties();
			});
		}

		private object GetDefaultValue(Type t) {
			return t.IsValueType ? Activator.CreateInstance(t) : null;
		}
	}
}
