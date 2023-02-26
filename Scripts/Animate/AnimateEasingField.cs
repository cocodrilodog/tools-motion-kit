namespace CocodriloDog.Animation {

	using System;
	using System.Collections.ObjectModel;
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

		public static ReadOnlyCollection<string> EasingNames {
			get {
				if (m_EasingNames == null) {
					List<string> easingNames = new List<string>();
					easingNames.AddRange(AllEasings.Keys);
					easingNames.Add(AnimateCurveName);
					easingNames.Add(BlinkName);
					easingNames.Add(PulseName);
					easingNames.Add(ShakeName);
					m_EasingNames = new ReadOnlyCollection<string>(easingNames);
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
		public MotionFloat.Easing FloatEasing => IsParameterized ? ParameterizedEasing.FloatEasing : AllEasings[m_EasingName].FloatEasing;


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
		public Motion3D.Easing Vector3Easing => IsParameterized ? ParameterizedEasing.Vector3Easing : AllEasings[m_EasingName].Vector3Easing;


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
		public MotionColor.Easing ColorEasing => IsParameterized ? ParameterizedEasing.ColorEasing : AllEasings[m_EasingName].ColorEasing;

		/// <summary>
		/// Is the selected easing a parameterized one?
		/// </summary>
		public bool IsParameterized {
			get {
				switch (m_EasingName) {
					case AnimateCurveName:
					case BlinkName:
					case PulseName:
					case ShakeName:
						return true;
					default:
						return false;
				}
			}
		}

		#endregion


		#region Public Methods

		public AnimateEasingField Copy() {
			var copy = new AnimateEasingField();
			copy.m_EasingName = m_EasingName;
			copy.m_ParameterizedEasing = m_ParameterizedEasing.Copy();
			return copy;
		}

		#endregion


		#region Private Static Fields

		[NonSerialized]
		private static ReadOnlyCollection<string> m_EasingNames;

		[NonSerialized]
		private static Dictionary<string, EasingsSet> m_AllEasings;

		#endregion


		#region Private Constants

		public const string AnimateCurveName = "Animate Curve";

		public const string BlinkName = "Blink";

		public const string PulseName = "Pulse";

		public const string ShakeName = "Shake";

		#endregion


		#region Private Static Properties

		private static Dictionary<string, EasingsSet> AllEasings {
			get {
				if(m_AllEasings == null) {

					m_AllEasings = new Dictionary<string, EasingsSet>();

					m_AllEasings["Back In"]			= new EasingsSet(AnimateEasing.BackIn, AnimateEasing.BackIn, AnimateEasing.BackIn);
					m_AllEasings["Back Out"]		= new EasingsSet(AnimateEasing.BackOut, AnimateEasing.BackOut, AnimateEasing.BackOut);
					m_AllEasings["Back In Out"]		= new EasingsSet(AnimateEasing.BackInOut, AnimateEasing.BackInOut, AnimateEasing.BackInOut);

					m_AllEasings["Bounce In"]		= new EasingsSet(AnimateEasing.BounceIn, AnimateEasing.BounceIn, AnimateEasing.BounceIn);
					m_AllEasings["Bounce Out"]		= new EasingsSet(AnimateEasing.BounceOut, AnimateEasing.BounceOut, AnimateEasing.BounceOut);
					m_AllEasings["Bounce In Out"]	= new EasingsSet(AnimateEasing.BounceInOut, AnimateEasing.BounceInOut, AnimateEasing.BounceInOut);

					m_AllEasings["Circ In"]			= new EasingsSet(AnimateEasing.CircIn, AnimateEasing.CircIn, AnimateEasing.CircIn);
					m_AllEasings["Circ Out"]		= new EasingsSet(AnimateEasing.CircOut, AnimateEasing.CircOut, AnimateEasing.CircOut);
					m_AllEasings["Circ In Out"]		= new EasingsSet(AnimateEasing.CircInOut, AnimateEasing.CircInOut, AnimateEasing.CircInOut);

					m_AllEasings["Elastic In"]		= new EasingsSet(AnimateEasing.ElasticIn, AnimateEasing.ElasticIn, AnimateEasing.ElasticIn);
					m_AllEasings["Elastic Out"]		= new EasingsSet(AnimateEasing.ElasticOut, AnimateEasing.ElasticOut, AnimateEasing.ElasticOut);
					m_AllEasings["Elastic In Out"]	= new EasingsSet(AnimateEasing.ElasticInOut, AnimateEasing.ElasticInOut, AnimateEasing.ElasticInOut);

					m_AllEasings["Expo In"]			= new EasingsSet(AnimateEasing.ExpoIn, AnimateEasing.ExpoIn, AnimateEasing.ExpoIn);
					m_AllEasings["Expo Out"]		= new EasingsSet(AnimateEasing.ExpoOut, AnimateEasing.ExpoOut, AnimateEasing.ExpoOut);
					m_AllEasings["Expo In Out"]		= new EasingsSet(AnimateEasing.ExpoInOut, AnimateEasing.ExpoInOut, AnimateEasing.ExpoInOut);

					m_AllEasings["Linear"]			= new EasingsSet(AnimateEasing.Linear, AnimateEasing.Linear, AnimateEasing.Linear);

					m_AllEasings["Quad In"]			= new EasingsSet(AnimateEasing.QuadIn, AnimateEasing.QuadIn, AnimateEasing.QuadIn);
					m_AllEasings["Quad Out"]		= new EasingsSet(AnimateEasing.QuadOut, AnimateEasing.QuadOut, AnimateEasing.QuadOut);
					m_AllEasings["Quad In Out"]		= new EasingsSet(AnimateEasing.QuadInOut, AnimateEasing.QuadInOut, AnimateEasing.QuadInOut);

					m_AllEasings["Quart In"]		= new EasingsSet(AnimateEasing.QuartIn, AnimateEasing.QuartIn, AnimateEasing.QuartIn);
					m_AllEasings["Quart Out"]		= new EasingsSet(AnimateEasing.QuartOut, AnimateEasing.QuartOut, AnimateEasing.QuartOut);
					m_AllEasings["Quart In Out"]	= new EasingsSet(AnimateEasing.QuartInOut, AnimateEasing.QuartInOut, AnimateEasing.QuartInOut);

					m_AllEasings["Quint In"]		= new EasingsSet(AnimateEasing.QuintIn, AnimateEasing.QuintIn, AnimateEasing.QuintIn);
					m_AllEasings["Quint Out"]		= new EasingsSet(AnimateEasing.QuintOut, AnimateEasing.QuintOut, AnimateEasing.QuintOut);
					m_AllEasings["Quint In Out"]	= new EasingsSet(AnimateEasing.QuintInOut, AnimateEasing.QuintInOut, AnimateEasing.QuintInOut);

					m_AllEasings["Sinus In"]		= new EasingsSet(AnimateEasing.SinusIn, AnimateEasing.SinusIn, AnimateEasing.SinusIn);
					m_AllEasings["Sinus Out"]		= new EasingsSet(AnimateEasing.SinusOut, AnimateEasing.SinusOut, AnimateEasing.SinusOut);
					m_AllEasings["Sinus In Out"]	= new EasingsSet(AnimateEasing.SinusInOut, AnimateEasing.SinusInOut, AnimateEasing.SinusInOut);

				}
				return m_AllEasings;
			}
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private string m_EasingName = "Linear";

		[SerializeReference]
		private ParameterizedEasing m_ParameterizedEasing;

		#endregion


		#region Private Properties

		/// <summary>
		/// This can be any of these: <see cref="AnimateCurve"/>, <see cref="Blink"/>,
		/// <see cref="Pulse"/> or <see cref="Shake"/>.
		/// </summary>
		private ParameterizedEasing ParameterizedEasing => m_ParameterizedEasing;

		#endregion


	}

}