using UniRx;

namespace Inconspicuous.Framework.Example.Test {
	[Scene("Test")]
	public class TestContext : Context {
		public TestContext(IContextView contextView, params Context[] subContexts)
			: base(contextView, subContexts) {
			//container.Register<ICommandHandler<TestCommand, Unit>,
			//	DimScreenEffectCommandHandlerDecorator<TestCommand, Unit>>(setup: DecoratorSetup.With());
		}

		public override void Start() {
			container.Resolve<ICommandDispatcher>().Dispatch(new StartCommand()).Subscribe().AddTo(this);
		}
	}
}
