namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	/// <summary>
	/// Property drawer that shows or hides the Damper property depending on IsDampered
	/// </summary>
	[CustomPropertyDrawer(typeof(Shake))]
	public class ShakePropertyDrawer : PropertyDrawerBase {


		#region Public Methods

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			base.GetPropertyHeight(property, label);
			if (Property.isExpanded) {
				float height = FieldHeight; // The header
				height += EditorGUI.GetPropertyHeight(MagnitudeProperty);
				height += EditorGUI.GetPropertyHeight(TMultiplierProperty);
				height += EditorGUI.GetPropertyHeight(IsDamperedProperty);
				if (IsDamperedProperty.boolValue) {
					height += EditorGUI.GetPropertyHeight(DamperProperty);
				}
				return height;
			} else {
				return FieldHeight;
			}
		}

		#endregion


		#region Unity Methods

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			base.OnGUI(position, property, label);
			Label = EditorGUI.BeginProperty(position, label, property);
			Property.isExpanded = EditorGUI.Foldout(GetNextPosition(), Property.isExpanded, Label);
			if (Property.isExpanded) {
				EditorGUI.indentLevel++;
				EditorGUI.PropertyField(GetNextPosition(MagnitudeProperty), MagnitudeProperty, true);
				EditorGUI.PropertyField(GetNextPosition(), TMultiplierProperty);
				EditorGUI.PropertyField(GetNextPosition(), IsDamperedProperty);
				if (IsDamperedProperty.boolValue) {
					EditorGUI.indentLevel++;
					EditorGUI.PropertyField(GetNextPosition(), DamperProperty);
					EditorGUI.indentLevel--;
				}
				EditorGUI.indentLevel--;
			}
			EditorGUI.EndProperty();
		}

		#endregion

		protected override void InitializePropertiesForGetHeight() {
			base.InitializePropertiesForGetHeight();
			MagnitudeProperty	= Property.FindPropertyRelative("m_Magnitude");
			TMultiplierProperty = Property.FindPropertyRelative("m_TMultiplier");
			IsDamperedProperty	= Property.FindPropertyRelative("m_IsDampered");
			DamperProperty		= Property.FindPropertyRelative("m_Damper");
		}

		protected override void InitializePropertiesForOnGUI() {
			base.InitializePropertiesForOnGUI();
			//MagnitudeProperty	= Property.FindPropertyRelative("m_Magnitude");
			//TMultiplierProperty = Property.FindPropertyRelative("m_TMultiplier");
			//IsDamperedProperty	= Property.FindPropertyRelative("m_IsDampered");
			//DamperProperty		= Property.FindPropertyRelative("m_Damper");
		}


		#region Private Properties

		private SerializedProperty MagnitudeProperty;

		private SerializedProperty TMultiplierProperty;

		private SerializedProperty IsDamperedProperty;

		private SerializedProperty DamperProperty;

		#endregion


	}

}