namespace CocodriloDog.Animation {

	using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

	// TODO: It may be cool to choose between certain curves.

	/// <summary>
	/// This easing pulses a value up or down and slowly returns it to the current value 
	/// of the property with two <c>QuadInOut</c> curves.
	/// </summary>
	/// 
	/// <remarks>
	/// Note that the initial value can be different from the final value and the pulsation
	/// will still occur on top of the lerp between those two values.
	/// </remarks>
	[Serializable]
	public class Pulse : ParameterizedEasing {


		#region Public Fields

		/// <summary>
		/// The maximumn value that will be added on top of the current value of the property.
		/// </summary>
		[SerializeField]
		public float PulseOffset = 0.5f;

		#endregion


		#region Public Properties

		public override MotionBase<float, MotionFloat>.Easing FloatEasing => (a, b, t) => {
			// Get the base value so that the pulsation adds on top of it.
			float baseValue = Mathf.Lerp(a, b, t);
			if (t >= 0 && t < 0.5f) {
				return baseValue + AnimateEasing.QuadInOut(0, PulseOffset, t * 2);
			} else {
				return baseValue + AnimateEasing.QuadInOut(PulseOffset, 0, (t * 2) - 1);
			}
		};

		public override MotionBase<Vector3, Motion3D>.Easing Vector3Easing => (a, b, t) => {

			// Get the base value so that the pulsation adds on top of it.
			Vector3 baseValue = Vector3.Lerp(a, b, t);
			Vector3 pulseOffset = Vector3.one * PulseOffset;

			if (t >= 0 && t< 0.5f) {
				return baseValue + AnimateEasing.QuadInOut(Vector3.zero, pulseOffset, t* 2);
			} else {
				return baseValue + AnimateEasing.QuadInOut(pulseOffset, Vector3.zero, (t* 2) - 1);
			}

		};

		public override MotionBase<Color, MotionColor>.Easing ColorEasing => (a, b, t) => {

			// Get the base value so that the pulsation adds on top of it.
			Color baseValue = Color.Lerp(a, b, t);
			Color pulseOffset = Color.white * PulseOffset;

			if (t >= 0 && t < 0.5f) {
				return baseValue + AnimateEasing.QuadInOut(Color.black, pulseOffset, t * 2);
			} else {
				return baseValue + AnimateEasing.QuadInOut(pulseOffset, Color.black, (t * 2) - 1);
			}

		};

		#endregion


		#region Public Constructors

		public Pulse() { }

		public Pulse(float offset) {
			PulseOffset = offset;
		}

		#endregion


	}

}