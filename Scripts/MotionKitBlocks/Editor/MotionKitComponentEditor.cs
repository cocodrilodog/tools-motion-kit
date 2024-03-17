namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(MotionKitComponent))]
	public class MotionKitComponentEditor : CompositeRootEditor {


		#region Unity Methods

		protected override void OnEnable() {
			base.OnEnable();
			m_BlocksProperty = serializedObject.FindProperty("m_Blocks");
		}

		#endregion


		#region Protected Methods

		protected override void OnRootInspectorGUI() {

			serializedObject.Update();
			
			CDEditorUtility.DrawDisabledField(ScriptProperty);

			foreach(var block in (target as MotionKitComponent).GetChildren()) {
				if (block != null) {
					block.DrawToggles = true;
				}
			}

			BlocksPropertyDrawer.DoLayoutList(m_BlocksProperty);

			EditorGUILayout.HelpBox(
				"Enable or disable PlayOnStart and SetInitialValuesOnStart on each block by ticking the corresponding toggles.", 
				MessageType.Info);

			serializedObject.ApplyModifiedProperties();

		}

		#endregion


		#region Private Fields

		private SerializedProperty m_BlocksProperty;

		private CompositeListPropertyDrawerForPrefab m_BlocksPropertyDrawer;

		#endregion


		#region Private Properties

		private CompositeListPropertyDrawerForPrefab BlocksPropertyDrawer => 
			m_BlocksPropertyDrawer = m_BlocksPropertyDrawer ?? new CompositeListPropertyDrawerForPrefab(m_BlocksProperty);

		#endregion


	}

}