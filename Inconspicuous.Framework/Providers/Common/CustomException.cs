using System;
using System.Runtime.Serialization;

namespace Inconspicuous.Framework {
	[Serializable]
	public class CustomException : Exception {
		public CustomException() : base() { }
		public CustomException(string message) : base(message) { }
		public CustomException(string format, params object[] args) : base(string.Format(format, args)) { }
		public CustomException(string message, Exception innerException) : base(message, innerException) { }
		public CustomException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }
		protected CustomException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
