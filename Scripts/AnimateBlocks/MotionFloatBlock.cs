namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Block for <see cref="float"/> motions.
	/// </summary>
	[Serializable]
	public class MotionFloatBlock : MotionBlock<float, MotionFloat, MotionFloatValues> {


		#region Public Methods

		public override string DefaultName => "MotionFloat";

		#endregion


		#region Protected Methods

		protected override MotionFloat GetMotion(Action<float> setterDelegate, Func<float> getterDelegate) {

			// Motion, easing and time mode
			var motion = Animate.GetMotion(Owner, ReuseID, v => setterDelegate(v))
				.SetEasing(Easing.FloatEasing)
				.SetTimeMode(TimeMode);

			// Set values and duration
			motion.SetInitialValue(InitialValueIsRelative ? getterDelegate() + InitialValue : InitialValue);
			motion.SetFinalValue(FinalValueIsRelative ? getterDelegate() + FinalValue : FinalValue);
			motion.SetDuration(DurationInput);

			// Callbacks: This approach will only work if the listeners are added via editor
			motion.SetOnStart(() => {
				if (InitialValueIsRelative || FinalValueIsRelative) {
					if (m_DontResetRelativeValuesOnStart) {
						// Reset the flag
						m_DontResetRelativeValuesOnStart = false;
					} else {
						// By default, reset the motion when we have relative values
						ResetMotion();
					}
				}
				if (OnStart.GetPersistentEventCount() > 0) OnStart.Invoke();
			});
			if (OnUpdate.GetPersistentEventCount() > 0) motion.SetOnUpdate(OnUpdate.Invoke);
			if (OnInterrupt.GetPersistentEventCount() > 0) motion.SetOnInterrupt(OnInterrupt.Invoke);
			if (OnComplete.GetPersistentEventCount() > 0) motion.SetOnComplete(OnComplete.Invoke);

			return motion;

		}

		#endregion


	}

}