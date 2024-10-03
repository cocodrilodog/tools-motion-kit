namespace CocodriloDog.MotionKit {

	using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

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
		public AnimatableValue Offset => m_Offset = m_Offset ?? new AnimatableValue();

		#endregion


		#region Public Properties

		public override MotionFloat.Easing FloatEasing => (a, b, t) => {
			// Get the base value so that the pulsation adds on top of it.
			float baseValue = Mathf.Lerp(a, b, t);
			if (t >= 0 && t < 0.5f) {
				return baseValue + MotionKitEasing.QuadInOut(0, Offset.Float, t * 2);
			} else {
				return baseValue + MotionKitEasing.QuadInOut(Offset.Float, 0, (t * 2) - 1);
			}
		};

		public override Motion3D.Easing Vector3Easing => (a, b, t) => {

			// Get the base value so that the pulsation adds on top of it.
			Vector3 baseValue = Vector3.Lerp(a, b, t);

			if (t >= 0 && t< 0.5f) {
				return baseValue + MotionKitEasing.QuadInOut(Vector3.zero, Offset.Vector3, t * 2);
			} else {
				return baseValue + MotionKitEasing.QuadInOut(Offset.Vector3, Vector3.zero, (t * 2) - 1);
			}

		};

		public override MotionColor.Easing ColorEasing => (a, b, t) => {

			// Get the base value so that the pulsation adds on top of it.
			Color baseValue = Color.Lerp(a, b, t);

			if (t >= 0 && t < 0.5f) {
				return baseValue + MotionKitEasing.QuadInOut(Color.black, Offset.Color, t * 2);
			} else {
				return baseValue + MotionKitEasing.QuadInOut(Offset.Color, Color.black, (t * 2) - 1);
			}

		};

		#endregion


		#region Public Constructors

		public Pulse() { }

		public Pulse(float offsetFloat, Vector3 offsetVector3, Color offsetColor) {
			m_Offset = new AnimatableValue(offsetFloat, offsetVector3, offsetColor);
		}

		public Pulse(float offsetFloat) => Offset.Float = offsetFloat;

		public Pulse(Vector3 offsetVector3) => Offset.Vector3 = offsetVector3;

		public Pulse(Color offsetColor) => Offset.Color = offsetColor;

		#endregion


		#region Public Methods

		public override ParameterizedEasing Copy() => new Pulse { m_Offset = m_Offset.Copy() };

		#endregion


		#region Private Fields

		[Tooltip(
			"The maximum value that will be added on top of the current value of the " +
			"property when the pulsation reachs its peak"
		)]
		[SerializeField]
		private AnimatableValue m_Offset;

		#endregion


	}

}