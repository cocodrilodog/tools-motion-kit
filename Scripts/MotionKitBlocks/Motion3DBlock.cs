namespace CocodriloDog.MotionKit {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Block for <see cref="Vector3"/> motions.
	/// </summary>
	[Serializable]
	public class Motion3DBlock : MotionBaseBlock<Vector3, Motion3D, Motion2DValues> {


		#region Public Methods

		public override string DefaultName => "Motion3D";

		#endregion


		#region Protected Methods

		protected override Motion3D GetMotion(Action<Vector3> setterDelegate, Func<Vector3> getterDelegate) {

			// Motion, easing and time mode
			var motion = MotionKit.GetMotion(Owner, ReuseID, v => setterDelegate(v))
				.SetEasing(Easing.Vector3Easing)
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