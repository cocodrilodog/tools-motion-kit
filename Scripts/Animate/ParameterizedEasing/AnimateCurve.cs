namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class AnimateCurve : ParameterizedEasing {


		#region Public Fields

		[SerializeField]
		public AnimationCurve Curve;

		#endregion


		#region Public Properties

		/// <summary>
		/// Gets an easing function for <c>float</c> modified by the 
		/// <see cref="Curve"/>.
		/// </summary>
		/// <returns>The <c>float</c> easing function.</returns>
		public override MotionFloat.Easing FloatEasing {
			get { return (a, b, t) => a + (b - a) * Curve.Evaluate(t); }
		}

		/// <summary>
		/// Gets an easing function for <c>Vector3</c> modified by the 
		/// <see cref="Curve"/>.
		/// </summary>
		/// <returns>The <c>Vector3</c> easing function.</returns>
		public override Motion3D.Easing Vector3Easing {
			get { return (a, b, t) => a + (b - a) * Curve.Evaluate(t); }
		}

		/// <summary>
		/// Gets an easing function for <c>Color</c> modified by the 
		/// <see cref="Curve"/>.
		/// </summary>
		/// <returns>The <c>Color</c> easing function.</returns>
		public override MotionColor.Easing ColorEasing {
			get { return (a, b, t) => a + (b - a) * Curve.Evaluate(t); }
		}

		#endregion


	}

}