using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	[Export(typeof(IContextScheduler))]
	public class ContextScheduler : IContextScheduler {
		private readonly ContextDispatcher contextDispatcher;

		public ContextScheduler(IContextView contextView) {
			this.contextDispatcher = contextView.GameObject.AddComponent<ContextDispatcher>();
		}

		public DateTimeOffset Now {
			get { return DateTimeOffset.Now; }
		}

		public static TimeSpan Normalize(TimeSpan timeSpan) {
			return timeSpan >= TimeSpan.Zero ? timeSpan : TimeSpan.Zero;
		}

		private IEnumerator DelayAction(TimeSpan dueTime, Action action) {
			yield return new WaitForSeconds((float)dueTime.TotalSeconds);
			contextDispatcher.Post(action);
		}

		public IDisposable Schedule(Action action) {
			var d = new BooleanDisposable();
			contextDispatcher.Post(() => {
				if(!d.IsDisposed) {
					action();
				}
			});
			return d;
		}

		public IDisposable Schedule(DateTimeOffset dueTime, Action action) {
			return Schedule(dueTime - Now, action);
		}

		public IDisposable Schedule(TimeSpan dueTime, Action action) {
			var d = new BooleanDisposable();
			var time = Normalize(dueTime);

			contextDispatcher.StartCoroutine(DelayAction(time, () => {
				if(!d.IsDisposed) {
					action();
				}
			}));

			return d;
		}

		public Coroutine StartCoroutine(IEnumerator enumerator) {
			return contextDispatcher.StartCoroutine(enumerator);
		}
	}

	public class ContextDispatcher : MonoBehaviour {
		private object gate = new object();
		private Queue<Action> actionQueue = new Queue<Action>();

		public void Update() {
			lock(gate) {
				while(actionQueue.Count != 0) {
					var action = actionQueue.Dequeue();
					try {
						action();
					} catch(Exception e) {
						Debug.LogException(e);
					}
				}
			}
		}

		public void Post(Action item) {
			lock(gate) {
				actionQueue.Enqueue(item);
			}
		}

		new public Coroutine StartCoroutine(IEnumerator routine) {
			lock(gate) {
				return StartCoroutine_Auto(routine);
			}
		}
	}
}
