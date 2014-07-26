using System;
using UnityEngine;

namespace Inconspicuous.Framework {
	[AttributeUsage(AttributeTargets.Field)]
	public class TypeAttribute : PropertyAttribute {
		private readonly Type type;

		public TypeAttribute(Type type) {
			this.type = type;
		}

		public Type Type {
			get { return type; }
		}
	}
}
