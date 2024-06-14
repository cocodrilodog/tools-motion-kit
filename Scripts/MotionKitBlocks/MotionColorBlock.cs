namespace CocodriloDog.MotionKit {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Block for <see cref="Color"/> motions.
	/// </summary>
	[Serializable]
	public class MotionColorBlock : MotionBaseBlock<Color, MotionColor, MotionColorValues> {


		#region Public Methods

		public override string DefaultName => "MotionColor";

		#endregion


		#region Protected Methods

		protected override MotionColor GetMotion(Action<Color> setterDelegate, Func<Color> getterDelegate) {

			// Motion, easing and time mode
			var motion = MotionKit.GetMotion(Owner, ReuseID, v => setterDelegate(v))
				.SetEasing(Easing.ColorEasing)
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
				OnStart_Runtime?.Invoke();
			});

			motion.SetOnUpdate(() => {
				if (OnUpdate.GetPersistentEventCount() > 0) OnUpdate.Invoke();
				OnUpdate_Runtime?.Invoke();
			});

			motion.SetOnInterrupt(() => {
				if (OnInterrupt.GetPersistentEventCount() > 0) OnInterrupt.Invoke();
				OnInterrupt_Runtime?.Invoke();
			});

			motion.SetOnComplete(() => {
				if (OnComplete.GetPersistentEventCount() > 0) OnComplete.Invoke();
				OnComplete_Runtime?.Invoke();
			});

			return motion;

		}

		#endregion


	}

}