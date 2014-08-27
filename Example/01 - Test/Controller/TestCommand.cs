using System;
using System.ComponentModel.Composition;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework.Example.Test {
	public class TestCommand : ICommand<Unit> {
		public int FirstValue { get; set; }
		public string SecondValue { get; set; }
	}

	[Export(typeof(ICommandHandler<TestCommand, Unit>))]
	public class TestCommandHandler : CommandHandler<TestCommand, Unit> {
		private readonly IContextScheduler contextScheduler;

		public TestCommandHandler(IContextScheduler contextScheduler) {
			this.contextScheduler = contextScheduler;
		}

		public override IObservable<Unit> Handle(TestCommand command) {
			var endTime = DateTimeOffset.Now.AddSeconds(2d);
			return Observable.Interval(TimeSpan.FromSeconds(0.5d), contextScheduler)
				.Timestamp()
				.TakeWhile(t => t.Timestamp < endTime)
				.Do(d => Debug.Log(d))
				.Select(_ => Unit.Default);
			//return Observable.Return(Unit.Default).Delay(TimeSpan.FromSeconds(1), contextScheduler);
		}
	}
}
