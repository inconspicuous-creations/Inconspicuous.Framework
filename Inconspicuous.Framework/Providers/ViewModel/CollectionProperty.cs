using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using UniRx;

namespace Inconspicuous.Framework {
	[Export(typeof(CollectionProperty<>))]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public sealed class CollectionProperty<T> : ICollection<T>, IList<T>, IEnumerable<T>, IDisposable {
		private readonly List<T> collection;
		private readonly Subject<T> addSubject;
		private readonly Subject<T> removeSubject;
		private readonly Subject<Unit> clearSubject;
		private readonly Subject<T> insertSubject;
		private readonly Subject<T> removeAtSubject;
		private readonly Subject<T> updateSubject;

		public CollectionProperty() {
			this.collection = new List<T>();
			this.addSubject = new Subject<T>();
			this.removeSubject = new Subject<T>();
			this.clearSubject = new Subject<Unit>();
			this.insertSubject = new Subject<T>();
			this.removeAtSubject = new Subject<T>();
			this.updateSubject = new Subject<T>();
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

		public IObservable<T> InsertAsObservable() {
			return insertSubject;
		}

		public IObservable<T> RemoveAtAsObservable() {
			return removeAtSubject;
		}

		public IObservable<T> UpdateAsObservable() {
			return updateSubject;
		}

		public IObservable<IList<T>> AsObservable() {
			return Observable.Merge(
				addSubject.Select(_ => this as IList<T>),
				removeSubject.Select(_ => this as IList<T>),
				clearSubject.Select(_ => this as IList<T>),
				insertSubject.Select(_ => this as IList<T>),
				removeAtSubject.Select(_ => this as IList<T>),
				updateSubject.Select(_ => this as IList<T>),
				Observable.Return(this as IList<T>));
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
			insertSubject.OnNext(item);
		}

		public void RemoveAt(int index) {
			var item = collection[index];
			collection.RemoveAt(index);
			removeAtSubject.OnNext(item);
		}

		public T this[int index] {
			get { return collection[index]; }
			set {
				collection[index] = value;
				updateSubject.OnNext(value);
			}
		}

		public void Dispose() {
			addSubject.Dispose();
			removeSubject.Dispose();
			clearSubject.Dispose();
			insertSubject.Dispose();
			removeAtSubject.Dispose();
			updateSubject.Dispose();
		}
	}
}
