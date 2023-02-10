namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// A base class for easings that require parameters.
	/// </summary>
	public abstract class ParameterizedEasing {


		#region Public Properties

		public abstract MotionFloat.Easing FloatEasing { get; }

		public abstract Motion3D.Easing Vector3Easing { get; }

		public abstract MotionColor.Easing ColorEasing { get; }

		#endregion


	}

}