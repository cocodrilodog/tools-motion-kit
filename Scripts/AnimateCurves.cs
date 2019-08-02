namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// Asset to design animation curves that can be used with <see cref="Animate"/>.
	/// </summary>
	[CreateAssetMenu(menuName = "Sago/DoodleStories/AnimateCurves")]
	public class AnimateCurves : ScriptableObject {


		#region Public Methods

		/// <summary>
		/// Gets an easing function for floats modified by the <see cref="AnimationCurve"/>
		/// identified by <c>curveKey</c>.
		/// </summary>
		/// <returns>The float easing function.</returns>
		/// <param name="curveKey">Curve key.</param>
		public MotionFloat.Easing EasingFloat(string curveKey) {
			AnimationCurve curve = GetAnimationCurveByKey(curveKey);
			return (a, b, t) => a + (b - a) * curve.Evaluate(t);
		}

		/// <summary>
		/// Gets an easing function for <see cref="Vector3"/> modified by the 
		/// <see cref="AnimationCurve"/> identified by <c>curveKey</c>.
		/// </summary>
		/// <returns>The <see cref="Vector3"/> easing function.</returns>
		/// <param name="curveKey">Curve key.</param>
		public Motion3D.Easing Easing3D(string curveKey) {
			AnimationCurve curve = GetAnimationCurveByKey(curveKey);
			return (a, b, t) => a + (b - a) * curve.Evaluate(t);
		}

		/// <summary>
		/// Gets an easing function for <see cref="Color"/> modified by the 
		/// <see cref="AnimationCurve"/> identified by <c>curveKey</c>.
		/// </summary>
		/// <returns>The <see cref="Color"/> easing function.</returns>
		/// <param name="curveKey">Curve key.</param>
		public MotionColor.Easing EasingColor(string curveKey) {
			AnimationCurve curve = GetAnimationCurveByKey(curveKey);
			return (a, b, t) => a + (b - a) * curve.Evaluate(t);
		}

		#endregion


		#region Internal Fields

		[SerializeField]
		private KeyedAnimation[] m_Animations;

		#endregion


		#region Internal Methods

		private AnimationCurve GetAnimationCurveByKey(string key) {
			AnimationCurve curve = null;
			foreach (KeyedAnimation animation in m_Animations) {
				if (animation.Key == key)
					curve = animation.AnimationCurve;
			}
			return curve;
		}

		#endregion

	}

	/// <summary>
	/// An object that has <see cref="AnimationCurve"/> paired with a key.
	/// </summary>
	[Serializable]
	public class KeyedAnimation {


		#region Constructors

		public KeyedAnimation() { }

		public KeyedAnimation(string key, AnimationCurve curve) {
			m_Key = key;
			m_AnimationCurve = curve;
		}

		#endregion


		#region Public Properties

		public string Key { get { return m_Key; } }

		public AnimationCurve AnimationCurve { get { return m_AnimationCurve; } }

		#endregion


		#region Internal Fields

		[SerializeField]
		private string m_Key;

		[SerializeField]
		private AnimationCurve m_AnimationCurve;

		#endregion


	}

}