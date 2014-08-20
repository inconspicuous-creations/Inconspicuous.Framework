namespace System {
	public sealed class Lazy<T> where T : class {
		public Lazy(Func<object> valueFactory) {
			if(valueFactory == null) throw new ArgumentNullException("valueFactory");
			_valueFactory = valueFactory;
		}

		public bool IsValueCreated { get; private set; }

		public T Value {
			get { return IsValueCreated ? _value as T : Create(); }
		}

		#region Implementation

		private Func<object> _valueFactory;
		private object _value;
		private readonly object _valueCreationLock = new object();

		private T Create() {
			lock(_valueCreationLock) {
				if(!IsValueCreated) {
					if(_valueFactory == null) throw new InvalidOperationException("The initialization function tries to access Value on this instance.");
					var factory = _valueFactory;
					_valueFactory = null;
					_value = factory();
					IsValueCreated = true;
				}
			}

			return _value as T;
		}

		#endregion
	}
}
