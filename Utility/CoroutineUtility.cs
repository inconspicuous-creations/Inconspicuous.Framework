using System;
using System.Collections;
using UnityEngine;

namespace Inconspicuous.Common {
	public static class CoroutineUtility {
		public static IEnumerator ExecuteUntil(Func<bool> func) {
			bool check = false;
			while(!check) {
				check = func();
				yield return null;
			}
		}

		public static IEnumerator ExecuteForDuration(Action func, float duration) {
			float elapsedTime = 0f;
			while(elapsedTime < duration) {
				func();
				elapsedTime += Time.deltaTime;
				yield return null;
			}
		}

		public static IEnumerator ExecuteForDuration(Action<float> func, float duration) {
			float elapsedTime = 0f;
			while(elapsedTime < duration) {
				func(elapsedTime);
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			func(duration);
		}

		public static IEnumerator ExecuteWithDelay(Action func, float delay) {
			float elapsedTime = 0f;
			while(elapsedTime < delay) {
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			func();
		}

		public static IEnumerator ExecuteEvery(Action func, float time = 1f) {
			float elapsedTime = 0f;
			while(true) {
				if(elapsedTime > time) {
					func();
					elapsedTime -= time;
				}
				elapsedTime += Time.deltaTime;
				yield return null;
			}
		}

		public static IEnumerator ExecuteEvery(Func<bool> func, float time = 1f) {
			float elapsedTime = 0f;
			bool check = false;
			while(!check) {
				if(elapsedTime > time) {
					check = func();
					elapsedTime -= time;
				}
				elapsedTime += Time.deltaTime;
				yield return null;
			}
		}
	}
}
