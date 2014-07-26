namespace Inconspicuous.Framework {
	public interface ICommand { }
	public interface ICommand<TResult> : ICommand where TResult : IResult { }
	public interface IResult { }
}
