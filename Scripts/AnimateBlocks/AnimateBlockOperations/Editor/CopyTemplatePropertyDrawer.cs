namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(CopyTemplateBlock))]
	public class CopyTemplatePropertyDrawer : AnimateBatchOperationPropertyDrawer {


		#region Protected Methods

		protected override void Edit_InitializePropertiesForGetHeight() {
			base.Edit_InitializePropertiesForGetHeight();
			TemplateBlockProperty = Property.FindPropertyRelative("m_TemplateBlock");
		}

		protected override float Edit_GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var height = base.Edit_GetPropertyHeight(property, label);
			height += EditorGUI.GetPropertyHeight(TemplateBlockProperty);
			return height;
		}

		protected override void Edit_OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			base.Edit_OnGUI(position, property, label);
			EditorGUI.PropertyField(GetNextPosition(TemplateBlockProperty), TemplateBlockProperty);
		}

		#endregion


		#region Private Properties

		private SerializedProperty TemplateBlockProperty { get; set; }

		#endregion


	}

}