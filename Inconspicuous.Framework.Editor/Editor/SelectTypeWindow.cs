#pragma warning disable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Inconspicuous.Framework.Editor {
	public class SelectTypeWindow : EditorWindow {
		public static void Open(EventHandler<OnSelectEventArgs> callback) {
			SelectTypeWindow.searchType = null;
			var window = EditorWindow.GetWindow<SelectTypeWindow>();
			onSelected = null;
			onSelected += callback;
			window.Show();
		}

		public static void Open(Type searchType, EventHandler<OnSelectEventArgs> callback) {
			SelectTypeWindow.searchType = searchType;
			var window = EditorWindow.GetWindow<SelectTypeWindow>();
			onSelected = null;
			onSelected += callback;
			window.Show();
		}

		public void Show() {
			title = "Select Type";
		}

		public static event EventHandler<OnSelectEventArgs> onSelected;
		public static Type searchType;

		private List<string> cachedTypes;
		private string searchTerms = "";
		private Vector2 scrollPosition;
		private int selected;

		public void OnGUI() {
			searchTerms = EditorGUILayout.TextField(searchTerms);
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			var results = FindTypes(searchTerms);
			//scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition, viewRect);
			//selected = GUI.SelectionGrid(viewRect, selected, results, 1);
			selected = GUILayout.SelectionGrid(selected, results, 1, "OL Elem");
			switch(Event.current.type) {
				case EventType.used:
					if(Event.current.clickCount == 2) {
						onSelected(this, new OnSelectEventArgs(EditorUtility.GetType(results[selected])));
						onSelected = null;
						Close();
					}
					break;
			}
			GUILayout.EndScrollView();
		}

		private List<string> GetBuiltinTypes() {
			/*
				Integer	Integer property.
				Boolean	Boolean property.
				Float	Float property.
				String	String property.
				Color	Color property.
				ObjectReference	Reference to another object.
				LayerMask	 LayerMask property.
				Enum	Enumeration property.
				Vector2	2D vector property.
				Vector3	3D vector property.
				Rect	Rectangle property.
				ArraySize	Array size property.
				Character	Character property.
				AnimationCurve	AnimationCurve property.
				Bounds	Bounds property.
				Gradient	Gradient property.
			*/
			return new List<string>(new string[] {
				"System.String",
				"System.Int32",
				"System.Boolean",
				"System.Single",
				//"UnityEngine.Color",
				"UnityEngine.Vector2",
				"UnityEngine.Vector3"
			});
		}

		private List<string> GetAllTypes() {
			var objectTypes = AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => {
					try {
						a.GetTypes();
					} catch {
						return false;
					}
					return true;

				})
				.SelectMany(a => a.GetTypes())
				//.Where(t => !string.IsNullOrEmpty(t.Namespace) && t.Namespace.Contains("UnityEngine"))
				.Where(t => searchType != null || typeof(UnityEngine.Object).IsAssignableFrom(t))
				.Where(t => searchType == null || !t.IsAbstract)
				.Where(t => searchType == null || !t.IsInterface)
				.Where(t => searchType == null || searchType.IsAssignableFrom(t))
				.Select(t => t.FullName)
				.ToList();
			return searchType == null ? GetBuiltinTypes().Concat(objectTypes).ToList() : objectTypes;
		}

		private string[] FindTypes(string searchTerms) {
			if(cachedTypes == null) {
				cachedTypes = GetAllTypes();
			}
			var list = cachedTypes;
			return list
				.Where(s => !string.IsNullOrEmpty(s) && s.ToUpper().Contains(searchTerms.ToUpper()))
				.ToArray();
		}

		public class OnSelectEventArgs : EventArgs {
			private readonly Type type;

			public OnSelectEventArgs(Type type) {
				this.type = type;
			}

			public Type Type {
				get { return type; }
			}
		}
	}
}
