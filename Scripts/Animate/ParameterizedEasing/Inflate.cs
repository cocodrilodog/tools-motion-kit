namespace CocodriloDog.Animation {

	using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

	// TODO: It may be cool to choose between certain curves.

	/// <summary>
	/// This easing inflates a value and deflates it to the current value of the property with
	/// two <c>QuadInOut</c> curves.
	/// </summary>
	/// 
	/// <remarks>
	/// Note that the initial value can be different from the final value and the inflation
	/// will still occur on top of the lerp between those two values.
	/// </remarks>
	[Serializable]
	public class Inflate : ParameterizedEasing {


		#region Public Fields

		/// <summary>
		/// The maximumn value that will be added on top of the current value of the property.
		/// </summary>
		[SerializeField]
		public float InflatedOffset = 0.5f;

		#endregion


		#region Public Properties

		public override MotionBase<float, MotionFloat>.Easing FloatEasing => (a, b, t) => {
			// Get the base value so that the inflation adds on top of it.
			float baseValue = Mathf.Lerp(a, b, t);
			if (t >= 0 && t < 0.5f) {
				return baseValue + AnimateEasing.QuadInOut(0, InflatedOffset, t * 2);
			} else {
				return baseValue + AnimateEasing.QuadInOut(InflatedOffset, 0, (t * 2) - 1);
			}
		};

		public override MotionBase<Vector3, Motion3D>.Easing Vector3Easing => (a, b, t) => {

			// Get the base value so that the inflation adds on top of it.
			Vector3 baseValue = Vector3.Lerp(a, b, t);
			Vector3 inflatedValue = Vector3.one * InflatedOffset;

			if (t >= 0 && t< 0.5f) {
				return baseValue + AnimateEasing.QuadInOut(Vector3.zero, inflatedValue, t* 2);
			} else {
				return baseValue + AnimateEasing.QuadInOut(inflatedValue, Vector3.zero, (t* 2) - 1);
			}

		};

		public override MotionBase<Color, MotionColor>.Easing ColorEasing => (a, b, t) => {

			// Get the base value so that the inflation adds on top of it.
			Color baseValue = Color.Lerp(a, b, t);
			Color inflatedValue = Color.white * InflatedOffset;

			if (t >= 0 && t < 0.5f) {
				return baseValue + AnimateEasing.QuadInOut(Color.black, inflatedValue, t * 2);
			} else {
				return baseValue + AnimateEasing.QuadInOut(inflatedValue, Color.black, (t * 2) - 1);
			}

		};

		#endregion


		#region Public Constructors

		public Inflate() { }

		public Inflate(float inflatedOffset) {
			InflatedOffset = inflatedOffset;
		}

		#endregion


	}

}