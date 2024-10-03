namespace CocodriloDog.MotionKit {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public static class MotionKitUtility {


		#region Public Methods

		public static float GetDeltaTime(TimeMode timeMode) {
			switch (timeMode) {
				case TimeMode.Normal: return Time.deltaTime;
				case TimeMode.Unscaled: return Time.unscaledDeltaTime;
				case TimeMode.Smooth: return Time.smoothDeltaTime;
				case TimeMode.Fixed: return Time.fixedDeltaTime;
				case TimeMode.FixedUnscaled: return Time.fixedUnscaledDeltaTime;
				default: return Time.deltaTime;
			}
		}

		public static float GetTime(TimeMode timeMode) {
			switch (timeMode) {
				case TimeMode.Normal: return Time.time;
				case TimeMode.Unscaled: return Time.unscaledTime;
				case TimeMode.Smooth: return Time.time;
				case TimeMode.Fixed: return Time.fixedTime;
				case TimeMode.FixedUnscaled: return Time.fixedUnscaledTime;
				default: return Time.deltaTime;
			}
		}

		#endregion


	}

}