using System.Collections;
using UniRx;
using UnityEngine;

namespace Inconspicuous.Framework {
	public interface IContextScheduler : IScheduler {
		Coroutine StartCoroutine(IEnumerator enumerator);
	}
}
