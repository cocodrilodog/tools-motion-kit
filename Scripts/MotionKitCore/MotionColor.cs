namespace CocodriloDog.MotionKit {

	using UnityEngine;

	/// <summary>
	/// A motion object that supports <see cref="Color"/> animations.
	/// </summary>
	public class MotionColor : MotionBase<Color, MotionColor> {

		public MotionColor(MonoBehaviour monoBehaviour, Setter setter) 
			: base(monoBehaviour, setter) { }

		protected override Easing GetDefaultEasing() {
			return Color.Lerp;
		}

	}

}