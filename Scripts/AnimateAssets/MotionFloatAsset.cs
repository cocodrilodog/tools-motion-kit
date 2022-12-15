namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class MotionFloatAsset : MotionBaseAsset<float, MotionFloat> {


		#region Protected Methods

		protected override MotionFloat CreateMotion(Action<float> setterDelegate) {

			var motion = Animate.GetMotion(this, ReuseID, v => setterDelegate(v))
				.SetEasing(Easing.FloatEasing)
				.SetValuesAndDuration(InitialValue, FinalValue, Duration);

			return motion;

		}

		#endregion


	}

}