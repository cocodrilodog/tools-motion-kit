namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(CopyBlock))]
	public class CopyBlockPropertyDrawer : AnimateBlockOperationPropertyDrawer {


		#region Protected Methods

		protected override void Edit_InitializePropertiesForGetHeight() {
			base.Edit_InitializePropertiesForGetHeight();
			OriginalProperty = Property.FindPropertyRelative("m_Original");
		}

		protected override float Edit_GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var height = base.Edit_GetPropertyHeight(property, label);
			height += EditorGUI.GetPropertyHeight(OriginalProperty);
			return height;
		}

		protected override void Edit_OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			base.Edit_OnGUI(position, property, label);
			EditorGUI.PropertyField(GetNextPosition(OriginalProperty), OriginalProperty);
		}

		#endregion


		#region Private Properties

		private SerializedProperty OriginalProperty { get; set; }

		#endregion


	}

}