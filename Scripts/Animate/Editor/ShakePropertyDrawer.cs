namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(Shake))]
	public class ShakePropertyDrawer : PropertyDrawerBase {


		#region Public Methods

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			base.GetPropertyHeight(property, label);
			if (Property.isExpanded) {
				float fieldsCount = 4;
				if (IsDamperedProperty.boolValue) {
					fieldsCount++;
				}
				return FieldHeight * fieldsCount;
			} else {
				return FieldHeight;
			}
		}

		#endregion


		#region Unity Methods

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			base.OnGUI(position, property, label);
			Property.isExpanded = EditorGUI.Foldout(GetNextPosition(), Property.isExpanded, label);
			if (Property.isExpanded) {
				EditorGUI.indentLevel++;
				EditorGUI.PropertyField(GetNextPosition(), TMultiplierProperty);
				EditorGUI.PropertyField(GetNextPosition(), MagnitudeProperty);
				EditorGUI.PropertyField(GetNextPosition(), IsDamperedProperty);
				if (IsDamperedProperty.boolValue) {
					EditorGUI.indentLevel++;
					EditorGUI.PropertyField(GetNextPosition(), DamperProperty);
					EditorGUI.indentLevel--;
				}
				EditorGUI.indentLevel--;
			}
		}

		#endregion

		protected override void InitializePropertiesForGetHeight() {
			base.InitializePropertiesForGetHeight();
			IsDamperedProperty = Property.FindPropertyRelative("IsDampered");
		}

		protected override void InitializePropertiesForOnGUI() {
			base.InitializePropertiesForOnGUI();
			TMultiplierProperty = Property.FindPropertyRelative("TMultiplier");
			MagnitudeProperty = Property.FindPropertyRelative("Magnitude");
			IsDamperedProperty = Property.FindPropertyRelative("IsDampered");
			DamperProperty = Property.FindPropertyRelative("m_Damper");
		}


		#region Private Properties

		private SerializedProperty TMultiplierProperty;

		private SerializedProperty MagnitudeProperty;

		private SerializedProperty IsDamperedProperty;

		private SerializedProperty DamperProperty;

		#endregion


	}

}