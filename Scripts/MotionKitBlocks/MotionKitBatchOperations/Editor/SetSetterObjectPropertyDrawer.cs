namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(SetSetterObject))]
	public class SetSetterObjectPropertyDrawer : MotionKitBatchOperationPathPropertyDrawer {


		#region Protected Methods

		protected override void Edit_InitializePropertiesForGetHeight() {
			base.Edit_InitializePropertiesForGetHeight();
			SetterObjectsProperty = Property.FindPropertyRelative("m_SetterObjects");
		}

		protected override float Edit_GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var height = base.Edit_GetPropertyHeight(property, label);
			height += EditorGUI.GetPropertyHeight(SetterObjectsProperty);
			return height;
		}

		protected override void Edit_OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			base.Edit_OnGUI(position, property, label);
			EditorGUI.PropertyField(GetNextPosition(SetterObjectsProperty), SetterObjectsProperty);
		}

		#endregion


		#region Private Properties

		private SerializedProperty SetterObjectsProperty { get; set; }

		#endregion


	}

}