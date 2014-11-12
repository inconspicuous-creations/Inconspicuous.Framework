using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using UniRx;

namespace Inconspicuous.Framework {
	[Export(typeof(CollectionProperty<>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public class CollectionProperty<T> : ICollection<T>, IList<T>, IEnumerable<T> {
		private List<T> collection;
		private ISubject<T> addSubject;
		private ISubject<T> removeSubject;
		private ISubject<Unit> clearSubject;
		private ISubject<Tuple<int, T>> insertSubject;
		private ISubject<int> removeAtSubject;
		private ISubject<Tuple<int, T>> updateSubject;
		private ISubject<ICollection<T>> changeSubject;

		public CollectionProperty() {
			collection = new List<T>();
			addSubject = new SuperFastSubject<T>();
			removeSubject = new SuperFastSubject<T>();
			clearSubject = new SuperFastSubject<Unit>();
			insertSubject = new SuperFastSubject<Tuple<int, T>>();
			removeAtSubject = new SuperFastSubject<int>();
			updateSubject = new SuperFastSubject<Tuple<int, T>>();
			changeSubject = new SuperFastSubject<ICollection<T>>();
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

		public IObservable<Tuple<int, T>> InsertAsObservable() {
			return insertSubject;
		}

		public IObservable<int> RemoveAtAsObservable() {
			return removeAtSubject;
		}

		public IObservable<Tuple<int, T>> UpdateAsObservable() {
			return updateSubject;
		}

		public IObservable<CollectionProperty<T>> AsObservable() {
			return Observable.Merge(
				addSubject.Select(_ => this),
				removeSubject.Select(_ => this),
				clearSubject.Select(_ => this),
				insertSubject.Select(_ => this),
				removeAtSubject.Select(_ => this),
				updateSubject.Select(_ => this),
				Observable.Return(this));
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

		public int IndexOf(T item) {
			return collection.IndexOf(item);
		}

		public void Insert(int index, T item) {
			collection.Insert(index, item);
			insertSubject.OnNext(Tuple.Create(index, item));
		}

		public void RemoveAt(int index) {
			collection.RemoveAt(index);
			removeAtSubject.OnNext(index);
		}

		public T this[int index] {
			get { return collection[index]; }
			set {
				collection[index] = value;
				updateSubject.OnNext(Tuple.Create(index, value));
			}
		}
	}
}
