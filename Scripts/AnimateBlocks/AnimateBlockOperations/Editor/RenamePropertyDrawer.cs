namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(Rename))]
	public class RenamePropertyDrawer : PathBlockOperationPropertyDrawer {


		#region Protected Methods

		protected override void Edit_InitializePropertiesForGetHeight() {
			base.Edit_InitializePropertiesForGetHeight();
			NewNameProperty = Property.FindPropertyRelative("m_NewName");
			AppendIndexProperty = Property.FindPropertyRelative("m_AppendIndex");
		}

		protected override float Edit_GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var height = base.Edit_GetPropertyHeight(property, label);
			height += EditorGUI.GetPropertyHeight(NewNameProperty);
			height += EditorGUI.GetPropertyHeight(AppendIndexProperty);
			return height;
		}

		protected override void Edit_OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			base.Edit_OnGUI(position, property, label);
			EditorGUI.PropertyField(GetNextPosition(NewNameProperty), NewNameProperty);
			EditorGUI.PropertyField(GetNextPosition(AppendIndexProperty), AppendIndexProperty);
		}

		#endregion


		#region Private Properties

		private SerializedProperty NewNameProperty { get; set; }

		private SerializedProperty AppendIndexProperty { get; set; }

		#endregion


	}

}