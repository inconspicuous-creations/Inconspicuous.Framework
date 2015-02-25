using System;
using System.Text;
using UnityEngine;

namespace Inconspicuous.Framework.Editor {
	public static class EditorUtility {
		public static Type GetType(string typeName) {
			var type = Type.GetType(typeName);
			if(type != null) {
				return type;
			}
			foreach(var a in AppDomain.CurrentDomain.GetAssemblies()) {
				type = a.GetType(typeName);
				if(type != null) {
					return type;
				}
			}
			return null;
		}

		public static string GetTitleName(string name) {
			var s = name.Substring(0, 1).ToUpper() + name.Substring(1);
			var builder = new StringBuilder();
			for(int i = 0; i < s.Length; i++) {
				builder.Append(i > 0 && Char.IsUpper(s[i]) ? " " : "").Append(s[i]);
			}
			return builder.ToString();
		}

		public static Rect CreateDefaultOffset(ref Rect rect) {
			var offset = new RectOffset(0, 0, -2, -2);
			return rect = offset.Add(rect);
		}
	}
}
