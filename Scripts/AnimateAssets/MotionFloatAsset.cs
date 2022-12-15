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

			// TODO: This approach would only work if the listeners are added via editor
			if (OnComplete.GetPersistentEventCount() > 0) {
				motion.SetOnComplete(OnComplete.Invoke);
			}

			return motion;

		}

		#endregion


	}

}