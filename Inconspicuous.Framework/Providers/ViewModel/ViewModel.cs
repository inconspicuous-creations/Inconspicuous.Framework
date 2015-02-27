using System.ComponentModel;

namespace Inconspicuous.Framework {
	public abstract class ViewModel : INotifyPropertyChanged {
		private PropertyChangedEventHandler propertyChanged;

		public event PropertyChangedEventHandler PropertyChanged {
			add { propertyChanged += value; }
			remove { propertyChanged -= value; }
		}

		protected void SetProperty<T>(ref T property, T value, string name) {
			property = value;
			OnPropertyChanged(name);
		}

		protected void OnPropertyChanged(string name) {
			if(propertyChanged != null) {
				propertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}
	}
}
