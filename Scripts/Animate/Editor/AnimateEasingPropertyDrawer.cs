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
			// I left this condition for consistency with the other property drawers.
			base.GetPropertyHeight(property, label);
			if(Property.isExpanded) {
				return FieldHeight * 3;
			} else {
				return FieldHeight;
			}
		}

		#endregion


		#region Unity Methods

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			base.OnGUI(position, property, label);

			EditorGUI.BeginProperty(Position, Label, Property);
			Property.isExpanded = EditorGUI.Foldout(GetNextPosition(), property.isExpanded, label);

			if(Property.isExpanded) {

				EditorGUI.indentLevel++;

				EditorGUI.PropertyField(GetNextPosition(), UseCustomCurveProperty);

				int easingNameIndex = AnimateEasingField.EasingNames.IndexOf(EasingNameProperty.stringValue);
				easingNameIndex = EditorGUI.Popup(GetNextPosition(), easingNameIndex, AnimateEasingField.EasingNames.ToArray());
				if (easingNameIndex > -1) {
					EasingNameProperty.stringValue = AnimateEasingField.EasingNames[easingNameIndex];
				}

				EditorGUI.indentLevel--;

			}

			EditorGUI.EndProperty();

		}

		#endregion


		#region Protected Methods

		protected override void InitializePropertiesForGetHeight() {
			base.InitializePropertiesForGetHeight();
			UseCustomCurveProperty = Property.FindPropertyRelative("m_UseCustomCurve");
			EasingNameProperty = Property.FindPropertyRelative("m_EasingName");
		}

		#endregion


		#region Private Properties

		private SerializedProperty UseCustomCurveProperty { get; set; }

		private SerializedProperty EasingNameProperty { get; set; }

		#endregion


	}

}