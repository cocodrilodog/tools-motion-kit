namespace CocodriloDog.MotionKit {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// This is used by <see cref="ParameterizedEasing"/> objects that have value-related parameters.
	/// </summary>
	[Serializable]
	public class AnimatableValue {


		#region Public Fields

		/// <summary>
		/// A float to be used by the easing function when working with a MotionFloat.
		/// </summary>
		[Tooltip("A float to be used by the easing function when working with a MotionFloat")]
		[SerializeField]
		public float Float;

		/// <summary>
		/// A Vector3 to be used by the easing function when working with a Motion3D.
		/// </summary>
		[Tooltip("A Vector3 to be used by the easing function when working with a Motion3D")]
		[SerializeField]
		public Vector3 Vector3;

		/// <summary>
		/// A color to be used by the easing function when working with a MotionColor.
		/// </summary>
		[Tooltip("A color to be used by the easing function when working with a MotionColor")]
		[SerializeField]
		public Color Color;

		#endregion


		#region Public Constructors

		public AnimatableValue() { }

		public AnimatableValue(float _float, Vector3 vector3, Color color) {
			Float = _float;
			Vector3 = vector3;
			Color = color;
		}

		public AnimatableValue(float _float) => Float = _float;

		public AnimatableValue(Vector3 vector3) => Vector3 = vector3;

		public AnimatableValue(Color color) => Color = color;

		#endregion


		#region Public Methods

		public AnimatableValue Copy() => new AnimatableValue(Float, Vector3, Color);

		#endregion


	}

}