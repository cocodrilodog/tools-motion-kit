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

		protected void DrawRunBatchOperationsButton() {
			if (GUI.Button(GetNextPosition(), "Run All Batch Operations")) {
				Undo.RecordObject(Property.serializedObject.targetObject, "Batch operations");
				(Property.managedReferenceValue as CompoundBlock).PerformBatchOperations();
			}
		}

		#endregion


	}

}