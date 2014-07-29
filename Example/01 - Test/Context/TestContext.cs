using System;
using System.ComponentModel.Composition;
using DryIoc;
using UniRx;

namespace Inconspicuous.Framework.Example.Test {
	public interface IService {
		void Execute();
	}

	[Export(typeof(IService))]
	public class Service : IService {
		public void Execute() {
			UnityEngine.Debug.Log("Test");
		}
	}

	[Scene("Test")]
	public class TestContext : Context {
		public TestContext(IContextView contextView, params Context[] subContexts)
			: base(contextView, subContexts) {
			container.RegisterDelegate<Lazy<IService>>(registry => new Lazy<IService>(() => registry.Resolve<IService>()));
			//container.Register<ICommandHandler<TestCommand, NullResult>,
			//	DimScreenEffectCommandHandlerDecorator<TestCommand, NullResult>>(setup: DecoratorSetup.With());
			//var service = container.Resolve<Lazy<IService>>();
			container.Resolve<Lazy<IService>>().Value.Execute();
		}

		public override void Start() {
			container.Resolve<ICommandDispatcher>().Dispatch(new StartCommand()).Subscribe().DisposeWith(this);
		}
	}
}
