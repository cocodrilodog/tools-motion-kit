namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(IncrementalDuration))]
	public class IncrementalDurationPropertyDrawer : AnimateBlockOperationPropertyDrawer {


		#region Protected Methods

		protected override void Edit_InitializePropertiesForGetHeight() {
			base.Edit_InitializePropertiesForGetHeight();
			PathProperty = Property.FindPropertyRelative("m_Path");
			BaseDurationProperty = Property.FindPropertyRelative("m_BaseDuration");
			DurationIncrementProperty = Property.FindPropertyRelative("m_DurationIncrement");
		}

		protected override float Edit_GetPropertyHeight(SerializedProperty property, GUIContent label) {
			var height = base.Edit_GetPropertyHeight(property, label);
			height += EditorGUI.GetPropertyHeight(PathProperty);
			height += EditorGUI.GetPropertyHeight(BaseDurationProperty);
			height += EditorGUI.GetPropertyHeight(DurationIncrementProperty);
			return height;
		}

		protected override void Edit_OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			base.Edit_OnGUI(position, property, label);
			EditorGUI.PropertyField(GetNextPosition(PathProperty), PathProperty);
			EditorGUI.PropertyField(GetNextPosition(BaseDurationProperty), BaseDurationProperty);
			EditorGUI.PropertyField(GetNextPosition(DurationIncrementProperty), DurationIncrementProperty);
		}

		#endregion


		#region Private Properties

		private SerializedProperty PathProperty { get; set; }

		private SerializedProperty BaseDurationProperty { get; set; }

		private SerializedProperty DurationIncrementProperty { get; set; }

		#endregion


	}

}