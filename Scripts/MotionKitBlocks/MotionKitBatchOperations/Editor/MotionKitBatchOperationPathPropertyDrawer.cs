namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(MotionKitBatchOperationPath))]
	public class MotionKitBatchOperationPathPropertyDrawer : MotionKitBatchOperationPropertyDrawer {


		#region Protected Methods

		protected override void Edit_InitializePropertiesForGetHeight() {
			base.Edit_InitializePropertiesForGetHeight();
			PathProperty = Property.FindPropertyRelative("m_Path");
		}

		protected override float Edit_GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var height = base.Edit_GetPropertyHeight(property, label);
			height += EditorGUI.GetPropertyHeight(PathProperty);
			return height;
		}

		protected override void Edit_OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			base.Edit_OnGUI(position, property, label);
			EditorGUI.PropertyField(GetNextPosition(PathProperty), PathProperty);
		}

		#endregion


		#region Private Properties

		private SerializedProperty PathProperty { get; set; }

		#endregion


	}

}