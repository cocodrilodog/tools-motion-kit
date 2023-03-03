namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using System;

	[CustomPropertyDrawer(typeof(MotionKitEasingField))]
	public class MotionKitEasingFieldPropertyDrawer : PropertyDrawerBase {


		#region Public Methods

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			base.GetPropertyHeight(property, label);
			var height = FieldHeight;
			if (IsParameterized) {
				// Add the height of the parameterized property.
				height += EditorGUI.GetPropertyHeight(ParameterizedEasingProperty) + 2;
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
			int easingNameIndex = MotionKitEasingField.EasingNames.IndexOf(EasingNameProperty.stringValue);
			easingNameIndex = EditorGUI.Popup(GetNextPosition(), Label, easingNameIndex, guiContents);
			if (easingNameIndex > -1) {
				EasingNameProperty.stringValue = MotionKitEasingField.EasingNames[easingNameIndex];
			}

			// Draw parameterized easing, if any of them is selected
			DrawParameterizedEasingProperty<MotionKitCurve>(MotionKitEasingField.MotionKitCurveName);
			DrawParameterizedEasingProperty<Blink>(MotionKitEasingField.BlinkName);
			DrawParameterizedEasingProperty<Pulse>(MotionKitEasingField.PulseName);
			DrawParameterizedEasingProperty<Shake>(MotionKitEasingField.ShakeName);

			EditorGUI.EndProperty();

		}

		#endregion


		#region Protected Methods

		protected override void InitializePropertiesForGetHeight() {
			base.InitializePropertiesForGetHeight();
			EasingNameProperty			= Property.FindPropertyRelative("m_EasingName");
			ParameterizedEasingProperty = Property.FindPropertyRelative("m_ParameterizedEasing");
		}

		#endregion


		#region Private Fields

		private string[] m_EasingNamesArray;

		#endregion


		#region Private Properties

		private SerializedProperty EasingNameProperty { get; set; }

		private SerializedProperty ParameterizedEasingProperty { get; set; }

		private string[] EasingNamesArray {
			get {
				if (m_EasingNamesArray == null) {
					m_EasingNamesArray = new string[MotionKitEasingField.EasingNames.Count];
					MotionKitEasingField.EasingNames.CopyTo(m_EasingNamesArray, 0);
				}
				return m_EasingNamesArray;
			}
		}

		private bool IsParameterized {
			get {
				switch (EasingNameProperty.stringValue) {
					case MotionKitEasingField.MotionKitCurveName:
					case MotionKitEasingField.BlinkName:
					case MotionKitEasingField.PulseName:
					case MotionKitEasingField.ShakeName:
						return true;
					default:
						return false;
				}
			}
		}

		#endregion


		#region Private Methods

		private void DrawParameterizedEasingProperty<T>(string easingName) where T : new() {
			if (EasingNameProperty.stringValue == easingName) {
				if (!(ParameterizedEasingProperty.managedReferenceValue is T)) {
					ParameterizedEasingProperty.managedReferenceValue = new T();
				}
				EditorGUI.indentLevel++;
				EditorGUI.PropertyField(GetNextPosition(), ParameterizedEasingProperty, new GUIContent(easingName), true);
				EditorGUI.indentLevel--;
			}
		}

		#endregion


	}

}