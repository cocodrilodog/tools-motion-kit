namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(AnimateEasingField))]
	public class AnimateEasingFieldPropertyDrawer : PropertyDrawerBase {


		#region Public Methods

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			base.GetPropertyHeight(property, label);
			var height = FieldHeight;
			switch (EasingNameProperty.stringValue) {
				case AnimateEasingField.AnimateCurveName: height += EditorGUI.GetPropertyHeight(AnimateCurveProperty) + 2; break;
				case AnimateEasingField.BlinkName:		  height += EditorGUI.GetPropertyHeight(BlinkProperty) + 2; break;
				case AnimateEasingField.PulseName:		  height += EditorGUI.GetPropertyHeight(PulseProperty) + 2; break;
				case AnimateEasingField.ShakeName:		  height += EditorGUI.GetPropertyHeight(ShakeProperty) + 2; break;
			}
			return height;
		}

		#endregion


		#region Unity Methods

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			base.OnGUI(position, property, label);

			Label = EditorGUI.BeginProperty(Position, Label, Property);

			// Create the list
			GUIContent[] guiContents = new GUIContent[EasingNamesArray.Length];
			for(int i = 0; i < EasingNamesArray.Length; i++) {
				guiContents[i] = new GUIContent(EasingNamesArray[i]);
			}

			// Draw the popup
			int easingNameIndex = AnimateEasingField.EasingNames.IndexOf(EasingNameProperty.stringValue);
			easingNameIndex = EditorGUI.Popup(GetNextPosition(), Label, easingNameIndex, guiContents);
			if (easingNameIndex > -1) {
				EasingNameProperty.stringValue = AnimateEasingField.EasingNames[easingNameIndex];
			}

			// Draw parameterized easing, if any of them is selected
			DrawAnimateCurve();
			DrawBlink();
			DrawPulse();
			DrawShake();

			EditorGUI.EndProperty();

		}

		#endregion


		#region Protected Methods

		protected override void InitializePropertiesForGetHeight() {
			base.InitializePropertiesForGetHeight();
			EasingNameProperty		= Property.FindPropertyRelative("m_EasingName");
			AnimateCurveProperty	= Property.FindPropertyRelative("m_AnimateCurve");
			BlinkProperty			= Property.FindPropertyRelative("m_Blink");
			PulseProperty			= Property.FindPropertyRelative("m_Pulse");
			ShakeProperty			= Property.FindPropertyRelative("m_Shake");
		}

		//protected override void InitializePropertiesForOnGUI() {
		//	base.InitializePropertiesForOnGUI();
		//	AnimateCurveProperty = Property.FindPropertyRelative("m_AnimateCurve");
		//}

		#endregion


		#region Private Fields

		private string[] m_EasingNamesArray;

		#endregion


		#region Private Properties

		private SerializedProperty EasingNameProperty { get; set; }

		private SerializedProperty AnimateCurveProperty { get; set; }

		private SerializedProperty BlinkProperty { get; set; }

		private SerializedProperty PulseProperty { get; set; }

		private SerializedProperty ShakeProperty { get; set; }

		private string[] EasingNamesArray {
			get {
				if (m_EasingNamesArray == null) {
					m_EasingNamesArray = new string[AnimateEasingField.EasingNames.Count];
					AnimateEasingField.EasingNames.CopyTo(m_EasingNamesArray, 0);
				}
				return m_EasingNamesArray;
			}
		}

		#endregion


		#region Private Methods

		private void DrawAnimateCurve() {
			if (EasingNameProperty.stringValue == AnimateEasingField.AnimateCurveName) {
				EditorGUI.indentLevel++;
				EditorGUI.PropertyField(GetNextPosition(), AnimateCurveProperty);
				EditorGUI.indentLevel--;
			}
		}

		private void DrawBlink() {
			if (EasingNameProperty.stringValue == AnimateEasingField.BlinkName) {
				EditorGUI.indentLevel++;
				EditorGUI.PropertyField(GetNextPosition(), BlinkProperty, new GUIContent("Blink Parameters"), true);
				EditorGUI.indentLevel--;
			}
		}

		private void DrawPulse() {
			if (EasingNameProperty.stringValue == AnimateEasingField.PulseName) {
				EditorGUI.indentLevel++;
				EditorGUI.PropertyField(GetNextPosition(), PulseProperty, new GUIContent("Pulse Parameters"), true);
				EditorGUI.indentLevel--;
			}
		}

		private void DrawShake() {
			if (EasingNameProperty.stringValue == AnimateEasingField.ShakeName) {
				EditorGUI.indentLevel++;
				EditorGUI.PropertyField(GetNextPosition(), ShakeProperty, new GUIContent("Shake Parameters"), true);
				EditorGUI.indentLevel--;
			}
		}

		#endregion


	}

}