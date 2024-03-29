﻿namespace CocodriloDog.MotionKit {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// AnimationCurve easing compatible with <see cref="MotionKit"/>.
	/// </summary>
	[Serializable]
	public class MotionKitCurve : ParameterizedEasing {


		#region Public Fields

		/// <summary>
		/// The animation curve.
		/// </summary>
		[SerializeField]
		public AnimationCurve Curve;

		#endregion


		#region Public Properties

		/// <summary>
		/// Gets an easing function for <c>float</c> modified by the 
		/// <see cref="Curve"/>.
		/// </summary>
		public override MotionFloat.Easing FloatEasing {
			get { return (a, b, t) => a + (b - a) * Curve.Evaluate(t); }
		}

		/// <summary>
		/// Gets an easing function for <c>Vector3</c> modified by the 
		/// <see cref="Curve"/>.
		/// </summary>
		public override Motion3D.Easing Vector3Easing {
			get { return (a, b, t) => a + (b - a) * Curve.Evaluate(t); }
		}

		/// <summary>
		/// Gets an easing function for <c>Color</c> modified by the 
		/// <see cref="Curve"/>.
		/// </summary>
		public override MotionColor.Easing ColorEasing {
			get { return (a, b, t) => a + (b - a) * Curve.Evaluate(t); }
		}

		#endregion


		#region Public Methods

		public override ParameterizedEasing Copy() {
			var copy = new MotionKitCurve();
			copy.Curve = new AnimationCurve(Curve.keys);
			return copy;
		}

		#endregion


	}

}