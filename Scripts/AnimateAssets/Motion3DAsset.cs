namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Motion3DAsset : MotionBaseAsset<Vector3, Motion3D> {


		#region Protected Methods

		protected override Motion3D CreateMotion(Action<Vector3> setterDelegate) {;

			var motion = Animate.GetMotion(this, ReuseID, v => setterDelegate(v))
				.SetEasing(Easing.Vector3Easing)
				.SetValuesAndDuration(InitialValue, FinalValue, Duration);

			return motion;

		}

		#endregion


	}

}