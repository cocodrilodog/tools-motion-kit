namespace CocodriloDog.MotionKit {
	using CocodriloDog.Core;
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
			if (ShowBatchOperationsResults) {
				height += BatchOperationsResultsHeight;
			}
			return height;
		}

		protected override void DrawAfterSettings() {
			GetNextPosition(SpaceHeight);
			SequenceItemsPropertyDrawer.DoList(GetNextPosition(SequenceItemsProperty), SequenceItemsProperty);
			DrawBatchOperations();
			DrawRunBatchOperationsButton();
		}

		#endregion


		#region Private Fields

		private CompositeListPropertyDrawerForPrefab m_SequenceItemsPropertyDrawer;

		#endregion


		#region Private Properties

		private SerializedProperty SequenceItemsProperty { get; set; }

		private CompositeListPropertyDrawerForPrefab SequenceItemsPropertyDrawer =>
			m_SequenceItemsPropertyDrawer = m_SequenceItemsPropertyDrawer ?? new CompositeListPropertyDrawerForPrefab(
				SequenceItemsProperty
			);

		#endregion


	}

}