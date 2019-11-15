namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public static partial class AnimateEasing {


		#region Public Methods - Back

		public static Vector3 BackIn(Vector3 a, Vector3 b, float t) {
			return BackIn(t, a, b - a, 1);
		}

		public static Vector3 BackOut(Vector3 a, Vector3 b, float t) {
			return BackOut(t, a, b - a, 1);
		}

		public static Vector3 BackInOut(Vector3 a, Vector3 b, float t) {
			return BackInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Bounce

		public static Vector3 BounceIn(Vector3 a, Vector3 b, float t) {
			return BounceIn(t, a, b - a, 1);
		}

		public static Vector3 BounceOut(Vector3 a, Vector3 b, float t) {
			return BounceOut(t, a, b - a, 1);
		}

		public static Vector3 BounceInOut(Vector3 a, Vector3 b, float t) {
			return BounceInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Circular

		public static Vector3 CircIn(Vector3 a, Vector3 b, float t) {
			return CircIn(t, a, b - a, 1);
		}

		public static Vector3 CircOut(Vector3 a, Vector3 b, float t) {
			return CircOut(t, a, b - a, 1);
		}

		public static Vector3 CircInOut(Vector3 a, Vector3 b, float t) {
			return CircInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Elastic

		public static Vector3 ElasticIn(Vector3 a, Vector3 b, float t) {
			return ElasticIn(t, a, b - a, 1);
		}

		public static Vector3 ElasticOut(Vector3 a, Vector3 b, float t) {
			return ElasticOut(t, a, b - a, 1);
		}

		public static Vector3 ElasticInOut(Vector3 a, Vector3 b, float t) {
			return ElasticInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Expo

		public static Vector3 ExpoIn(Vector3 a, Vector3 b, float t) {
			return ExpoIn(t, a, b - a, 1);
		}

		public static Vector3 ExpoOut(Vector3 a, Vector3 b, float t) {
			return ExpoOut(t, a, b - a, 1);
		}

		public static Vector3 ExpoInOut(Vector3 a, Vector3 b, float t) {
			return ExpoInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Linear

		public static Vector3 Linear(Vector3 a, Vector3 b, float t) {
			return Linear(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Quadratic

		public static Vector3 QuadIn(Vector3 a, Vector3 b, float t) {
			return QuadIn(t, a, b - a, 1);
		}

		public static Vector3 QuadOut(Vector3 a, Vector3 b, float t) {
			return QuadOut(t, a, b - a, 1);
		}

		public static Vector3 QuadInOut(Vector3 a, Vector3 b, float t) {
			return QuadInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Quartic

		public static Vector3 QuartIn(Vector3 a, Vector3 b, float t) {
			return QuartIn(t, a, b - a, 1);
		}

		public static Vector3 QuartOut(Vector3 a, Vector3 b, float t) {
			return QuartOut(t, a, b - a, 1);
		}

		public static Vector3 QuartInOut(Vector3 a, Vector3 b, float t) {
			return QuartInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Quintic

		public static Vector3 QuintIn(Vector3 a, Vector3 b, float t) {
			return QuintIn(t, a, b - a, 1);
		}

		public static Vector3 QuintOut(Vector3 a, Vector3 b, float t) {
			return QuintOut(t, a, b - a, 1);
		}

		public static Vector3 QuintInOut(Vector3 a, Vector3 b, float t) {
			return QuintInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Sinusoidal

		public static Vector3 SinusIn(Vector3 a, Vector3 b, float t) {
			return SinusIn(t, a, b - a, 1);
		}

		public static Vector3 SinusOut(Vector3 a, Vector3 b, float t) {
			return SinusOut(t, a, b - a, 1);
		}

		public static Vector3 SinusInOut(Vector3 a, Vector3 b, float t) {
			return SinusInOut(t, a, b - a, 1);
		}

		#endregion


		#region Internal Methods - Back

		private static Vector3 BackIn(float t, Vector3 b, Vector3 c, float d) {
			return c * (t /= d) * t * ((1.70158f + 1) * t - 1.70158f) + b;
		}


		public static Vector3 BackOut(float t, Vector3 b, Vector3 c, float d) {
			return c * ((t = t / d - 1) * t * ((1.70158f + 1) * t + 1.70158f) + 1) + b;
		}


		private static Vector3 BackInOut(float t, Vector3 b, Vector3 c, float d) {
			float s = 1.70158f;
			if ((t /= d / 2) < 1) {
				return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
			}
			return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
		}

		#endregion


		#region Internal Methods - Bounce

		private static Vector3 BounceIn(float t, Vector3 b, Vector3 c, float d) {
			return c - BounceOut(d - t, Vector3.zero, c, d) + b;
		}

		private static Vector3 BounceOut(float t, Vector3 b, Vector3 c, float d) {
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

		private static Vector3 BounceInOut(float t, Vector3 b, Vector3 c, float d) {
			if (t < d / 2)
				return BounceIn(t * 2, Vector3.zero, c, d) * 0.5f + b;
			else
				return BounceOut(t * 2 - d, Vector3.zero, c, d) * .5f + c * 0.5f + b;
		}

		#endregion


		#region Internal Methods - Circular

		private static Vector3 CircIn(float t, Vector3 b, Vector3 c, float d) {
			return -c * (Mathf.Sqrt(1 - (t /= d) * t) - 1) + b;
		}

		private static Vector3 CircOut(float t, Vector3 b, Vector3 c, float d) {
			return c * Mathf.Sqrt(1 - (t = t / d - 1) * t) + b;
		}

		private static Vector3 CircInOut(float t, Vector3 b, Vector3 c, float d) {
			if ((t /= d / 2) < 1) {
				return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
			}
			return c / 2 * (Mathf.Sqrt(1 - (t -= 2) * t) + 1) + b;
		}

		#endregion


		#region Internal Methods - Elastic

		private static Vector3 ElasticIn(float t, Vector3 b, Vector3 c, float d) {
			if (t == 0) {
				return b;
			}
			if ((t /= d) == 1) {
				return b + c;
			}
			float p = d * .3f;
			float s = p / 4;
			return -(c * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
		}

		private static Vector3 ElasticOut(float t, Vector3 b, Vector3 c, float d) {
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

		private static Vector3 ElasticInOut(float t, Vector3 b, Vector3 c, float d) {
			if (t == 0) {
				return b;
			}
			if ((t /= d / 2) == 2) {
				return b + c;
			}
			float p = d * (.3f * 1.5f);
			Vector3 a = c;
			float s = p / 4;
			if (t < 1) {
				return -.5f * (a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
			}
			return a * Mathf.Pow(2, -10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) * .5f + c + b;
		}

		#endregion


		#region Public Methods - Expo

		private static Vector3 ExpoIn(float t, Vector3 b, Vector3 c, float d) {
			return (t == 0) ? b : c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
		}

		private static Vector3 ExpoOut(float t, Vector3 b, Vector3 c, float d) {
			return (t == d) ? b + c : c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
		}

		private static Vector3 ExpoInOut(float t, Vector3 b, Vector3 c, float d) {
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

		private static Vector3 Linear(float t, Vector3 b, Vector3 c, float d) {
			return c * t / d + b;
		}

		#endregion


		#region Internal Methods - Quadratic

		private static Vector3 QuadIn(float t, Vector3 b, Vector3 c, float d) {
			return c * (t /= d) * t + b;
		}

		private static Vector3 QuadOut(float t, Vector3 b, Vector3 c, float d) {
			return -c * (t /= d) * (t - 2) + b;
		}

		private static Vector3 QuadInOut(float t, Vector3 b, Vector3 c, float d) {
			if ((t /= d / 2) < 1) {
				return c / 2 * t * t + b;
			}
			return -c / 2 * ((--t) * (t - 2) - 1) + b;
		}

		#endregion


		#region Internal Methods - Quartic

		private static Vector3 QuartIn(float t, Vector3 b, Vector3 c, float d) {
			return c * (t /= d) * t * t * t + b;
		}

		private static Vector3 QuartOut(float t, Vector3 b, Vector3 c, float d) {
			return -c * ((t = t / d - 1) * t * t * t - 1) + b;
		}

		private static Vector3 QuartInOut(float t, Vector3 b, Vector3 c, float d) {
			if ((t /= d / 2) < 1) {
				return c / 2 * t * t * t * t + b;
			}
			return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
		}

		#endregion


		#region Internal Methods - Quintic

		private static Vector3 QuintIn(float t, Vector3 b, Vector3 c, float d) {
			return c * (t /= d) * t * t * t * t + b;
		}

		private static Vector3 QuintOut(float t, Vector3 b, Vector3 c, float d) {
			return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
		}

		private static Vector3 QuintInOut(float t, Vector3 b, Vector3 c, float d) {
			if ((t /= d / 2) < 1) {
				return c / 2 * t * t * t * t * t + b;
			}
			return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
		}

		#endregion


		#region Internal Methods - Sinusoidal

		private static Vector3 SinusIn(float t, Vector3 b, Vector3 c, float d) {
			return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
		}

		private static Vector3 SinusOut(float t, Vector3 b, Vector3 c, float d) {
			return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
		}

		private static Vector3 SinusInOut(float t, Vector3 b, Vector3 c, float d) {
			return -c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
		}

		#endregion
	}
}