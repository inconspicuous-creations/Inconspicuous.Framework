namespace Inconspicuous.Framework {
	public interface IMainContextConfiguration {
		void Configure(IContainer container);
		void Start();
	}
}
