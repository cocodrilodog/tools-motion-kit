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
			switch (EasingNameProperty.stringValue) {
				case AnimateEasingField.AnimateCurveName:
					return FieldHeight * 2;
				default:
					return FieldHeight;
			}
		}

		#endregion


		#region Unity Methods

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			base.OnGUI(position, property, label);

			EditorGUI.BeginProperty(Position, Label, Property);

			int easingNameIndex = AnimateEasingField.EasingNames.IndexOf(EasingNameProperty.stringValue);
			easingNameIndex = EditorGUI.Popup(GetNextPosition(), label.text, easingNameIndex, EasingNamesArray);
			if (easingNameIndex > -1) {
				EasingNameProperty.stringValue = AnimateEasingField.EasingNames[easingNameIndex];
			}

			if (EasingNameProperty.stringValue == AnimateEasingField.AnimateCurveName) {
				EditorGUI.indentLevel++;
				EditorGUI.PropertyField(GetNextPosition(), AnimateCurveProperty);
				EditorGUI.indentLevel--;
			}

			EditorGUI.EndProperty();

		}

		#endregion


		#region Protected Methods

		protected override void InitializePropertiesForGetHeight() {
			base.InitializePropertiesForGetHeight();
			EasingNameProperty = Property.FindPropertyRelative("m_EasingName");
		}

		protected override void InitializePropertiesForOnGUI() {
			base.InitializePropertiesForOnGUI();
			AnimateCurveProperty = Property.FindPropertyRelative("m_AnimateCurve");
		}

		#endregion


		#region Private Fields

		private string[] m_EasingNamesArray;

		#endregion


		#region Private Properties

		private SerializedProperty EasingNameProperty { get; set; }

		private SerializedProperty AnimateCurveProperty { get; set; }

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


	}

}