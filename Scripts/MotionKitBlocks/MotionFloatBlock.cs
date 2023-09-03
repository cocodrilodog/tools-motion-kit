namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Block for <see cref="float"/> motions.
	/// </summary>
	[Serializable]
	public class MotionFloatBlock : MotionBaseBlock<float, MotionFloat, MotionFloatValues> {


		#region Public Methods

		public override string DefaultName => "MotionFloat";

		#endregion


		#region Protected Methods

		protected override MotionFloat GetMotion(Action<float> setterDelegate, Func<float> getterDelegate) {

			// Motion, easing and time mode
			var motion = MotionKit.GetMotion(Owner, ReuseID, v => setterDelegate(v))
				.SetEasing(Easing.FloatEasing)
				.SetTimeMode(TimeMode);

			// Set values and duration
			motion.SetInitialValue(InitialValueIsRelative ? getterDelegate() + InitialValue : InitialValue);
			motion.SetFinalValue(FinalValueIsRelative ? getterDelegate() + FinalValue : FinalValue);
			motion.SetDuration(DurationInput);

			// Callbacks: This approach will only work if the listeners are added via editor
			// We are setting the callbacks to null when there are none to clean it in case this is a reused playback
			motion.SetOnStart(() => {
				TryResetPlayback(false);    // After a recursive reset on play, reset only this object (not recursively) when the playback starts
				UnlockResetPlayback(false); // After a possible recursive lock when setting initial values, this auto unlocks this object (not recursively)
											// when the lock is not needed anymore
				if (OnStart.GetPersistentEventCount() > 0) OnStart.Invoke();
			});
			motion.SetOnUpdate(OnUpdate.GetPersistentEventCount() > 0 ? OnUpdate.Invoke : null);
			motion.SetOnInterrupt(OnInterrupt.GetPersistentEventCount() > 0 ? OnInterrupt.Invoke : null);
			motion.SetOnComplete(OnComplete.GetPersistentEventCount() > 0 ? OnComplete.Invoke : null);

			return motion;

		}

		#endregion


	}

}