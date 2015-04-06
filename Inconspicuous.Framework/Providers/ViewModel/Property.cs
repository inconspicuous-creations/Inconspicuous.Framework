using System;
using System.ComponentModel.Composition;
using UniRx;

namespace Inconspicuous.Framework {
	[Obsolete("Use the .NET idiomatic ViewModel pattern by inheriting or containing ViewModel instead.")]
	[Export(typeof(Property<>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public sealed class Property<T> : IObservable<T>, IDisposable {
		private readonly Subject<T> subject;
		private readonly bool alwaysNotify;
		private T value;

		public Property(bool alwaysNotify = false) {
			this.subject = new Subject<T>();
			this.alwaysNotify = alwaysNotify;
		}

		public Property(T value, bool alwaysNotify = false)
			: this(alwaysNotify) {
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

		public void Notify() {
			subject.OnNext(value);
		}

		public override string ToString() {
			return value.ToString();
		}

		//public static implicit operator T(Property<T> property) {
		//	return property.Value;
		//}

		public void Dispose() {
			subject.Dispose();
		}
	}
}
