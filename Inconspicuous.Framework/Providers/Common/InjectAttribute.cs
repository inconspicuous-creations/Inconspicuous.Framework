using System;

namespace Inconspicuous.Framework {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class InjectAttribute : Attribute { }
}
