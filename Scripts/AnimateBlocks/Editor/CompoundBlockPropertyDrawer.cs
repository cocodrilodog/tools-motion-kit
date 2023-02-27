namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	public abstract class CompoundBlockPropertyDrawer : AnimateBlockPropertyDrawer {


		#region Protected Properties

		protected SerializedProperty BatchOperationsProperty { get; set; }

		#endregion


		#region Protected Methods

		protected override void Edit_InitializePropertiesForGetHeight() {
			base.Edit_InitializePropertiesForGetHeight();
			BatchOperationsProperty = Property.FindPropertyRelative("m_BatchOperations");
		}

		protected void DrawBatchOperations() {
			
			// Draw the list
			EditorGUI.PropertyField(GetNextPosition(BatchOperationsProperty), BatchOperationsProperty);

			// Check if any operation is pending
			for(int i = 0; i < BatchOperationsProperty.arraySize; i++) {
				var operationProperty = BatchOperationsProperty.GetArrayElementAtIndex(i);
				if (operationProperty.managedReferenceValue != null && 
					(operationProperty.managedReferenceValue as AnimateBatchOperation).FieldActionIsPending) {

					var operation = operationProperty.managedReferenceValue as AnimateBatchOperation;
					Undo.RecordObject(Property.serializedObject.targetObject, $"{ObjectNames.NicifyVariableName(operation.DisplayName)}");

					(Property.managedReferenceValue as CompoundBlock).PerformBatchOperation(i);

				}
			}

		}

		protected void DrawRunBatchOperationsButton() {
			if (GUI.Button(GetNextPosition(), "Run All Batch Operations")) {
				Undo.RecordObject(Property.serializedObject.targetObject, "All batch operations");
				(Property.managedReferenceValue as CompoundBlock).PerformAllBatchOperations();
			}
		}

		#endregion


	}

}