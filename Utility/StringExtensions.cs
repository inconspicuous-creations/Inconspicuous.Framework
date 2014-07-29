using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Inconspicuous.Common {
	public static class ExtensionUtility {
		public static string CamelSpaced(this string name) {
			return Regex.Replace(name, @"([a-z])([A-Z])", @"$1 $2");
		}
	}
}
