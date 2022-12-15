namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class MotionColorAsset : MotionBaseAsset<Color, MotionColor> {


		#region Protected Methods

		protected override MotionColor CreateMotion(Action<Color> setterDelegate) {

			var motion = Animate.GetMotion(this, ReuseID, v => setterDelegate(v))
				.SetEasing(Easing.ColorEasing)
				.SetValuesAndDuration(InitialValue, FinalValue, Duration);

			return motion;

		}

		#endregion


	}

}