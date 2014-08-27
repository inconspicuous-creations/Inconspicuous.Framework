using System;
using System.ComponentModel.Composition;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework.Example.Test {
	public class TestCommand : ICommand<NullResult> {
		public int FirstValue { get; set; }
		public string SecondValue { get; set; }
	}

	[Export(typeof(ICommandHandler<TestCommand, NullResult>))]
	public class TestCommandHandler : CommandHandler<TestCommand, NullResult> {
		private readonly IContextScheduler contextScheduler;

		public TestCommandHandler(IContextScheduler contextScheduler) {
			this.contextScheduler = contextScheduler;
		}

		public override IObservable<NullResult> Handle(TestCommand command) {
			var endTime = DateTimeOffset.Now.AddSeconds(2d);
			return Observable.Interval(TimeSpan.FromSeconds(0.5d), contextScheduler)
				.Timestamp()
				.TakeWhile(t => t.Timestamp < endTime)
				.Do(d => Debug.Log(d))
				.Select(_ => NullResult.Default);
			//return Observable.Return(NullResult.Default).Delay(TimeSpan.FromSeconds(1), contextScheduler);
		}
	}
}
