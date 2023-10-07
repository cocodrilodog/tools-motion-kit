namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(MotionKitComponent))]
	public class MotionKitComponentEditor : CompositeRootEditor {

		protected override void OnEnable() {
			base.OnEnable();
			m_BlocksProperty = serializedObject.FindProperty("m_Blocks");
			m_PlayOnStartProperty = serializedObject.FindProperty("m_PlayOnStart");
			m_SetInitialValuesOnStartProperty = serializedObject.FindProperty("m_SetInitialValuesOnStart");
		}

		#region Protected Methods

		protected override void OnRootInspectorGUI() {

			serializedObject.Update();
			
			CDEditorUtility.DrawDisabledField(ScriptProperty);

			foreach(var block in (target as MotionKitComponent).GetChildren()) {
				if (block != null) {
					block.DrawToggles = true;
				}
			}

			EditorGUILayout.PropertyField(m_BlocksProperty);
			//EditorGUILayout.PropertyField(m_PlayOnStartProperty);
			//EditorGUILayout.PropertyField(m_SetInitialValuesOnStartProperty);

			EditorGUILayout.HelpBox(
				"Enable or disable PlayOnStart and SetInitialValuesOnStart on each block by ticking the corresponding toggles.", 
				MessageType.Info);

			serializedObject.ApplyModifiedProperties();
		}

		#endregion


		#region Private Fields

		private SerializedProperty m_BlocksProperty;

		private SerializedProperty m_PlayOnStartProperty;

		private SerializedProperty m_SetInitialValuesOnStartProperty;

		#endregion


	}

}