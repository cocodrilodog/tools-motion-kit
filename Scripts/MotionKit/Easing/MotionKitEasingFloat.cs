namespace CocodriloDog.MotionKit {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Easing functions to be used with <see cref="MotionKit"/> classes.
	/// </summary>
	/// 
	/// <remarks>
	/// 
	/// This methods are taken from DOTween. They are governed by Microsoft Public 
	/// License (Ms-PL).
	/// 
	/// The original functions are wrapped so that their signature complies with
	/// the <see cref="MotionBase{T}.Easing"/> signature. For reference, below is
	/// the description the parameters of the original functions:
	/// 
	/// Param: t
	/// The current time(or position) of the tween.This can be seconds or frames,
	/// steps, seconds, ms, whatever – as long as the unit is the same as is 
	/// used for the total time d.
	/// 
	/// Param: b
	/// The beginning value of the property.
	/// 
	/// Param: c
	/// The change between the beginning and destination value of the property.
	/// 
	/// Param: d 
	/// The total duration of the tween.
	/// 
	/// </remarks>
	public static partial class MotionKitEasing {


		#region Public Methods - Back

		public static float BackIn(float a, float b, float t) {
			return BackIn(t, a, b - a, 1);
		}

		public static float BackOut(float a, float b, float t) {
			return BackOut(t, a, b - a, 1);
		}

		public static float BackInOut(float a, float b, float t) {
			return BackInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Bounce

		public static float BounceIn(float a, float b, float t) {
			return BounceIn(t, a, b - a, 1);
		}

		public static float BounceOut(float a, float b, float t) {
			return BounceOut(t, a, b - a, 1);
		}

		public static float BounceInOut(float a, float b, float t) {
			return BounceInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Circular

		public static float CircIn(float a, float b, float t) {
			return CircIn(t, a, b - a, 1);
		}

		public static float CircOut(float a, float b, float t) {
			return CircOut(t, a, b - a, 1);
		}

		public static float CircInOut(float a, float b, float t) {
			return CircInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Elastic

		public static float ElasticIn(float a, float b, float t) {
			return ElasticIn(t, a, b - a, 1);
		}

		public static float ElasticOut(float a, float b, float t) {
			return ElasticOut(t, a, b - a, 1);
		}

		public static float ElasticInOut(float a, float b, float t) {
			return ElasticInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Expo

		public static float ExpoIn(float a, float b, float t) {
			return ExpoIn(t, a, b - a, 1);
		}

		public static float ExpoOut(float a, float b, float t) {
			return ExpoOut(t, a, b - a, 1);
		}

		public static float ExpoInOut(float a, float b, float t) {
			return ExpoInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Linear

		public static float Linear(float a, float b, float t) {
			return Linear(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Quadratic

		public static float QuadIn(float a, float b, float t) {
			return QuadIn(t, a, b - a, 1);
		}

		public static float QuadOut(float a, float b, float t) {
			return QuadOut(t, a, b - a, 1);
		}

		public static float QuadInOut(float a, float b, float t) {
			return QuadInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Quartic

		public static float QuartIn(float a, float b, float t) {
			return QuartIn(t, a, b - a, 1);
		}

		public static float QuartOut(float a, float b, float t) {
			return QuartOut(t, a, b - a, 1);
		}

		public static float QuartInOut(float a, float b, float t) {
			return QuartInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Quintic

		public static float QuintIn(float a, float b, float t) {
			return QuintIn(t, a, b - a, 1);
		}

		public static float QuintOut(float a, float b, float t) {
			return QuintOut(t, a, b - a, 1);
		}

		public static float QuintInOut(float a, float b, float t) {
			return QuintInOut(t, a, b - a, 1);
		}

		#endregion


		#region Public Methods - Sinusoidal

		public static float SinusIn(float a, float b, float t) {
			return SinusIn(t, a, b - a, 1);
		}

		public static float SinusOut(float a, float b, float t) {
			return SinusOut(t, a, b - a, 1);
		}

		public static float SinusInOut(float a, float b, float t) {
			return SinusInOut(t, a, b - a, 1);
		}

		#endregion


		#region Internal Methods - Back

		private static float BackIn(float t, float b, float c, float d) {
			return c * (t /= d) * t * ((1.70158f + 1) * t - 1.70158f) + b;
		}


		private static float BackOut(float t, float b, float c, float d) {
			return c * ((t = t / d - 1) * t * ((1.70158f + 1) * t + 1.70158f) + 1) + b;
		}


		private static float BackInOut(float t, float b, float c, float d) {
			float s = 1.70158f;
			if ((t /= d / 2) < 1) {
				return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
			}
			return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
		}

		#endregion


		#region Internal Methods - Bounce

		private static float BounceIn(float t, float b, float c, float d) {
			return c - BounceOut(d - t, 0, c, d) + b;
		}

		private static float BounceOut(float t, float b, float c, float d) {
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

		private static float BounceInOut(float t, float b, float c, float d) {
			if (t < d / 2)
				return BounceIn(t * 2, 0, c, d) * 0.5f + b;
			else
				return BounceOut(t * 2 - d, 0, c, d) * .5f + c * 0.5f + b;
		}

		#endregion


		#region Internal Methods - Circular

		private static float CircIn(float t, float b, float c, float d) {
			return -c * (Mathf.Sqrt(1 - (t /= d) * t) - 1) + b;
		}

		private static float CircOut(float t, float b, float c, float d) {
			return c * Mathf.Sqrt(1 - (t = t / d - 1) * t) + b;
		}

		private static float CircInOut(float t, float b, float c, float d) {
			if ((t /= d / 2) < 1) {
				return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
			}
			return c / 2 * (Mathf.Sqrt(1 - (t -= 2) * t) + 1) + b;
		}

		#endregion


		#region Internal Methods - Elastic

		private static float ElasticIn(float t, float b, float c, float d) {
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

		private static float ElasticOut(float t, float b, float c, float d) {
			if (t == 0) {
				return b;
			}
			if ((t /= d) == 1) {
				return b + c;
			}
			float p = d * .3f;
			float s = p / 4;
			return (c * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + c + b);
		}

		private static float ElasticInOut(float t, float b, float c, float d) {
			if (t == 0) {
				return b;
			}
			if ((t /= d / 2) == 2) {
				return b + c;
			}
			float p = d * (.3f * 1.5f);
			float a = c;
			float s = p / 4;
			if (t < 1) {
				return -.5f * (a * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
			}
			return (float)(a * Mathf.Pow(2, -10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) * .5 + c + b);
		}

		#endregion


		#region Public Methods - Expo

		private static float ExpoIn(float t, float b, float c, float d) {
			return (t == 0) ? b : c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
		}

		private static float ExpoOut(float t, float b, float c, float d) {
			return (t == d) ? b + c : c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
		}

		private static float ExpoInOut(float t, float b, float c, float d) {
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

		private static float Linear(float t, float b, float c, float d) {
			return c * t / d + b;
		}

		#endregion


		#region Internal Methods - Quadratic

		private static float QuadIn(float t, float b, float c, float d) {
			return c * (t /= d) * t + b;
		}

		private static float QuadOut(float t, float b, float c, float d) {
			return -c * (t /= d) * (t - 2) + b;
		}

		private static float QuadInOut(float t, float b, float c, float d) {
			if ((t /= d / 2) < 1) {
				return c / 2 * t * t + b;
			}
			return -c / 2 * ((--t) * (t - 2) - 1) + b;
		}

		#endregion


		#region Internal Methods - Quartic

		private static float QuartIn(float t, float b, float c, float d) {
			return c * (t /= d) * t * t * t + b;
		}

		private static float QuartOut(float t, float b, float c, float d) {
			return -c * ((t = t / d - 1) * t * t * t - 1) + b;
		}

		private static float QuartInOut(float t, float b, float c, float d) {
			if ((t /= d / 2) < 1) {
				return c / 2 * t * t * t * t + b;
			}
			return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
		}

		#endregion


		#region Internal Methods - Quintic

		private static float QuintIn(float t, float b, float c, float d) {
			return c * (t /= d) * t * t * t * t + b;
		}

		private static float QuintOut(float t, float b, float c, float d) {
			return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
		}

		private static float QuintInOut(float t, float b, float c, float d) {
			if ((t /= d / 2) < 1) {
				return c / 2 * t * t * t * t * t + b;
			}
			return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
		}

		#endregion


		#region Internal Methods - Sinusoidal

		private static float SinusIn(float t, float b, float c, float d) {
			return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
		}

		private static float SinusOut(float t, float b, float c, float d) {
			return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
		}

		private static float SinusInOut(float t, float b, float c, float d) {
			return -c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
		}

		#endregion
	}
}