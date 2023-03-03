namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(RenameBlock))]
	public class RenamePropertyDrawer : MotionKitBatchOperationPathPropertyDrawer {


		#region Protected Methods

		protected override void Edit_InitializePropertiesForGetHeight() {
			base.Edit_InitializePropertiesForGetHeight();
			NewNameProperty = Property.FindPropertyRelative("m_NewName");
			AppendSerialNumberProperty = Property.FindPropertyRelative("m_AppendSerialNumber");
		}

		protected override float Edit_GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var height = base.Edit_GetPropertyHeight(property, label);
			height += EditorGUI.GetPropertyHeight(NewNameProperty);
			height += EditorGUI.GetPropertyHeight(AppendSerialNumberProperty);
			return height;
		}

		protected override void Edit_OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			base.Edit_OnGUI(position, property, label);
			EditorGUI.PropertyField(GetNextPosition(NewNameProperty), NewNameProperty);
			EditorGUI.PropertyField(GetNextPosition(AppendSerialNumberProperty), AppendSerialNumberProperty);
		}

		#endregion


		#region Private Properties

		private SerializedProperty NewNameProperty { get; set; }

		private SerializedProperty AppendSerialNumberProperty { get; set; }

		#endregion


	}

}