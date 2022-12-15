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
			
			// TODO: This approach would only work if the listeners are added via editor
			if(OnComplete.GetPersistentEventCount() > 0) {
				motion.SetOnComplete(OnComplete.Invoke);
			}
			
			return motion;

		}

		#endregion


	}

}