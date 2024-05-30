namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(MotionValues<>), true)]
	public class MotionValuesEditor : Editor {


		#region Unity Methods

		protected virtual void OnEnable() {
			m_ScriptProperty = serializedObject.FindProperty("m_Script");
			m_InitialValueProperty = serializedObject.FindProperty("m_InitialValue");
			m_InitialValueIsRelativeProperty = serializedObject.FindProperty("m_InitialValueIsRelative");
			m_FinalValueProperty = serializedObject.FindProperty("m_FinalValue");
			m_FinalValueIsRelativeProperty = serializedObject.FindProperty("m_FinalValueIsRelative");
		}

		public override void OnInspectorGUI() {

			serializedObject.Update();

			var valueLabelWidth = EditorGUIUtility.labelWidth - 20;
			var isRelativeLabelWidth = 64;
			var space = 5;

			// Script
			EditorGUIUtility.labelWidth = valueLabelWidth;
			CDEditorUtility.DrawDisabledField(m_ScriptProperty);

			// Set the shared color
			var color = GUI.contentColor;
			EditorStyles.label.normal.textColor = MotionKitSettingsEditor.SharedColor;
			EditorStyles.boldLabel.normal.textColor = MotionKitSettingsEditor.SharedColor;

			// Initial value
			DrawInitialValue(valueLabelWidth, space, isRelativeLabelWidth);

			// Final Value
			DrawFinalValue(valueLabelWidth, space, isRelativeLabelWidth);

			// Reset label width
			EditorGUIUtility.labelWidth = 0;

			// Reset the color
			EditorStyles.label.normal.textColor = color;
			EditorStyles.boldLabel.normal.textColor = color;

			serializedObject.ApplyModifiedProperties();

		}

		#endregion


		#region Protected Methods

		protected virtual void DrawInitialValue() {
			EditorGUILayout.PropertyField(m_InitialValueProperty);
		}

		protected virtual void DrawFinalValue() {
			EditorGUILayout.PropertyField(m_FinalValueProperty);
		}

		#endregion


		#region Private Fields

		private SerializedProperty m_ScriptProperty;

		private SerializedProperty m_InitialValueProperty;

		private SerializedProperty m_InitialValueIsRelativeProperty;

		private SerializedProperty m_FinalValueProperty;

		private SerializedProperty m_FinalValueIsRelativeProperty;

		#endregion


		#region Private Methods

		private void DrawInitialValue(float valueLabelWidth, float space, float isRelativeLabelWidth) {

			GUILayout.BeginHorizontal();
			EditorGUIUtility.labelWidth = valueLabelWidth;
			DrawInitialValue();

			GUILayout.Space(space);
			EditorGUIUtility.labelWidth = isRelativeLabelWidth;
			EditorGUILayout.PropertyField(m_InitialValueIsRelativeProperty, new GUIContent("Is Relative"), GUILayout.Width(85));
			GUILayout.EndHorizontal();

		}

		private void DrawFinalValue(float valueLabelWidth, float space, float isRelativeLabelWidth) {

			GUILayout.BeginHorizontal();
			EditorGUIUtility.labelWidth = valueLabelWidth;
			DrawFinalValue();

			GUILayout.Space(space);
			EditorGUIUtility.labelWidth = isRelativeLabelWidth;
			EditorGUILayout.PropertyField(m_FinalValueIsRelativeProperty, new GUIContent("Is Relative"), GUILayout.Width(85));
			GUILayout.EndHorizontal();

		}

		#endregion


	}

}