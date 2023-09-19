namespace CocodriloDog.MotionKit {

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
	/// any <see cref="MotionKitEasing"/> function or an <see cref="AnimationCurve"/>
	/// to be used as <c>Easing</c> by <see cref="MotionKit"/>.
	/// </summary>
	[Serializable]
	public class MotionKitEasingField {


		#region Public Static Properties

		public static ReadOnlyCollection<string> EasingNames {
			get {
				if (m_EasingNames == null) {
					List<string> easingNames = new List<string>();
					easingNames.AddRange(AllEasings.Keys);
					easingNames.Add(MotionKitCurveName);
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
		/// otherwise it will return the selected <see cref="MotionKitEasing"/> function.
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
		/// otherwise it will return the selected <see cref="MotionKitEasing"/> function.
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
		/// otherwise it will return the selected <see cref="MotionKitEasing"/> function.
		/// </remarks>
		public MotionColor.Easing ColorEasing => IsParameterized ? ParameterizedEasing.ColorEasing : AllEasings[m_EasingName].ColorEasing;

		/// <summary>
		/// Is the selected easing a parameterized one?
		/// </summary>
		public bool IsParameterized {
			get {
				switch (m_EasingName) {
					case MotionKitCurveName:
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

		public MotionKitEasingField Copy() {
			var copy = new MotionKitEasingField();
			copy.m_EasingName = m_EasingName;
			copy.m_ParameterizedEasing = m_ParameterizedEasing.Copy();
			return copy;
		}

		#endregion


		#region Private Static Fields

		private static ReadOnlyCollection<string> m_EasingNames;

		private static Dictionary<string, EasingsSet> m_AllEasings;

		#endregion


		#region Private Constants

		public const string MotionKitCurveName = "Animation Curve";

		public const string BlinkName = "Blink";

		public const string PulseName = "Pulse";

		public const string ShakeName = "Shake";

		#endregion


		#region Private Static Properties

		private static Dictionary<string, EasingsSet> AllEasings {
			get {
				if(m_AllEasings == null) {

					m_AllEasings = new Dictionary<string, EasingsSet>();

					m_AllEasings["Back In"]			= new EasingsSet(MotionKitEasing.BackIn, MotionKitEasing.BackIn, MotionKitEasing.BackIn);
					m_AllEasings["Back Out"]		= new EasingsSet(MotionKitEasing.BackOut, MotionKitEasing.BackOut, MotionKitEasing.BackOut);
					m_AllEasings["Back In Out"]		= new EasingsSet(MotionKitEasing.BackInOut, MotionKitEasing.BackInOut, MotionKitEasing.BackInOut);

					m_AllEasings["Bounce In"]		= new EasingsSet(MotionKitEasing.BounceIn, MotionKitEasing.BounceIn, MotionKitEasing.BounceIn);
					m_AllEasings["Bounce Out"]		= new EasingsSet(MotionKitEasing.BounceOut, MotionKitEasing.BounceOut, MotionKitEasing.BounceOut);
					m_AllEasings["Bounce In Out"]	= new EasingsSet(MotionKitEasing.BounceInOut, MotionKitEasing.BounceInOut, MotionKitEasing.BounceInOut);

					m_AllEasings["Circ In"]			= new EasingsSet(MotionKitEasing.CircIn, MotionKitEasing.CircIn, MotionKitEasing.CircIn);
					m_AllEasings["Circ Out"]		= new EasingsSet(MotionKitEasing.CircOut, MotionKitEasing.CircOut, MotionKitEasing.CircOut);
					m_AllEasings["Circ In Out"]		= new EasingsSet(MotionKitEasing.CircInOut, MotionKitEasing.CircInOut, MotionKitEasing.CircInOut);

					m_AllEasings["Elastic In"]		= new EasingsSet(MotionKitEasing.ElasticIn, MotionKitEasing.ElasticIn, MotionKitEasing.ElasticIn);
					m_AllEasings["Elastic Out"]		= new EasingsSet(MotionKitEasing.ElasticOut, MotionKitEasing.ElasticOut, MotionKitEasing.ElasticOut);
					m_AllEasings["Elastic In Out"]	= new EasingsSet(MotionKitEasing.ElasticInOut, MotionKitEasing.ElasticInOut, MotionKitEasing.ElasticInOut);

					m_AllEasings["Expo In"]			= new EasingsSet(MotionKitEasing.ExpoIn, MotionKitEasing.ExpoIn, MotionKitEasing.ExpoIn);
					m_AllEasings["Expo Out"]		= new EasingsSet(MotionKitEasing.ExpoOut, MotionKitEasing.ExpoOut, MotionKitEasing.ExpoOut);
					m_AllEasings["Expo In Out"]		= new EasingsSet(MotionKitEasing.ExpoInOut, MotionKitEasing.ExpoInOut, MotionKitEasing.ExpoInOut);

					m_AllEasings["Linear"]			= new EasingsSet(MotionKitEasing.Linear, MotionKitEasing.Linear, MotionKitEasing.Linear);

					m_AllEasings["Quad In"]			= new EasingsSet(MotionKitEasing.QuadIn, MotionKitEasing.QuadIn, MotionKitEasing.QuadIn);
					m_AllEasings["Quad Out"]		= new EasingsSet(MotionKitEasing.QuadOut, MotionKitEasing.QuadOut, MotionKitEasing.QuadOut);
					m_AllEasings["Quad In Out"]		= new EasingsSet(MotionKitEasing.QuadInOut, MotionKitEasing.QuadInOut, MotionKitEasing.QuadInOut);

					m_AllEasings["Quart In"]		= new EasingsSet(MotionKitEasing.QuartIn, MotionKitEasing.QuartIn, MotionKitEasing.QuartIn);
					m_AllEasings["Quart Out"]		= new EasingsSet(MotionKitEasing.QuartOut, MotionKitEasing.QuartOut, MotionKitEasing.QuartOut);
					m_AllEasings["Quart In Out"]	= new EasingsSet(MotionKitEasing.QuartInOut, MotionKitEasing.QuartInOut, MotionKitEasing.QuartInOut);

					m_AllEasings["Quint In"]		= new EasingsSet(MotionKitEasing.QuintIn, MotionKitEasing.QuintIn, MotionKitEasing.QuintIn);
					m_AllEasings["Quint Out"]		= new EasingsSet(MotionKitEasing.QuintOut, MotionKitEasing.QuintOut, MotionKitEasing.QuintOut);
					m_AllEasings["Quint In Out"]	= new EasingsSet(MotionKitEasing.QuintInOut, MotionKitEasing.QuintInOut, MotionKitEasing.QuintInOut);

					m_AllEasings["Sinus In"]		= new EasingsSet(MotionKitEasing.SinusIn, MotionKitEasing.SinusIn, MotionKitEasing.SinusIn);
					m_AllEasings["Sinus Out"]		= new EasingsSet(MotionKitEasing.SinusOut, MotionKitEasing.SinusOut, MotionKitEasing.SinusOut);
					m_AllEasings["Sinus In Out"]	= new EasingsSet(MotionKitEasing.SinusInOut, MotionKitEasing.SinusInOut, MotionKitEasing.SinusInOut);

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
		/// This can be any of these: <see cref="MotionKitCurve"/>, <see cref="Blink"/>,
		/// <see cref="Pulse"/> or <see cref="Shake"/>.
		/// </summary>
		private ParameterizedEasing ParameterizedEasing => m_ParameterizedEasing;

		#endregion


	}

}