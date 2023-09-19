namespace CocodriloDog.MotionKit {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public static partial class MotionKitEasing {


		#region Public Methods - Back

		public static Color BackIn(Color a, Color b, float t) {
			return BackIn(t, a, b - a, 1);
		}

		public static Color BackOut(Color a, Color b, float t) {
			return BackOut(t, a, b - a, 1);
		}

		public static Color BackInOut(Color a, Color b, float t) {
			return BackInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Bounce

		public static Color BounceIn(Color a, Color b, float t) {
			return BounceIn(t, a, b - a, 1);
		}

		public static Color BounceOut(Color a, Color b, float t) {
			return BounceOut(t, a, b - a, 1);
		}

		public static Color BounceInOut(Color a, Color b, float t) {
			return BounceInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Circular

		public static Color CircIn(Color a, Color b, float t) {
			return CircIn(t, a, b - a, 1);
		}

		public static Color CircOut(Color a, Color b, float t) {
			return CircOut(t, a, b - a, 1);
		}

		public static Color CircInOut(Color a, Color b, float t) {
			return CircInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Elastic

		public static Color ElasticIn(Color a, Color b, float t) {
			return ElasticIn(t, a, b - a, 1);
		}

		public static Color ElasticOut(Color a, Color b, float t) {
			return ElasticOut(t, a, b - a, 1);
		}

		public static Color ElasticInOut(Color a, Color b, float t) {
			return ElasticInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Expo

		public static Color ExpoIn(Color a, Color b, float t) {
			return ExpoIn(t, a, b - a, 1);
		}

		public static Color ExpoOut(Color a, Color b, float t) {
			return ElasticOut(t, a, b - a, 1);
		}

		public static Color ExpoInOut(Color a, Color b, float t) {
			return ElasticInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Linear

		public static Color Linear(Color a, Color b, float t) {
			return Linear(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Quadratic

		public static Color QuadIn(Color a, Color b, float t) {
			return QuadIn(t, a, b - a, 1);
		}

		public static Color QuadOut(Color a, Color b, float t) {
			return QuadOut(t, a, b - a, 1);
		}

		public static Color QuadInOut(Color a, Color b, float t) {
			return QuadInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Quartic

		public static Color QuartIn(Color a, Color b, float t) {
			return QuartIn(t, a, b - a, 1);
		}

		public static Color QuartOut(Color a, Color b, float t) {
			return QuartOut(t, a, b - a, 1);
		}

		public static Color QuartInOut(Color a, Color b, float t) {
			return QuartInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Quintic

		public static Color QuintIn(Color a, Color b, float t) {
			return QuintIn(t, a, b - a, 1);
		}

		public static Color QuintOut(Color a, Color b, float t) {
			return QuintOut(t, a, b - a, 1);
		}

		public static Color QuintInOut(Color a, Color b, float t) {
			return QuintInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Sinusoidal

		public static Color SinusIn(Color a, Color b, float t) {
			return SinusIn(t, a, b - a, 1);
		}

		public static Color SinusOut(Color a, Color b, float t) {
			return SinusOut(t, a, b - a, 1);
		}

		public static Color SinusInOut(Color a, Color b, float t) {
			return SinusInOut(t, a, b - a, 1);
		}

		#endregion


		#region Internal Methods - Back

		private static Color BackIn(float t, Color b, Color c, float d) {
			return c * (t /= d) * t * ((1.70158f + 1) * t - 1.70158f) + b;
		}


		public static Color BackOut(float t, Color b, Color c, float d) {
			return c * ((t = t / d - 1) * t * ((1.70158f + 1) * t + 1.70158f) + 1) + b;
		}


		private static Color BackInOut(float t, Color b, Color c, float d) {
			float s = 1.70158f;
			if ((t /= d / 2) < 1) {
				return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
			}
			return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
		}

		#endregion


		#region Internal Methods - Bounce

		private static Color BounceIn(float t, Color b, Color c, float d) {
			return c - BounceOut(d - t, Color.black, c, d) + b;
		}

		private static Color BounceOut(float t, Color b, Color c, float d) {
			if ((t /= d) < (1 / 2.75)) {
				return c * (7.5625f * t * t) + b;
			} else if (t < (2 / 2.75)) {
				return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
			} else if (t < (2.5 / 2.75)) {
				return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
			} else {
				return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
			}
		}

		private static Color BounceInOut(float t, Color b, Color c, float d) {
			if (t < d / 2)
				return BounceIn(t * 2, Color.black, c, d) * 0.5f + b;
			else
				return BounceOut(t * 2 - d, Color.black, c, d) * .5f + c * 0.5f + b;
		}

		#endregion


		#region Internal Methods - Circular

		private static Color CircIn(float t, Color b, Color c, float d) {
			return -1 * c * (Mathf.Sqrt(1 - (t /= d) * t) - 1) + b;
		}

		private static Color CircOut(float t, Color b, Color c, float d) {
			return c * Mathf.Sqrt(1 - (t = t / d - 1) * t) + b;
		}

		private static Color CircInOut(float t, Color b, Color c, float d) {
			if ((t /= d / 2) < 1) {
				return -1 * c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
			}
			return c / 2 * (Mathf.Sqrt(1 - (t -= 2) * t) + 1) + b;
		}

		#endregion


		#region Internal Methods - Elastic

		private static Color ElasticIn(float t, Color b, Color c, float d) {
			if (t == 0) {
				return b;
			}
			if ((t /= d) == 1) {
				return b + c;
			}
			float p = d * .3f;
			float s = p / 4;
			return -1 * (c * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
		}

		private static Color ElasticOut(float t, Color b, Color c, float d) {
			if (t == 0) {
				return b;
			}
			if ((t /= d) == 1) {
				return b + c;
			}
			float p = d * .3f;
			float s = p / 4;
			return c * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + c + b;
		}

		private static Color ElasticInOut(float t, Color b, Color c, float d) {
			if (t == 0) {
				return b;
			}
			if ((t /= d / 2) == 2) {
				return b + c;
			}
			float p = d * (.3f * 1.5f);
			Color a = c;
			float s = p / 4;
			if (t < 1) {
				return -.5f * (a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
			}
			return a * Mathf.Pow(2, -10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) * .5f + c + b;
		}

		#endregion


		#region Public Methods - Expo

		private static Color ExpoIn(float t, Color b, Color c, float d) {
			return (t == 0) ? b : c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
		}

		private static Color ExpoOut(float t, Color b, Color c, float d) {
			return (t == d) ? b + c : c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
		}

		private static Color ExpoInOut(float t, Color b, Color c, float d) {
			if (t == 0) {
				return b;
			}
			if (t == d) {
				return b + c;
			}
			if ((t /= d / 2) < 1) {
				return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b;
			}
			return c / 2 * (-Mathf.Pow(2, -10 * --t) + 2) + b;
		}

		#endregion


		#region Internal Methods - Linear

		private static Color Linear(float t, Color b, Color c, float d) {
			return c * t / d + b;
		}

		#endregion


		#region Internal Methods - Quadratic

		private static Color QuadIn(float t, Color b, Color c, float d) {
			return c * (t /= d) * t + b;
		}

		private static Color QuadOut(float t, Color b, Color c, float d) {
			return -1 * c * (t /= d) * (t - 2) + b;
		}

		private static Color QuadInOut(float t, Color b, Color c, float d) {
			if ((t /= d / 2) < 1) {
				return c / 2 * t * t + b;
			}
			return -1 * c / 2 * ((--t) * (t - 2) - 1) + b;
		}

		#endregion


		#region Internal Methods - Quartic

		private static Color QuartIn(float t, Color b, Color c, float d) {
			return c * (t /= d) * t * t * t + b;
		}

		private static Color QuartOut(float t, Color b, Color c, float d) {
			return -1 * c * ((t = t / d - 1) * t * t * t - 1) + b;
		}

		private static Color QuartInOut(float t, Color b, Color c, float d) {
			if ((t /= d / 2) < 1) {
				return c / 2 * t * t * t * t + b;
			}
			return -1 * c / 2 * ((t -= 2) * t * t * t - 2) + b;
		}

		#endregion


		#region Internal Methods - Quintic

		private static Color QuintIn(float t, Color b, Color c, float d) {
			return c * (t /= d) * t * t * t * t + b;
		}

		private static Color QuintOut(float t, Color b, Color c, float d) {
			return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
		}

		private static Color QuintInOut(float t, Color b, Color c, float d) {
			if ((t /= d / 2) < 1) {
				return c / 2 * t * t * t * t * t + b;
			}
			return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
		}

		#endregion


		#region Internal Methods - Sinusoidal

		private static Color SinusIn(float t, Color b, Color c, float d) {
			return -1 * c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
		}

		private static Color SinusOut(float t, Color b, Color c, float d) {
			return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
		}

		private static Color SinusInOut(float t, Color b, Color c, float d) {
			return -1 * c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
		}

		#endregion
	}
}