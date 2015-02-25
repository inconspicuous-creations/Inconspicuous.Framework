using System.ComponentModel;

namespace Inconspicuous.Framework {
	public abstract class ViewModel : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		protected void SetProperty<T>(ref T property, T value, string name) {
			property = value;
			OnPropertyChanged(name);
		}

		protected void OnPropertyChanged(string name) {
			PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
	}
}
