namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using System;

	[CustomPropertyDrawer(typeof(AnimateEasingField))]
	public class AnimateEasingFieldPropertyDrawer : PropertyDrawerBase {


		#region Public Methods

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			base.GetPropertyHeight(property, label);
			var height = FieldHeight;
			switch (EasingNameProperty.stringValue) {
				case AnimateEasingField.AnimateCurveName:
				case AnimateEasingField.BlinkName:
				case AnimateEasingField.PulseName:
				case AnimateEasingField.ShakeName:	  
					height += EditorGUI.GetPropertyHeight(ParameterizedEasingProperty) + 2; 
					break;
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
			DrawParameterizedEasingProperty<AnimateCurve>(AnimateEasingField.AnimateCurveName);
			DrawParameterizedEasingProperty<Blink>(AnimateEasingField.BlinkName);
			DrawParameterizedEasingProperty<Pulse>(AnimateEasingField.PulseName);
			DrawParameterizedEasingProperty<Shake>(AnimateEasingField.ShakeName);

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
					m_EasingNamesArray = new string[AnimateEasingField.EasingNames.Count];
					AnimateEasingField.EasingNames.CopyTo(m_EasingNamesArray, 0);
				}
				return m_EasingNamesArray;
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