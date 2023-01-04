namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Asset for <see cref="Color"/> motions.
	/// </summary>
	public class MotionColorAsset : MotionBaseAsset<Color, MotionColor> {


		#region Protected Methods

		protected override MotionColor CreateMotion(Action<Color> setterDelegate, Func<Color> getterDelegate) {

			// Motion, easing and time mode
			var motion = Animate.GetMotion(this, ReuseID, v => setterDelegate(v))
				.SetEasing(Easing.ColorEasing)
				.SetTimeMode(TimeMode);

			// Set values and duration
			motion.SetInitialValue(InitialValueIsRelative ? getterDelegate() + InitialValue : InitialValue);
			motion.SetFinalValue(FinalValueIsRelative ? getterDelegate() + FinalValue : FinalValue);
			motion.SetDuration(DurationInput);

			// Callbacks: This approach will only work if the listeners are added via editor
			if (OnStart.GetPersistentEventCount() > 0) motion.SetOnStart(OnStart.Invoke);
			if (OnUpdate.GetPersistentEventCount() > 0) motion.SetOnUpdate(OnUpdate.Invoke);
			if (OnInterrupt.GetPersistentEventCount() > 0) motion.SetOnInterrupt(OnInterrupt.Invoke);
			if (OnComplete.GetPersistentEventCount() > 0) motion.SetOnComplete(OnComplete.Invoke);

			return motion;

		}

		#endregion


	}

}