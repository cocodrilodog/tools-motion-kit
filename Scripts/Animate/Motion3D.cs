namespace CocodriloDog.Animation {

	using UnityEngine;

	/// <summary>
	/// A motion object that supports <see cref="Vector3"/> animations.
	/// </summary>
	public class Motion3D : MotionBase<Vector3, Motion3D> {

		public Motion3D(MonoBehaviour monoBehaviour, Setter setter) 
			: base(monoBehaviour, setter) { }

		protected override Easing GetDefaultEasing() {
			return Vector3.Lerp;
		}

	}
}