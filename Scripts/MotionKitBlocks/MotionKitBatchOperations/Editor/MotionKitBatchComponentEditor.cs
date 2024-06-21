namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(MotionKitBatchComponent))]
	public class MotionKitBatchComponentEditor : CompositeRootEditor {


		#region Unity Methods

		protected override void OnEnable() {
			base.OnEnable();
			m_ScriptProperty = serializedObject.FindProperty("m_Script");
			m_MotionKitComponentsProperty = serializedObject.FindProperty("m_MotionKitComponents");
			m_BatchOperationGroupsProperty = serializedObject.FindProperty("m_BatchOperationGroups.m_List");
		}

		protected override void OnRootInspectorGUI() {
			serializedObject.Update();
			CDEditorUtility.DrawDisabledField(m_ScriptProperty);
			DrawMotionKitComponents();
			DrawBatchOperationGroups();
			DrawAffectedBlockIndicesWarning();
			DrawRunBatchOperationsButton();
			DrawResutls();
			serializedObject.ApplyModifiedProperties();
		}

		#endregion


		#region Private Fields

		private SerializedProperty m_ScriptProperty;

		private SerializedProperty m_MotionKitComponentsProperty;

		private SerializedProperty m_BatchOperationGroupsProperty;

		private bool m_ShowResults;

		private string m_ResultsMessage;

		private List<BatchOperationsGroupResult> m_BatchOperationGroupResults = new List<BatchOperationsGroupResult>();

		#endregion


		#region Private Properties

		private bool HasMotionKitComponents {
			get {
				for (int i = 0; i < m_MotionKitComponentsProperty.arraySize; i++) {
					var motionKitComponent = m_MotionKitComponentsProperty.GetArrayElementAtIndex(i).objectReferenceValue as MotionKitComponent;
					if (motionKitComponent != null) {
						return true;
					}
				}
				return false;
			}
		}

		private bool HasBatchOperationGroups {
			get {
				for (int i = 0; i < m_BatchOperationGroupsProperty.arraySize; i++) {
					var batchOperationGroup = m_BatchOperationGroupsProperty.GetArrayElementAtIndex(i).managedReferenceValue as MotionKitBatchOperationsGroup;
					if (batchOperationGroup != null) {
						return true;
					}
				}
				return false;
			}
		}

		#endregion


		#region Private Methods

		private void DrawMotionKitComponents() {
			EditorGUILayout.PropertyField(m_MotionKitComponentsProperty);
			if (HasBatchOperationGroups && !HasMotionKitComponents) {
				EditorGUILayout.HelpBox("Add MotionKitComponents to run the operations.", MessageType.Warning);
			}
		}

		private void DrawBatchOperationGroups() {
			EditorGUILayout.PropertyField(serializedObject.FindProperty("m_BatchOperationGroups"));
			if (HasMotionKitComponents && !HasBatchOperationGroups) {
				EditorGUILayout.HelpBox("Add batch operations to run on the MotionKitComponents.", MessageType.Warning);
			}
		}

		private void DrawAffectedBlockIndicesWarning() {

			var affectedBlockIndices = new List<int>();
			var repeatedBlockIndices = new List<int>();

			for (int i = 0; i < m_BatchOperationGroupsProperty.arraySize; i++) {
				var boGroupProperty = m_BatchOperationGroupsProperty.GetArrayElementAtIndex(i);
				if (boGroupProperty.managedReferenceValue != null) {

					var affectedBlockIndexProperty = boGroupProperty.FindPropertyRelative("m_AffectedBlockIndex");
					var index = affectedBlockIndexProperty.intValue;

					if (!affectedBlockIndices.Contains(index)) {
						affectedBlockIndices.Add(index);
					} else {
						repeatedBlockIndices.Add(index);
					}

				}
			}

			if (repeatedBlockIndices.Count > 0) {
				var message = $"There is more than one operation group that will affect the MotionKitBlocks at these positions: ";
				foreach (var repeatedIndex in repeatedBlockIndices) {
					message += $" {repeatedIndex}, ";
				}
				message = message.Remove(message.Length - 2);
				message += "\n\nThis may overwrite operations of preceding groups.";
				EditorGUILayout.HelpBox(message, MessageType.Warning);
			}

		}

		private void DrawRunBatchOperationsButton() {
			EditorGUI.BeginDisabledGroup(!HasMotionKitComponents || !HasBatchOperationGroups);
			if (GUILayout.Button("Run Batch Operations")) {

				var motionKitComponents = new List<MotionKitComponent>();
				for (int i = 0; i < m_MotionKitComponentsProperty.arraySize; i++) {
					var motionKitComponent = m_MotionKitComponentsProperty.GetArrayElementAtIndex(i).objectReferenceValue as MotionKitComponent;
					motionKitComponents.Add(motionKitComponent);
				}
				Undo.RecordObjects(motionKitComponents.ToArray(), "Run Batch Operations");

				RunBatchOperations();
				m_ShowResults = true;
				m_ResultsMessage = GetResultsMessage();

			}
			EditorGUI.EndDisabledGroup();
		}

		private void DrawResutls() {
			if (m_ShowResults) {
				GUIStyle resultsStyle = new GUIStyle(EditorStyles.helpBox);
				resultsStyle.richText = true;
				m_ResultsMessage = EditorGUILayout.TextArea(m_ResultsMessage, resultsStyle);
				if (GUILayout.Button("OK")) {
					m_ShowResults = false;
				}
			}
		}

		private void RunBatchOperations() {

			m_BatchOperationGroupResults.Clear();

			// Each operations group
			for (int i = 0; i < m_BatchOperationGroupsProperty.arraySize; i++) {

				var operationsGroupProperty = m_BatchOperationGroupsProperty.GetArrayElementAtIndex(i);
				var operationsGroup = operationsGroupProperty.managedReferenceValue as MotionKitBatchOperationsGroup;
				var operationsGroupBlockIndex = operationsGroupProperty.FindPropertyRelative("m_AffectedBlockIndex").intValue;
				var operationsProperty = operationsGroupProperty.FindPropertyRelative("m_BatchOperations.m_List");
				var operationsGroupResults = new BatchOperationsGroupResult(operationsGroup.Name, operationsGroupBlockIndex);

				// Each operation
				for (int j = 0; j < operationsProperty.arraySize; j++) {

					var operation = operationsProperty.GetArrayElementAtIndex(j).managedReferenceValue as MotionKitBatchOperation;
					var operationResults = new BatchOperationResults(operation.Name);

					// Each MotionKitComponent
					for (int k = 0; k < m_MotionKitComponentsProperty.arraySize; k++) {

						var motionKitComponent = m_MotionKitComponentsProperty.GetArrayElementAtIndex(k).objectReferenceValue as MotionKitComponent;

						if (motionKitComponent != null) {

							var mkcSerializedObject = new SerializedObject(motionKitComponent);
							mkcSerializedObject.Update();

							var block = motionKitComponent.GetBlock(operationsGroupBlockIndex);

							if (operation.Enabled) {
								if (operation.Perform(ref block, k)) {
									operationResults.Successful++;
								}
								// Use the index of the block, not the index of the motion kit component
								motionKitComponent.SetBlock(operationsGroupBlockIndex, block);
								operationResults.Total++;
							}

							mkcSerializedObject.ApplyModifiedProperties();

						}

					}

					operationsGroupResults.BatchOperationResults.Add(operationResults);

				}

				m_BatchOperationGroupResults.Add(operationsGroupResults);

			}
		}

		private string GetResultsMessage() {
			var message = "<b>Sucessful Operations:</b>\n\n";
			foreach (var operationsGroupResults in m_BatchOperationGroupResults) {
				message += $"{operationsGroupResults.GetMessage()}\n";
			}
			return message;
		}

		#endregion


		#region Support Types

		public class BatchOperationsGroupResult {

			public string Name;

			public int BlockIndex;

			public List<BatchOperationResults> BatchOperationResults = new List<BatchOperationResults>();

			public BatchOperationsGroupResult(string name, int blockIndex) {
				Name = name;
				BlockIndex = blockIndex;
			}

			public string GetMessage() {
				var message = $"{Name} - Applied to block at index {BlockIndex}\n";
				foreach (var results in BatchOperationResults) {
					message += $"\t{results.GetMessage()}\n";
				}
				return message;
			}

		}

		public class BatchOperationResults {

			public string Name;

			public int Successful;

			public int Total;

			public BatchOperationResults(string name) {
				Name = name;
			}

			public string GetMessage() {

				var message = "";

				// Highlight in yellow the ones that weren't completely successful
				var someFailed = Successful < Total;
				if (someFailed) {
					message += "<color=yellow>";
				}

				// successful / total message
				message += $"{Name}: {Successful} of {Total}";

				// Close the color tag
				if (someFailed) {
					message += "</color>";
				}

				return message;

			}

		}

		#endregion


	}

}