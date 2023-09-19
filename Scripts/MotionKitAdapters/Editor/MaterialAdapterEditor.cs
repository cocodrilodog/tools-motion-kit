namespace CocodriloDog.MotionKit {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	public class MaterialAdapterEditor : Editor {


		#region Internal Fields

		[NonSerialized]
		private SerializedProperty m_ScriptProperty;

		[NonSerialized]
		private MaterialAdapter[] m_MaterialAdapters;

		#endregion


		#region Internal Properties

		protected MaterialAdapter[] MaterialAdapters {
			get {
				if (m_MaterialAdapters == null) {
					m_MaterialAdapters = new MaterialAdapter[targets.Length];
					for (int i = 0; i < m_MaterialAdapters.Length; i++) {
						m_MaterialAdapters[i] = (MaterialAdapter)targets[i];
					}
				}
				return m_MaterialAdapters;
			}
		}

		private SerializedProperty ScriptProperty {
			get { return m_ScriptProperty = m_ScriptProperty ?? serializedObject.FindProperty("m_Script"); }
		}

		#endregion


		#region Internal Methods

		protected void DrawScript() {
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(ScriptProperty);
			EditorGUI.EndDisabledGroup();
		}

		protected void DrawMaterialHelpBox(bool showObjectInfo) {

			// This is used to show a help box when any material is null
			List<Renderer> renderersWithoutMaterial = GetRenderersWithoutMaterial();

			// Draw the help box
			if (renderersWithoutMaterial != null) { // There are some objects selected without materials, must show the help box.
				if (showObjectInfo) { // This is the case when rendering from the ColorPalette editor
					EditorGUILayout.HelpBox(string.Format(
						"The renderer component in game object {0}" +
						" should have at least one material assigned.",
						renderersWithoutMaterial[0].name
					), MessageType.Error);
				} else if (renderersWithoutMaterial.Count == 1) { // Only one object has no material
					if (Selection.objects.Length == 1) { // The selected color object has no material
						EditorGUILayout.HelpBox(string.Format(
							"The renderer component should have " +
							"at least one material assigned."
						), MessageType.Error);
					} else { // One of the selected color objects doesn't have material
						EditorGUILayout.HelpBox(string.Format(
							"The renderer component in game object {0} should have " +
							"at least one material assigned.",
							renderersWithoutMaterial[0].name
						), MessageType.Error);
					}
				} else {  // More than one object is selected, let's create a good help string

					string gameObjectsWithoutMaterial = "";

					for (int i = 0; i < renderersWithoutMaterial.Count; i++) {

						if (i > 0 && i < renderersWithoutMaterial.Count - 1) {
							gameObjectsWithoutMaterial += ", ";
						}

						if (i == renderersWithoutMaterial.Count - 1) {
							gameObjectsWithoutMaterial += " and ";
						}

						gameObjectsWithoutMaterial += string.Format("\"{0}\"", renderersWithoutMaterial[i].name);
					}

					EditorGUILayout.HelpBox(string.Format(
						"The renderer component in game objects {0} should have " +
						"at least one material assigned.",
						gameObjectsWithoutMaterial
					), MessageType.Error);

				}
			}
		}

		protected List<Renderer> GetRenderersWithoutMaterial() {

			List<Renderer> renderersWithoutMaterial = null;

			for (int i = 0; i < MaterialAdapters.Length; i++) {
				if (MaterialAdapters[i].Renderer.sharedMaterial == null) {
					if (renderersWithoutMaterial == null) {
						renderersWithoutMaterial = new List<Renderer>();
					}
					renderersWithoutMaterial.Add(MaterialAdapters[i].Renderer);
				}
			}

			return renderersWithoutMaterial;
		}

		#endregion


	}

}