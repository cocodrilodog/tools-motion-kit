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
				.SetValuesAndDuration(InitialValue, FinalValue, Duration)
				.SetTimeMode(TimeMode);

			// This approach will only work if the listeners are added via editor
			if (OnStart.GetPersistentEventCount() > 0) motion.SetOnStart(OnStart.Invoke);
			if (OnUpdate.GetPersistentEventCount() > 0) motion.SetOnUpdate(OnUpdate.Invoke);
			if (OnInterrupt.GetPersistentEventCount() > 0) motion.SetOnInterrupt(OnInterrupt.Invoke);
			if (OnComplete.GetPersistentEventCount() > 0) motion.SetOnComplete(OnComplete.Invoke);

			return motion;

		}

		#endregion


	}

}