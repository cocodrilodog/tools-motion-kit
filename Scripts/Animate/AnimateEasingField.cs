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

				}
				return m_EasingNames;
			}
		}

		#endregion


		#region Public Properties

		public Motion3D.Easing Vector3Easing {
			get { return Vector3Easings[m_EasingName]; }
		}

		#endregion


		#region Private Static Fields

		[NonSerialized]
		private static List<string> m_EasingNames;

		[NonSerialized]
		private static Dictionary<string, Motion3D.Easing> m_Vector3Easings;

		#endregion


		#region Private Static Properties

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

				}
				return m_Vector3Easings;
			}
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private bool m_UseCustomCurve;

		[SerializeField]
		private string m_EasingName = "Linear";

		#endregion


	}

}