namespace CocodriloDog.MotionKit {

	using UnityEngine;

	/// <summary>
	/// A motion object that supports <see cref="float"/> animations.
	/// </summary>
	public class MotionFloat : MotionBase<float, MotionFloat> {

		public MotionFloat(MonoBehaviour monoBehaviour, Setter setter) 
			: base(monoBehaviour, setter) { }

		protected override Easing GetDefaultEasing() {
			return Mathf.Lerp;
		}

	}

}