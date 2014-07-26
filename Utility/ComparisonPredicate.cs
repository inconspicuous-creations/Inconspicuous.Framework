using System;

namespace Inconspicuous.Common {
	public enum ComparisonPredicate {
		Equal,
		Unequal,
		Less,
		LessThanOrEqual,
		Greater,
		GreaterOrEqual
	}

	public static class ComparisonPredicateExtensions {
		public static bool Compare(this ComparisonPredicate compareType, IComparable a, IComparable b) {
			switch(compareType) {
				case ComparisonPredicate.Equal:
					return a.CompareTo(b) == 0;
				case ComparisonPredicate.Unequal:
					return a.CompareTo(b) != 0;
				case ComparisonPredicate.Less:
					return a.CompareTo(b) < 0;
				case ComparisonPredicate.LessThanOrEqual:
					return a.CompareTo(b) <= 0;
				case ComparisonPredicate.Greater:
					return a.CompareTo(b) > 0;
				case ComparisonPredicate.GreaterOrEqual:
					return a.CompareTo(b) >= 0;
				default:
					return false;
			}
		}

		public static string ToString(this ComparisonPredicate compareType) {
			switch(compareType) {
				case ComparisonPredicate.Equal:
					return "";
				case ComparisonPredicate.Unequal:
					return "Different than ";
				case ComparisonPredicate.Less:
					return "Less than ";
				case ComparisonPredicate.LessThanOrEqual:
					return "Less or equal to ";
				case ComparisonPredicate.Greater:
					return "More than ";
				case ComparisonPredicate.GreaterOrEqual:
					return "More or equal to ";
				default:
					return "";
			}
		}

		public static string ToOperatorString(this ComparisonPredicate compareType) {
			switch(compareType) {
				case ComparisonPredicate.Equal:
					return "=";
				case ComparisonPredicate.Unequal:
					return "<>";
				case ComparisonPredicate.Less:
					return "<";
				case ComparisonPredicate.LessThanOrEqual:
					return "<=";
				case ComparisonPredicate.Greater:
					return ">";
				case ComparisonPredicate.GreaterOrEqual:
					return ">=";
				default:
					return "";
			}
		}
	}
}
