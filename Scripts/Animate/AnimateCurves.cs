namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// Asset to design animation curves that can be used with <see cref="Animate"/>.
	/// </summary>
	[CreateAssetMenu(menuName = "Cocodrilo Dog/Animation/AnimateCurves")]
	public static class AnimateCurves {


		#region Public Static Methods

		/// <summary>
		/// Gets an easing function for <c>float</c> modified by the provided 
		/// <see cref="AnimationCurve"/>.
		/// </summary>
		/// <returns>The <c>float</c> easing function.</returns>
		/// <param name="curve">The <see cref="AnimationCurve"/>.</param>
		public static MotionFloat.Easing FloatEasing(AnimationCurve curve) {
			return (a, b, t) => a + (b - a) * curve.Evaluate(t);
		}

		/// <summary>
		/// Gets an easing function for <see cref="UnityEngine.Vector3"/> modified
		/// by the provided <see cref="AnimationCurve"/>.
		/// </summary>
		/// <returns>The <see cref="UnityEngine.Vector3"/> easing function.</returns>
		/// <param name="curve">The <see cref="AnimationCurve"/>.</param>
		public static Motion3D.Easing Vector3Easing(AnimationCurve curve) {
			return (a, b, t) => a + (b - a) * curve.Evaluate(t);
		}

		/// <summary>
		/// Gets an easing function for <see cref="UnityEngine.Color"/> modified
		/// by the provided <see cref="AnimationCurve"/>.
		/// </summary>
		/// <returns>The <see cref="UnityEngine.Color"/> easing function.</returns>
		/// <param name="curve">The <see cref="AnimationCurve"/>.</param>
		public static MotionColor.Easing ColorEasing(AnimationCurve curve) {
			return (a, b, t) => a + (b - a) * curve.Evaluate(t);
		}

		#endregion


	}

}