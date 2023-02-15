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
		}

		protected override float Edit_GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var height = base.Edit_GetPropertyHeight(property, label);
			height += SpaceHeight;
			height += EditorGUI.GetPropertyHeight(ParallelItemsProperty);
			return height;
		}

		protected override void DrawAfterSettings() {
			GetNextPosition(SpaceHeight);
			EditorGUI.PropertyField(GetNextPosition(ParallelItemsProperty), ParallelItemsProperty, true);
		}

		#endregion


		#region Private Properties

		private SerializedProperty ParallelItemsProperty { get; set; }

		#endregion


	}

}