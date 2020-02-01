namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;


	#region Small Types

	public class EasingsSet {

		public MotionFloat.Easing FloatEasing;

		public Motion3D.Easing Vector3Easing;

		public MotionColor.Easing ColorEasing;

		public EasingsSet(
			MotionFloat.Easing floatEasing,
			Motion3D.Easing vector3Easing,
			MotionColor.Easing colorEasing
		) {
			FloatEasing = floatEasing;
			Vector3Easing = vector3Easing;
			ColorEasing = colorEasing;
		}

	}

	#endregion

	/// <summary>
	/// Use this type to create fields in the inspector that allow to choose either
	/// any <see cref="AnimateEasing"/> function or an <see cref="AnimationCurve"/>
	/// to be used as <c>Easing</c> by <see cref="Animate"/>.
	/// </summary>
	[Serializable]
	public class AnimateEasingField {


		#region Public Static Properties

		public static List<string> EasingNames {
			get {
				if (m_EasingNames == null) {
					m_EasingNames = new List<string>();
					m_EasingNames.Add(AnimationCurveName);
					m_EasingNames.AddRange(AllEasings.Keys);
				}
				return m_EasingNames;
			}
		}

		#endregion


		#region Public Properties

		/// <summary>
		/// Returns a <c>float</c> easing function based on the selected <see cref="m_EasingName"/>
		/// in the inspector.
		/// </summary>
		///
		/// <remarks>
		///	If the selected <see cref="m_EasingName"/> is <c>"AnimationCurve"</c>, it will 
		/// return an easing function based on the provided <see cref="AnimationCurve"/>,
		/// otherwise it will return the selected <see cref="AnimateEasing"/> function.
		/// </remarks>
		public MotionFloat.Easing FloatEasing {
			get {
				if(m_EasingName == AnimationCurveName) {
					return AnimateCurves.FloatEasing(m_AnimationCurve);
				} else {
					return AllEasings[m_EasingName].FloatEasing;
				}
			}
		}


		/// <summary>
		/// Returns a <c>Vector3</c> easing function based on the selected <see cref="m_EasingName"/>
		/// in the inspector.
		/// </summary>
		///
		/// <remarks>
		///	If the selected <see cref="m_EasingName"/> is <c>"AnimationCurve"</c>, it will 
		/// return an easing function based on the provided <see cref="AnimationCurve"/>,
		/// otherwise it will return the selected <see cref="AnimateEasing"/> function.
		/// </remarks>
		public Motion3D.Easing Vector3Easing {
			get {
				if (m_EasingName == AnimationCurveName) {
					return AnimateCurves.Vector3Easing(m_AnimationCurve);
				} else {
					return AllEasings[m_EasingName].Vector3Easing;
				}
			}
		}


		/// <summary>
		/// Returns a <c>Color</c> easing function based on the selected <see cref="m_EasingName"/>
		/// in the inspector.
		/// </summary>
		///
		/// <remarks>
		///	If the selected <see cref="m_EasingName"/> is <c>"AnimationCurve"</c>, it will 
		/// return an easing function based on the provided <see cref="AnimationCurve"/>,
		/// otherwise it will return the selected <see cref="AnimateEasing"/> function.
		/// </remarks>
		public MotionColor.Easing ColorEasing {
			get {
				if (m_EasingName == AnimationCurveName) {
					return AnimateCurves.ColorEasing(m_AnimationCurve);
				} else {
					return AllEasings[m_EasingName].ColorEasing;
				}
			}
		}

		#endregion


		#region Private Static Fields

		[NonSerialized]
		private static List<string> m_EasingNames;

		[NonSerialized]
		private static Dictionary<string, EasingsSet> m_AllEasings;

		#endregion


		#region Private Constants

		private const string AnimationCurveName = "AnimationCurve";

		#endregion


		#region Private Static Properties

		private static Dictionary<string, EasingsSet> AllEasings {
			get {
				if(m_AllEasings == null) {

					m_AllEasings = new Dictionary<string, EasingsSet>();

					m_AllEasings["BackIn"] = new EasingsSet(AnimateEasing.BackIn, AnimateEasing.BackIn, AnimateEasing.BackIn);
					m_AllEasings["BackOut"] = new EasingsSet(AnimateEasing.BackOut, AnimateEasing.BackOut, AnimateEasing.BackOut);
					m_AllEasings["BackInOut"] = new EasingsSet(AnimateEasing.BackInOut, AnimateEasing.BackInOut, AnimateEasing.BackInOut);

					m_AllEasings["BounceIn"] = new EasingsSet(AnimateEasing.BounceIn, AnimateEasing.BounceIn, AnimateEasing.BounceIn);
					m_AllEasings["BounceOut"] = new EasingsSet(AnimateEasing.BounceOut, AnimateEasing.BounceOut, AnimateEasing.BounceOut);
					m_AllEasings["BounceInOut"] = new EasingsSet(AnimateEasing.BounceInOut, AnimateEasing.BounceInOut, AnimateEasing.BounceInOut);

					m_AllEasings["CircIn"] = new EasingsSet(AnimateEasing.CircIn, AnimateEasing.CircIn, AnimateEasing.CircIn);
					m_AllEasings["CircOut"] = new EasingsSet(AnimateEasing.CircOut, AnimateEasing.CircOut, AnimateEasing.CircOut);
					m_AllEasings["CircInOut"] = new EasingsSet(AnimateEasing.CircInOut, AnimateEasing.CircInOut, AnimateEasing.CircInOut);

					m_AllEasings["ElasticIn"] = new EasingsSet(AnimateEasing.ElasticIn, AnimateEasing.ElasticIn, AnimateEasing.ElasticIn);
					m_AllEasings["ElasticOut"] = new EasingsSet(AnimateEasing.ElasticOut, AnimateEasing.ElasticOut, AnimateEasing.ElasticOut);
					m_AllEasings["ElasticInOut"] = new EasingsSet(AnimateEasing.ElasticInOut, AnimateEasing.ElasticInOut, AnimateEasing.ElasticInOut);

					m_AllEasings["ExpoIn"] = new EasingsSet(AnimateEasing.ExpoIn, AnimateEasing.ExpoIn, AnimateEasing.ExpoIn);
					m_AllEasings["ExpoOut"] = new EasingsSet(AnimateEasing.ExpoOut, AnimateEasing.ExpoOut, AnimateEasing.ExpoOut);
					m_AllEasings["ExpoInOut"] = new EasingsSet(AnimateEasing.ExpoInOut, AnimateEasing.ExpoInOut, AnimateEasing.ExpoInOut);

					m_AllEasings["Linear"] = new EasingsSet(AnimateEasing.Linear, AnimateEasing.Linear, AnimateEasing.Linear);

					m_AllEasings["QuadIn"] = new EasingsSet(AnimateEasing.QuadIn, AnimateEasing.QuadIn, AnimateEasing.QuadIn);
					m_AllEasings["QuadOut"] = new EasingsSet(AnimateEasing.QuadOut, AnimateEasing.QuadOut, AnimateEasing.QuadOut);
					m_AllEasings["QuadInOut"] = new EasingsSet(AnimateEasing.QuadInOut, AnimateEasing.QuadInOut, AnimateEasing.QuadInOut);

					m_AllEasings["QuartIn"] = new EasingsSet(AnimateEasing.QuartIn, AnimateEasing.QuartIn, AnimateEasing.QuartIn);
					m_AllEasings["QuartOut"] = new EasingsSet(AnimateEasing.QuartOut, AnimateEasing.QuartOut, AnimateEasing.QuartOut); ;
					m_AllEasings["QuartInOut"] = new EasingsSet(AnimateEasing.QuartInOut, AnimateEasing.QuartInOut, AnimateEasing.QuartInOut);

					m_AllEasings["QuintIn"] = new EasingsSet(AnimateEasing.QuintIn, AnimateEasing.QuintIn, AnimateEasing.QuintIn);
					m_AllEasings["QuintOut"] = new EasingsSet(AnimateEasing.QuintOut, AnimateEasing.QuintOut, AnimateEasing.QuintOut);
					m_AllEasings["QuintInOut"] = new EasingsSet(AnimateEasing.QuintInOut, AnimateEasing.QuintInOut, AnimateEasing.QuintInOut);

					m_AllEasings["SinusIn"] = new EasingsSet(AnimateEasing.SinusIn, AnimateEasing.SinusIn, AnimateEasing.SinusIn);
					m_AllEasings["SinusOut"] = new EasingsSet(AnimateEasing.SinusOut, AnimateEasing.SinusOut, AnimateEasing.SinusOut);
					m_AllEasings["SinusInOut"] = new EasingsSet(AnimateEasing.SinusInOut, AnimateEasing.SinusInOut, AnimateEasing.SinusInOut);

				}
				return m_AllEasings;
			}
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private string m_EasingName = "Linear";

		[SerializeField]
		private AnimationCurve m_AnimationCurve;

		#endregion


	}

}