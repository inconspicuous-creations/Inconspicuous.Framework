using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using UniRx;

namespace Inconspicuous.Framework {
	[Export(typeof(CollectionProperty<>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class CollectionProperty<T> : ICollection<T> {
		private ICollection<T> collection;
		private ISubject<T> addSubject;
		private ISubject<T> removeSubject;
		private ISubject<Unit> clearSubject;

		public CollectionProperty() {
			collection = new List<T>();
			addSubject = new SuperFastSubject<T>();
			removeSubject = new SuperFastSubject<T>();
			clearSubject = new SuperFastSubject<Unit>();
		}

		public IObservable<T> AddAsObservable() {
			return addSubject;
		}

		public IObservable<T> RemoveAsObservable() {
			return removeSubject;
		}

		public IObservable<Unit> ClearAsObservable() {
			return clearSubject;
		}

		public void Add(T item) {
			collection.Add(item);
			addSubject.OnNext(item);
		}

		public void Clear() {
			collection.Clear();
			clearSubject.OnNext(Unit.Default);
		}

		public bool Contains(T item) {
			return collection.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex) {
			collection.CopyTo(array, arrayIndex);
		}

		public int Count {
			get { return collection.Count; }
		}

		public bool IsReadOnly {
			get { return false; }
		}

		public bool Remove(T item) {
			var success = collection.Remove(item);
			removeSubject.OnNext(item);
			return success;
		}

		public IEnumerator<T> GetEnumerator() {
			return collection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return collection.GetEnumerator();
		}
	}
}
