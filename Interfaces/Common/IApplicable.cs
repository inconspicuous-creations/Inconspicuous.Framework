namespace Inconspicuous.Framework {
	public interface IApplicable<T> {
		void Apply(T target);
		void Remove(T target);
	}
}
