namespace CocodriloDog.Animation {

	using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

	// TODO: It may be cool to choose between certain curves.

	/// <summary>
	/// This easing changes the value of a property by <see cref="Offset"/> amount looking like a 
	/// pulsation and returns it to the current value of the property with one <c>QuadInOut</c>
	/// curve to change the value and other <c>QuadInOut</c> curve to restore it.
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
		/// The maximum value that will be added on top of the current value of the property
		/// when the pulsation reachs its peak.
		/// </summary>
		[SerializeField]
		public float Offset = 0.5f;

		#endregion


		#region Public Properties

		public override MotionFloat.Easing FloatEasing => (a, b, t) => {
			// Get the base value so that the pulsation adds on top of it.
			float baseValue = Mathf.Lerp(a, b, t);
			if (t >= 0 && t < 0.5f) {
				return baseValue + MotionKitEasing.QuadInOut(0, Offset, t * 2);
			} else {
				return baseValue + MotionKitEasing.QuadInOut(Offset, 0, (t * 2) - 1);
			}
		};

		public override Motion3D.Easing Vector3Easing => (a, b, t) => {

			// Get the base value so that the pulsation adds on top of it.
			Vector3 baseValue = Vector3.Lerp(a, b, t);
			Vector3 pulseOffset = Vector3.one * Offset;

			if (t >= 0 && t< 0.5f) {
				return baseValue + MotionKitEasing.QuadInOut(Vector3.zero, pulseOffset, t* 2);
			} else {
				return baseValue + MotionKitEasing.QuadInOut(pulseOffset, Vector3.zero, (t* 2) - 1);
			}

		};

		public override MotionColor.Easing ColorEasing => (a, b, t) => {

			// Get the base value so that the pulsation adds on top of it.
			Color baseValue = Color.Lerp(a, b, t);
			Color pulseOffset = Color.white * Offset;

			if (t >= 0 && t < 0.5f) {
				return baseValue + MotionKitEasing.QuadInOut(Color.black, pulseOffset, t * 2);
			} else {
				return baseValue + MotionKitEasing.QuadInOut(pulseOffset, Color.black, (t * 2) - 1);
			}

		};

		#endregion


		#region Public Constructors

		public Pulse() { }

		public Pulse(float offset) {
			Offset = offset;
		}

		#endregion


		#region Public Methods

		public override ParameterizedEasing Copy() => new Pulse(Offset);

		#endregion


	}

}