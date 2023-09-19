namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	public abstract class CompoundBlockPropertyDrawer : MotionKitBlockPropertyDrawer {


		#region Protected Properties

		protected SerializedProperty BatchOperationsProperty { get; set; }

		protected bool ShowBatchOperationsResults => BatchOperationsResults.Show;

		protected float BatchOperationsResultsHeight => BatchOperationsResults.Height;

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
					(operationProperty.managedReferenceValue as MotionKitBatchOperation).FieldActionIsPending) {

					var operation = operationProperty.managedReferenceValue as MotionKitBatchOperation;
					Undo.RecordObject(Property.serializedObject.targetObject, $"{ObjectNames.NicifyVariableName(operation.DisplayName)}");

					(Property.managedReferenceValue as CompoundBlock).PerformBatchOperation(i);

				}
			}

		}

		protected void DrawRunBatchOperationsButton() {

			if (GUI.Button(GetNextPosition(), "Run All Batch Operations")) {

				// Set results
				BatchOperationsResults.Show = true;
				BatchOperationsResults.Message = "<b>Sucessful Operations:</b>\n";

				// Perform the operations
				Undo.RecordObject(Property.serializedObject.targetObject, "All batch operations");
				var compoundBlock = (Property.managedReferenceValue as CompoundBlock);
				var successful = compoundBlock.PerformAllBatchOperations();
				
				// Create the rich text results message
				for(int i = 0; i < successful.Length; i++) {

					// Get the operation name
					var operation = compoundBlock.GetBatchOperation(i);

					// Highlight in yellow the ones that weren't completely successful
					var someFailed = successful[i] < compoundBlock.Items.Count;
					if (someFailed) {
						BatchOperationsResults.Message += "<color=yellow>";
					}
					// sycessful / total message
					BatchOperationsResults.Message += $"{operation.Name}: {successful[i]} of {compoundBlock.Items.Count}";
					// Close the color tag
					if (someFailed) {
						BatchOperationsResults.Message += "</color>";
					}

					// Add new line in all except the last line
					if (i < successful.Length - 1) {
						BatchOperationsResults.Message += "\n";
					}

				}

			}

			if (ShowBatchOperationsResults) {

				// Get the position and width with height 0
				var infoRect = GetNextPosition(0f);

				// Copy the style from the help box
				GUIStyle resultsStyle = new GUIStyle(EditorStyles.helpBox);
				resultsStyle.richText = true;

				// Update the rect with the required height
				infoRect = GetNextPosition(resultsStyle.CalcHeight(new GUIContent(BatchOperationsResults.Message), infoRect.width));

				// Draw the text area
				EditorGUI.TextArea(infoRect, BatchOperationsResults.Message, resultsStyle);

				// OK button to hide the info
				var okButtonRect = GetNextPosition();
				if (GUI.Button(okButtonRect, "OK") ||
					// The condition below makes the info text to be closed when the user navigates to other CompositeObject
					(Property.managedReferenceValue != null && !(Property.managedReferenceValue as CompositeObject).Edit)) {
					BatchOperationsResults.Show = false;
				}

				// Update this for the subclasses.
				BatchOperationsResults.Height = infoRect.height + okButtonRect.height;

			}

		}

		#endregion


		#region Private Properties

		BatchOperationsResults BatchOperationsResults => 
			m_BatchOperationsResults = m_BatchOperationsResults ?? new BatchOperationsResults();

		#endregion


		#region Private Fields

		private BatchOperationsResults m_BatchOperationsResults;

		#endregion


	}

	public class BatchOperationsResults {


		#region Public Fields

		public bool Show;

		public float Height;

		public string Message;

		#endregion


	}

}