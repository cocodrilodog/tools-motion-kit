namespace CocodriloDog.MotionKit {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class AnimatableValue {


		#region Public Fields

		[SerializeField]
		public float Float;

		[SerializeField]
		public Vector3 Vector3;

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