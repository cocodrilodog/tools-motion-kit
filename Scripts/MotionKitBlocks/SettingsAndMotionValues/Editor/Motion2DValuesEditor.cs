namespace CocodriloDog.MotionKit {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(Motion2DValues))]
	public class Motion2DValuesEditor : MotionValuesEditor {


		#region Unity Methods

		protected override void OnEnable() {
			base.OnEnable();
			m_InitialValueProperty = serializedObject.FindProperty("m_InitialValue");
			m_FinalValueProperty = serializedObject.FindProperty("m_FinalValue");
		}

		#endregion


		#region Protected Methods

		protected override void DrawInitialValue() {
			m_InitialValueProperty.vector3Value = 
				EditorGUILayout.Vector2Field(m_InitialValueProperty.displayName, m_InitialValueProperty.vector3Value);
		}

		protected override void DrawFinalValue() {
			m_FinalValueProperty.vector3Value =
				EditorGUILayout.Vector2Field(m_FinalValueProperty.displayName, m_FinalValueProperty.vector3Value);
		}

		#endregion


		#region Private Fields

		private SerializedProperty m_InitialValueProperty;

		private SerializedProperty m_FinalValueProperty;

		#endregion


	}

}