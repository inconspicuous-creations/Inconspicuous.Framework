using System;
using System.Collections.Generic;

namespace Inconspicuous.Common {
	public class Distribution {
		private Random random;

		public Distribution(int seed) {
			random = new Random(seed);
		}

		public double Normal(double m, double v) {
			double u1 = random.NextDouble();
			double u2 = random.NextDouble();
			double r = Math.Sqrt(-2 * Math.Log(u1));
			double s = 2 * Math.PI * u2;
			//double z1 = v * r * Math.Sin(s) + m;
			return v * r * Math.Cos(s) + m;
		}

		public T SampleFromEmpirical<T>(List<KeyValuePair<T, float>> table) {
			if(table.Count > 0) {
				var cumulative = new float[table.Count];
				for(int i = 0; i < table.Count; i++) {
					float last = i - 1 >= 0 ? cumulative[i - 1] : 0;
					cumulative[i] = table[i].Value + last;
				}
				float sum = cumulative[cumulative.Length - 1];
				var n = (float)random.NextDouble();
				for(int i = 0; i < cumulative.Length; i++) {
					if((cumulative[i] / sum) > n) {
						return table[i].Key;
					}
				}
				return table[0].Key;
			}
			return default(T);
		}

		public T SampleFromEmpirical<T>(Dictionary<T, float> table) {
			var list = new List<KeyValuePair<T, float>>();
			foreach(var t in table) {
				list.Add(new KeyValuePair<T, float>(t.Key, t.Value));
			}
			return SampleFromEmpirical(list);
		}
	}
}
