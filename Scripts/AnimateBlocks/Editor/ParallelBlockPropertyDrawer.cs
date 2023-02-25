namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(ParallelBlock))]
	public class ParallelBlockPropertyDrawer : AnimateBlockPropertyDrawer {


		#region Protected Methods

		protected override void Edit_InitializePropertiesForGetHeight() {
			base.Edit_InitializePropertiesForGetHeight();
			ParallelItemsProperty = Property.FindPropertyRelative("m_ParallelItems");
			BatchOperationsProperty = Property.FindPropertyRelative("m_BatchOperations");
		}

		protected override float Edit_GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var height = base.Edit_GetPropertyHeight(property, label);
			height += SpaceHeight;
			height += EditorGUI.GetPropertyHeight(ParallelItemsProperty);
			height += EditorGUI.GetPropertyHeight(BatchOperationsProperty);
			return height;
		}

		protected override void DrawAfterSettings() {
			GetNextPosition(SpaceHeight);
			EditorGUI.PropertyField(GetNextPosition(ParallelItemsProperty), ParallelItemsProperty, true);
			EditorGUI.PropertyField(GetNextPosition(BatchOperationsProperty), BatchOperationsProperty, true);
		}

		#endregion


		#region Private Properties

		private SerializedProperty ParallelItemsProperty { get; set; }

		private SerializedProperty BatchOperationsProperty { get; set; }

		#endregion


	}

}