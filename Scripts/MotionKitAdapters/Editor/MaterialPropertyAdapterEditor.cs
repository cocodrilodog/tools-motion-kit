namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(MaterialPropertyAdapter<>), true)]
	public class MaterialPropertyAdapterEditor : Editor {


		#region Unity Methods

		private void OnEnable() {
			m_ScriptProperty = serializedObject.FindProperty("m_Script");
			m_ValueProperty = serializedObject.FindProperty("m_Value");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();
			CDEditorUtility.DrawDisabledField(m_ScriptProperty);
			EditorGUILayout.PropertyField(m_ValueProperty);
			// Updating always handles the cases for undo, revert prefabproperty, etc.
			foreach (var t in targets) {
				(t as IApplicableValue).ApplyValue();
			}
			serializedObject.ApplyModifiedProperties();
		}

		#endregion


		#region Private Fields

		private SerializedProperty m_ScriptProperty;

		private SerializedProperty m_ValueProperty;

		#endregion


	}

}