namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(MotionKitSettings))]
	public class MotionKitSettingsEditor : Editor {


		#region Public Static Properties

		/// <summary>
		/// Taken from the ScriptableObject icon.
		/// </summary>
		public static Color SharedColor => new Color(121f / 255, 204f / 255, 239f / 255);

		#endregion


		#region Unity Methods

		private void OnEnable() {

			m_ScriptProperty = serializedObject.FindProperty("m_Script");

			m_SerializedProperties.Add(serializedObject.FindProperty("m_Duration"));
			m_SerializedProperties.Add(serializedObject.FindProperty("m_TimeMode"));
			m_SerializedProperties.Add(serializedObject.FindProperty("m_Easing"));

		}

		public override void OnInspectorGUI() {
			
			CDEditorUtility.DrawDisabledField(m_ScriptProperty);

			// Set the shared color
			var color = GUI.contentColor;
			EditorStyles.label.normal.textColor = SharedColor;
			EditorStyles.boldLabel.normal.textColor = SharedColor;

			foreach (var prop in m_SerializedProperties) {
				EditorGUILayout.PropertyField(prop);
			}

			// Reset the color
			EditorStyles.label.normal.textColor = color;
			EditorStyles.boldLabel.normal.textColor = color;

		}

		#endregion


		#region Private Fields

		private SerializedProperty m_ScriptProperty;

		private List<SerializedProperty> m_SerializedProperties = new List<SerializedProperty>();

		#endregion


	}

}