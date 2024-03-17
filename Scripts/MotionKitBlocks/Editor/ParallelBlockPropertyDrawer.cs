namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(ParallelBlock))]
	public class ParallelBlockPropertyDrawer : CompoundBlockPropertyDrawer {


		#region Protected Methods

		protected override void Edit_InitializePropertiesForGetHeight() {
			base.Edit_InitializePropertiesForGetHeight();
			ParallelItemsProperty = Property.FindPropertyRelative("m_ParallelItems");
		}

		protected override float Edit_GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var height = base.Edit_GetPropertyHeight(property, label);
			height += SpaceHeight;
			height += EditorGUI.GetPropertyHeight(ParallelItemsProperty);
			height += EditorGUI.GetPropertyHeight(BatchOperationsProperty);
			height += FieldHeight; // <- Batch Operations button
			if (ShowBatchOperationsResults) {
				height += BatchOperationsResultsHeight;
			}
			return height;
		}

		protected override void DrawAfterSettings() {
			GetNextPosition(SpaceHeight);
			ParallelItemsPropertyDrawer.DoList(GetNextPosition(ParallelItemsProperty), ParallelItemsProperty);
			DrawBatchOperations();
			DrawRunBatchOperationsButton();
		}

		#endregion


		#region Private Fields

		private CompositeListPropertyDrawerForPrefab m_ParallelItemsPropertyDrawer;

		#endregion


		#region Private Properties

		private SerializedProperty ParallelItemsProperty { get; set; }

		private CompositeListPropertyDrawerForPrefab ParallelItemsPropertyDrawer =>
			m_ParallelItemsPropertyDrawer = m_ParallelItemsPropertyDrawer ?? new CompositeListPropertyDrawerForPrefab(
				ParallelItemsProperty
			);

		#endregion


	}

}