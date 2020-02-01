namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class AnimateEasingField {


		#region Public Static Properties

		public static List<string> EasingNames {
			get {
				if (m_EasingNames == null) {
					m_EasingNames = new List<string>();

					m_EasingNames.Add(AnimationCurveName);

					m_EasingNames.Add("BackIn");
					m_EasingNames.Add("BackOut");
					m_EasingNames.Add("BackInOut");

					m_EasingNames.Add("BounceIn");
					m_EasingNames.Add("BounceOut");
					m_EasingNames.Add("BounceInOut");

					m_EasingNames.Add("CircIn");
					m_EasingNames.Add("CircOut");
					m_EasingNames.Add("CircInOut");

					m_EasingNames.Add("ElasticIn");
					m_EasingNames.Add("ElasticOut");
					m_EasingNames.Add("ElasticInOut");

					m_EasingNames.Add("ExpoIn");
					m_EasingNames.Add("ExpoOut");
					m_EasingNames.Add("ExpoInOut");

					m_EasingNames.Add("Linear");

					m_EasingNames.Add("QuadIn");
					m_EasingNames.Add("QuadOut");
					m_EasingNames.Add("QuadInOut");

					m_EasingNames.Add("QuartIn");
					m_EasingNames.Add("QuartOut");
					m_EasingNames.Add("QuartInOut");

					m_EasingNames.Add("QuintIn");
					m_EasingNames.Add("QuintOut");
					m_EasingNames.Add("QuintInOut");

					m_EasingNames.Add("SinusIn");
					m_EasingNames.Add("SinusOut");
					m_EasingNames.Add("SinusInOut");

				}
				return m_EasingNames;
			}
		}

		#endregion


		#region Public Properties

		public MotionFloat.Easing FloatEasing {
			get {
				if(m_EasingName == AnimationCurveName) {
					return AnimateCurves.FloatEasing(m_AnimationCurve);
				} else {
					return FloatEasings[m_EasingName];
				}
			}
		}

		public Motion3D.Easing Vector3Easing {
			get {
				if (m_EasingName == AnimationCurveName) {
					return AnimateCurves.Vector3Easing(m_AnimationCurve);
				} else {
					return Vector3Easings[m_EasingName];
				}
			}
		}

		public MotionColor.Easing ColorEasing {
			get {
				if (m_EasingName == AnimationCurveName) {
					return AnimateCurves.ColorEasing(m_AnimationCurve);
				} else {
					return ColorEasings[m_EasingName];
				}
			}
		}

		#endregion


		#region Private Static Fields

		[NonSerialized]
		private static List<string> m_EasingNames;

		[NonSerialized]
		private static Dictionary<string, MotionFloat.Easing> m_FloatEasings;

		[NonSerialized]
		private static Dictionary<string, Motion3D.Easing> m_Vector3Easings;

		[NonSerialized]
		private static Dictionary<string, MotionColor.Easing> m_ColorEasings;

		#endregion


		#region Private Constants

		private const string AnimationCurveName = "AnimationCurve";

		#endregion


		#region Private Static Properties

		private static Dictionary<string, MotionFloat.Easing> FloatEasings {
			get {
				if (m_FloatEasings == null) {
					m_FloatEasings = new Dictionary<string, MotionFloat.Easing>();

					m_FloatEasings["BackIn"] = AnimateEasing.BackIn;
					m_FloatEasings["BackOut"] = AnimateEasing.BackOut;
					m_FloatEasings["BackInOut"] = AnimateEasing.BackInOut;

					m_FloatEasings["BounceIn"] = AnimateEasing.BounceIn;
					m_FloatEasings["BounceOut"] = AnimateEasing.BounceOut;
					m_FloatEasings["BounceInOut"] = AnimateEasing.BounceInOut;

					m_FloatEasings["CircIn"] = AnimateEasing.CircIn;
					m_FloatEasings["CircOut"] = AnimateEasing.CircOut;
					m_FloatEasings["CircInOut"] = AnimateEasing.CircInOut;

					m_FloatEasings["ElasticIn"] = AnimateEasing.ElasticIn;
					m_FloatEasings["ElasticOut"] = AnimateEasing.ElasticOut;
					m_FloatEasings["ElasticInOut"] = AnimateEasing.ElasticInOut;

					m_FloatEasings["ExpoIn"] = AnimateEasing.ExpoIn;
					m_FloatEasings["ExpoOut"] = AnimateEasing.ExpoOut;
					m_FloatEasings["ExpoInOut"] = AnimateEasing.ExpoInOut;

					m_FloatEasings["Linear"] = AnimateEasing.Linear;

					m_FloatEasings["QuadIn"] = AnimateEasing.QuadIn;
					m_FloatEasings["QuadOut"] = AnimateEasing.QuadOut;
					m_FloatEasings["QuadInOut"] = AnimateEasing.QuadInOut;

					m_FloatEasings["QuartIn"] = AnimateEasing.QuartIn;
					m_FloatEasings["QuartOut"] = AnimateEasing.QuartOut;
					m_FloatEasings["QuartInOut"] = AnimateEasing.QuartInOut;

					m_FloatEasings["QuintIn"] = AnimateEasing.QuintIn;
					m_FloatEasings["QuintOut"] = AnimateEasing.QuintOut;
					m_FloatEasings["QuintInOut"] = AnimateEasing.QuintInOut;

					m_FloatEasings["SinusIn"] = AnimateEasing.SinusIn;
					m_FloatEasings["SinusOut"] = AnimateEasing.SinusOut;
					m_FloatEasings["SinusInOut"] = AnimateEasing.SinusInOut;

				}
				return m_FloatEasings;
			}
		}

		private static Dictionary<string, Motion3D.Easing> Vector3Easings {
			get {
				if (m_Vector3Easings == null) {
					m_Vector3Easings = new Dictionary<string, Motion3D.Easing>();

					m_Vector3Easings["BackIn"] = AnimateEasing.BackIn;
					m_Vector3Easings["BackOut"] = AnimateEasing.BackOut;
					m_Vector3Easings["BackInOut"] = AnimateEasing.BackInOut;

					m_Vector3Easings["BounceIn"] = AnimateEasing.BounceIn;
					m_Vector3Easings["BounceOut"] = AnimateEasing.BounceOut;
					m_Vector3Easings["BounceInOut"] = AnimateEasing.BounceInOut;

					m_Vector3Easings["CircIn"] = AnimateEasing.CircIn;
					m_Vector3Easings["CircOut"] = AnimateEasing.CircOut;
					m_Vector3Easings["CircInOut"] = AnimateEasing.CircInOut;

					m_Vector3Easings["ElasticIn"] = AnimateEasing.ElasticIn;
					m_Vector3Easings["ElasticOut"] = AnimateEasing.ElasticOut;
					m_Vector3Easings["ElasticInOut"] = AnimateEasing.ElasticInOut;

					m_Vector3Easings["ExpoIn"] = AnimateEasing.ExpoIn;
					m_Vector3Easings["ExpoOut"] = AnimateEasing.ExpoOut;
					m_Vector3Easings["ExpoInOut"] = AnimateEasing.ExpoInOut;

					m_Vector3Easings["Linear"] = AnimateEasing.Linear;

					m_Vector3Easings["QuadIn"] = AnimateEasing.QuadIn;
					m_Vector3Easings["QuadOut"] = AnimateEasing.QuadOut;
					m_Vector3Easings["QuadInOut"] = AnimateEasing.QuadInOut;

					m_Vector3Easings["QuartIn"] = AnimateEasing.QuartIn;
					m_Vector3Easings["QuartOut"] = AnimateEasing.QuartOut;
					m_Vector3Easings["QuartInOut"] = AnimateEasing.QuartInOut;

					m_Vector3Easings["QuintIn"] = AnimateEasing.QuintIn;
					m_Vector3Easings["QuintOut"] = AnimateEasing.QuintOut;
					m_Vector3Easings["QuintInOut"] = AnimateEasing.QuintInOut;

					m_Vector3Easings["SinusIn"] = AnimateEasing.SinusIn;
					m_Vector3Easings["SinusOut"] = AnimateEasing.SinusOut;
					m_Vector3Easings["SinusInOut"] = AnimateEasing.SinusInOut;

				}
				return m_Vector3Easings;
			}
		}

		private static Dictionary<string, MotionColor.Easing> ColorEasings {
			get {
				if (m_ColorEasings == null) {
					m_ColorEasings = new Dictionary<string, MotionColor.Easing>();

					m_ColorEasings["BackIn"] = AnimateEasing.BackIn;
					m_ColorEasings["BackOut"] = AnimateEasing.BackOut;
					m_ColorEasings["BackInOut"] = AnimateEasing.BackInOut;

					m_ColorEasings["BounceIn"] = AnimateEasing.BounceIn;
					m_ColorEasings["BounceOut"] = AnimateEasing.BounceOut;
					m_ColorEasings["BounceInOut"] = AnimateEasing.BounceInOut;

					m_ColorEasings["CircIn"] = AnimateEasing.CircIn;
					m_ColorEasings["CircOut"] = AnimateEasing.CircOut;
					m_ColorEasings["CircInOut"] = AnimateEasing.CircInOut;

					m_ColorEasings["ElasticIn"] = AnimateEasing.ElasticIn;
					m_ColorEasings["ElasticOut"] = AnimateEasing.ElasticOut;
					m_ColorEasings["ElasticInOut"] = AnimateEasing.ElasticInOut;

					m_ColorEasings["ExpoIn"] = AnimateEasing.ExpoIn;
					m_ColorEasings["ExpoOut"] = AnimateEasing.ExpoOut;
					m_ColorEasings["ExpoInOut"] = AnimateEasing.ExpoInOut;

					m_ColorEasings["Linear"] = AnimateEasing.Linear;

					m_ColorEasings["QuadIn"] = AnimateEasing.QuadIn;
					m_ColorEasings["QuadOut"] = AnimateEasing.QuadOut;
					m_ColorEasings["QuadInOut"] = AnimateEasing.QuadInOut;

					m_ColorEasings["QuartIn"] = AnimateEasing.QuartIn;
					m_ColorEasings["QuartOut"] = AnimateEasing.QuartOut;
					m_ColorEasings["QuartInOut"] = AnimateEasing.QuartInOut;

					m_ColorEasings["QuintIn"] = AnimateEasing.QuintIn;
					m_ColorEasings["QuintOut"] = AnimateEasing.QuintOut;
					m_ColorEasings["QuintInOut"] = AnimateEasing.QuintInOut;

					m_ColorEasings["SinusIn"] = AnimateEasing.SinusIn;
					m_ColorEasings["SinusOut"] = AnimateEasing.SinusOut;
					m_ColorEasings["SinusInOut"] = AnimateEasing.SinusInOut;

				}
				return m_ColorEasings;
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