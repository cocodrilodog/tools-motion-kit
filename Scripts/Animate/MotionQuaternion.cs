namespace CocodriloDog.Animation {

	using UnityEngine;

	/// <summary>
	/// A motion object that supports <see cref="Quaternion"/> animations.
	/// </summary>
	public class MotionQuaternion : MotionBase<Quaternion, MotionQuaternion> {

		public MotionQuaternion(MonoBehaviour monoBehaviour, Setter setter)
			: base(monoBehaviour, setter) { }

		protected override Easing GetDefaultEasing() {
			return Quaternion.Lerp;
		}

	}

}