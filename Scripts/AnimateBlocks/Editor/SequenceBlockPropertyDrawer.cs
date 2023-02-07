namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(SequenceBlock))]
	public class SequenceBlockPropertyDrawer : AnimateBlockPropertyDrawer {


		#region Protected Methods

		protected override void InitializePropertiesForGetHeight() {
			base.InitializePropertiesForGetHeight();
			SequenceItemsProperty = Property.FindPropertyRelative("m_SequenceItems");
		}

		protected override float GetEditPropertyHeight(SerializedProperty property, GUIContent label) {
			var height = base.GetEditPropertyHeight(property, label);
			height += SpaceHeight;
			height += EditorGUI.GetPropertyHeight(SequenceItemsProperty);
			return height;
		}

		protected override void DrawAfterSettings() {
			GetNextPosition(SpaceHeight);
			EditorGUI.PropertyField(GetNextPosition(SequenceItemsProperty), SequenceItemsProperty);
		}

		#endregion


		#region Private Properties

		private SerializedProperty SequenceItemsProperty { get; set; }

		#endregion


	}

}