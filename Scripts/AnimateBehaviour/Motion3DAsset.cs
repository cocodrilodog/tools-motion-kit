namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using UnityEngine;
	using UnityEngine.Events;

	public class Motion3DAsset : MotionAsset<Vector3, Motion3D> {


		#region Protected Methods

		protected override Motion3D CreateMotion(Action<Vector3> setterDelegate) {;

			var motion3D = Animate.GetMotion(this, ReuseID, v => setterDelegate(v))
				.SetEasing(Easing.Vector3Easing)
				.Play(InitialValue, FinalValue, Duration);

			return motion3D;
		}

		#endregion


	}

}