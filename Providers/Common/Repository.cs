using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;

namespace Inconspicuous.Framework {
	[Export(typeof(IRepository<>))]
	public class Repository<T> : IRepository<T> where T : class, ICloneable, INamed {
		private readonly List<T> items;

		public Repository() {
			items = new List<T>();
		}

		public ReadOnlyCollection<T> Items {
			get { return items.AsReadOnly(); }
		}

		public T Retrieve(string name) {
			return (T)items.FirstOrDefault(i => i.Name == name).Clone();
		}

		public U Retrieve<U>(string name) where U : class, T {
			return (U)items.FirstOrDefault(i => i.Name == name).Clone();
		}

		public U Register<U>(U item) where U : class, T {
			var found = Items.FirstOrDefault(i => i.Name == item.Name);
			if(found != null && found is U) {
				return (U)found;
			}
			var clone = (U)item.Clone();
			items.Add(clone);
			return clone;
		}
	}
}
