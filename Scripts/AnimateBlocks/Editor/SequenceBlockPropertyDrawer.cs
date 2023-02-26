namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(SequenceBlock))]
	public class SequenceBlockPropertyDrawer : CompoundBlockPropertyDrawer {


		#region Protected Methods

		protected override void Edit_InitializePropertiesForGetHeight() {
			base.Edit_InitializePropertiesForGetHeight();
			SequenceItemsProperty = Property.FindPropertyRelative("m_SequenceItems");
		}

		protected override float Edit_GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var height = base.Edit_GetPropertyHeight(property, label);
			height += SpaceHeight;
			height += EditorGUI.GetPropertyHeight(SequenceItemsProperty);
			height += EditorGUI.GetPropertyHeight(BatchOperationsProperty);
			height += FieldHeight; // <- Batch Operations button
			return height;
		}

		protected override void DrawAfterSettings() {
			GetNextPosition(SpaceHeight);
			EditorGUI.PropertyField(GetNextPosition(SequenceItemsProperty), SequenceItemsProperty);
			EditorGUI.PropertyField(GetNextPosition(BatchOperationsProperty), BatchOperationsProperty);
			DrawRunBatchOperationsButton();
		}

		#endregion


		#region Private Properties

		private SerializedProperty SequenceItemsProperty { get; set; }

		#endregion


	}

}