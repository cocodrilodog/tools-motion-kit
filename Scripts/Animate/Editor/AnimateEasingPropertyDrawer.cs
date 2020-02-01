namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(AnimateEasingField))]
	public class AnimateEasingPropertyDrawer : PropertyDrawerBase {


		#region Public Methods

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			base.GetPropertyHeight(property, label);
			if (EasingNameProperty.stringValue == "AnimationCurve") {
				return FieldHeight * 2;
			} else {
				return FieldHeight;
			}
		}

		#endregion


		#region Unity Methods

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			base.OnGUI(position, property, label);

			EditorGUI.BeginProperty(Position, Label, Property);

			int easingNameIndex = AnimateEasingField.EasingNames.IndexOf(EasingNameProperty.stringValue);
			easingNameIndex = EditorGUI.Popup(GetNextPosition(), label.text, easingNameIndex, AnimateEasingField.EasingNames.ToArray());
			if (easingNameIndex > -1) {
				EasingNameProperty.stringValue = AnimateEasingField.EasingNames[easingNameIndex];
			}

			if(EasingNameProperty.stringValue == "AnimationCurve") {
				EditorGUI.indentLevel++;
				EditorGUI.PropertyField(GetNextPosition(), AnimationCurveProperty);
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
			AnimationCurveProperty = Property.FindPropertyRelative("m_AnimationCurve");
		}

		#endregion


		#region Private Properties

		private SerializedProperty EasingNameProperty { get; set; }

		private SerializedProperty AnimationCurveProperty { get; set; }

		#endregion


	}

}