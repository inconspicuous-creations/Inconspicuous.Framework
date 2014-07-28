using DryIoc;
using UniRx;

namespace Inconspicuous.Framework.Example.Test {
	[Scene("Test")]
	public class TestContext : Context {
		public TestContext(IContextView contextView, params Context[] subContexts)
			: base(contextView, subContexts) {
			container.Resolve<IViewMediationBinder>().Mediate(contextView);
			//container.Register<ICommandHandler<TestCommand, NullResult>,
			//	DimScreenEffectCommandHandlerDecorator<TestCommand, NullResult>>(setup: DecoratorSetup.With());
		}

		public override void Start() {
			container.Resolve<ICommandDispatcher>().Dispatch(new StartCommand()).Subscribe().DisposeWith(this);
		}
	}
}
