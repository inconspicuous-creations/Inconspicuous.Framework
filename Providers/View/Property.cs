using System;
using System.ComponentModel.Composition;
using UniRx;

namespace Inconspicuous.Framework {
	[Export(typeof(Property<>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class Property<T> : IObservable<T> where T : IEquatable<T> {
		private T value;
		private ISubject<T> subject;
		private bool alwaysNotify;

		public Property() {
			subject = new SuperFastSubject<T>();
		}

		public Property(T value)
			: this() {
			this.value = value;
		}

		public T Value {
			get { return value; }
			set {
				if(alwaysNotify || this.value == null || !this.value.Equals(value)) {
					this.value = value;
					subject.OnNext(value);
				}
			}
		}

		public IDisposable Subscribe(IObserver<T> observer) {
			var disposable = subject.Subscribe(observer);
			if(value != null) {
				subject.OnNext(value);
			}
			return disposable;
		}

		public Property<T> AlwaysNotify() {
			alwaysNotify = true;
			return this;
		}

		public override string ToString() {
			return value.ToString();
		}

		//public static implicit operator T(Property<T> property) {
		//	return property.Value;
		//}
	}
}
