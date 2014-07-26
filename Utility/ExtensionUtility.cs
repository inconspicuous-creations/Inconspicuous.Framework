using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Inconspicuous.Common {
	public static class ExtensionUtility {
		public static string CamelSpaced(this string name) {
			return Regex.Replace(name, @"([a-z])([A-Z])", @"$1 $2");
		}

		public static IList<T> Clone<T>(this IList<T> list) where T : ICloneable {
			return list.Select(i => (T)i.Clone()).ToList();
		}

		public static IDictionary<T, U> Clone<T, U>(this IDictionary<T, U> dictionary) where U : ICloneable {
			return dictionary.ToDictionary(i => i.Key, i => (U)i.Value.Clone());
		}

		public static IDictionary<T, U> InitializeForEnum<T, U>(this IDictionary<T, U> dictionary, U defaultValue) {
			dictionary = new Dictionary<T, U>();
			foreach(var key in (T[])Enum.GetValues(typeof(T))) {
				dictionary[key] = defaultValue;
			}
			return dictionary;
		}
	}
}
