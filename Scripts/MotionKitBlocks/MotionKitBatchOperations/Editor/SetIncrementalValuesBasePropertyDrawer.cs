namespace CocodriloDog.MotionKit {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	public class SetIncrementalValuesBasePropertyDrawer : MotionKitBatchOperationPathPropertyDrawer {


		#region Protected Methods

		protected override void Edit_InitializePropertiesForGetHeight() {
			base.Edit_InitializePropertiesForGetHeight();
			BaseInitialValueProperty = Property.FindPropertyRelative("m_BaseInitialValue");
			InitialValueIncrementProperty = Property.FindPropertyRelative("m_InitialValueIncrement");
			BaseFinalValueProperty = Property.FindPropertyRelative("m_BaseFinalValue");
			FinalValueIncrementProperty = Property.FindPropertyRelative("m_FinalValueIncrement");
		}

		protected override float Edit_GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var height = base.Edit_GetPropertyHeight(property, label);
			height += EditorGUI.GetPropertyHeight(BaseInitialValueProperty);
			height += EditorGUI.GetPropertyHeight(InitialValueIncrementProperty);
			height += EditorGUI.GetPropertyHeight(BaseFinalValueProperty);
			height += EditorGUI.GetPropertyHeight(FinalValueIncrementProperty);
			height += 8; // 2 extra pixel per property
			return height;
		}

		protected override void Edit_OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			base.Edit_OnGUI(position, property, label);
			EditorGUI.PropertyField(GetNextPosition(BaseInitialValueProperty), BaseInitialValueProperty);
			EditorGUI.PropertyField(GetNextPosition(InitialValueIncrementProperty), InitialValueIncrementProperty);
			EditorGUI.PropertyField(GetNextPosition(BaseFinalValueProperty), BaseFinalValueProperty);
			EditorGUI.PropertyField(GetNextPosition(FinalValueIncrementProperty), FinalValueIncrementProperty);
		}

		#endregion


		#region Private Properties

		private SerializedProperty BaseInitialValueProperty { get; set; }

		private SerializedProperty InitialValueIncrementProperty { get; set; }

		private SerializedProperty BaseFinalValueProperty { get; set; }

		private SerializedProperty FinalValueIncrementProperty { get; set; }

		#endregion


	}

}