namespace CocodriloDog.MotionKit {
	
	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(GradientAdapter))]
	public class GradientAdapterEditor : Editor {


		#region Unity Methods

		private void OnEnable() {
			m_ScriptProperty = serializedObject.FindProperty("m_Script");
			m_GradientProperty = serializedObject.FindProperty("m_Gradient");
			m_ValueProperty = serializedObject.FindProperty("m_Value");
			Undo.undoRedoPerformed += Undo_undoRedoPerformed;
		}

		public override void OnInspectorGUI() {	
			
			serializedObject.Update();

			CDEditorUtility.DrawDisabledField(m_ScriptProperty);
			EditorGUILayout.PropertyField(m_GradientProperty);

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(m_ValueProperty);
			if (EditorGUI.EndChangeCheck()) {
				ApplyValue();
			}

			serializedObject.ApplyModifiedProperties();

		}

		private void OnDisable() {
			Undo.undoRedoPerformed -= Undo_undoRedoPerformed;
		}

		#endregion


		#region Event Handlers

		private void Undo_undoRedoPerformed() {
			ApplyValue();
		}

		#endregion


		#region Private Properties

		private SerializedProperty m_ScriptProperty;

		private SerializedProperty m_GradientProperty;

		private SerializedProperty m_ValueProperty;

		#endregion


		#region Private Methods

		private void ApplyValue() {
			foreach (var t in targets) {
				(t as GradientAdapter).ApplyValue();
			}
		}

		#endregion


	}

}