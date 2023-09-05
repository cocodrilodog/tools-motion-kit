namespace CocodriloDog.Animation {

	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEditor;
	using UnityEditor.SceneManagement;

	/// <summary>
	/// The custom editor for the <see cref="ColorModifier"/>. Used to handle the
	/// get / set of the <see cref="ColorModifier.Color"/> property.
	/// </summary>
	[CustomEditor(typeof(ColorAdapter))]
	[CanEditMultipleObjects]
	public class ColorAdapterEditor : MaterialAdapterEditor {


		#region InitializeOnLoad Methods

		[InitializeOnLoadMethod]
		private static void InitializeOnLoad() {

			EditorSceneManager.sceneOpened -= EditorSceneManager_SceneOpened;
			EditorSceneManager.sceneOpened += EditorSceneManager_SceneOpened;

			EditorApplication.hierarchyChanged -= EditorApplication_HierarchyWindowChanged;
			EditorApplication.hierarchyChanged += EditorApplication_HierarchyWindowChanged;

			EditorApplication.playModeStateChanged -= EditorApplication_PlayModeStateChanged;
			EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;

			EditorApplication.update -= EditorApplication_Update;
			EditorApplication.update += EditorApplication_Update;

		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Draws the color field.
		/// </summary>
		/// <param name="showObjectInfo">
		/// If <c>true</c> shows the name of the object in the label. If no material, shows
		/// the name of the game object in the help box.
		/// This is used when rendering the inspector from the parent <see cref="ColorSelector"/>
		/// </param>
		public void DrawColorProperties(bool showObjectInfo = false) {

			// Draw the color field
			GUIContent label = null;

			if (showObjectInfo) {
				label = new GUIContent(ColorModifiers[0].name);
			}

			// Evaluate Color and Alpha
			EditorGUI.BeginChangeCheck();
			serializedObject.Update();
			EditorGUILayout.PropertyField(ColorPropertyNameProperty);
			EditorGUILayout.PropertyField(ColorProperty, label);
			EditorGUILayout.PropertyField(AlphaProperty);
			serializedObject.ApplyModifiedProperties();
			if (EditorGUI.EndChangeCheck()) {
				ForEachTargetColorModifier(cm => cm.ApplyColor());
			}

			DrawMaterialHelpBox(showObjectInfo);

		}

		#endregion


		#region Editor Methods

		private void OnEnable() {
			// This handles the case where an object is duplicated in the editor.
			if (ColorModifiers[0] != null) {
				ColorModifiers[0].ApplyColor();
			}
			Undo.undoRedoPerformed += Undo_UndoRedoPerformed;
		}

		private void OnDisable() {
			Undo.undoRedoPerformed -= Undo_UndoRedoPerformed;
		}

		public override void OnInspectorGUI() {
			DrawScript();
			DrawColorProperties();
		}

		#endregion


		#region Internal Static Methods

		private static void EditorSceneManager_SceneOpened(Scene scene, OpenSceneMode mode) {
			ForEachColorModifierInScene(cm => cm.ApplyColor());
		}

		private static void EditorApplication_HierarchyWindowChanged() {
			if (!Application.isPlaying) {
				ForEachColorModifierInScene(cm => cm.ApplyColor());
			}
		}

		private static void EditorApplication_Update() {
			ForEachColorModifierInScene(cm => cm.ApplyColor());
			EditorApplication.update -= EditorApplication_Update;
		}

		private static void EditorApplication_PlayModeStateChanged(PlayModeStateChange obj) {
			switch (obj) {
				case PlayModeStateChange.EnteredEditMode: {
						ForEachColorModifierInScene(cm => cm.ApplyColor());
						break;
					}
			}
		}

		private static void ForEachColorModifierInScene(Action<ColorAdapter> action) {
			foreach (ColorAdapter colorModifier in FindObjectsOfType<ColorAdapter>()) {
				action(colorModifier);
			}
		}

		#endregion


		#region Internal Fields

		[NonSerialized]
		private SerializedProperty m_ColorPropertyNameProperty;

		[NonSerialized]
		private SerializedProperty m_ColorProperty;

		[NonSerialized]
		private SerializedProperty m_AlphaProperty;

		[NonSerialized]
		private ColorAdapter[] m_ColorModifiers;

		#endregion


		#region Internal Properties

		private SerializedProperty ColorPropertyNameProperty {
			get { return m_ColorPropertyNameProperty = m_ColorPropertyNameProperty ?? serializedObject.FindProperty("m_ColorPropertyName"); }
		}

		private SerializedProperty ColorProperty {
			get { return m_ColorProperty = m_ColorProperty ?? serializedObject.FindProperty("m_Color"); }
		}

		private SerializedProperty AlphaProperty {
			get { return m_AlphaProperty = m_AlphaProperty ?? serializedObject.FindProperty("m_Alpha"); }
		}

		private ColorAdapter[] ColorModifiers {
			get {
				if (m_ColorModifiers == null) {
					m_ColorModifiers = new ColorAdapter[targets.Length];
					for (int i = 0; i < m_ColorModifiers.Length; i++) {
						m_ColorModifiers[i] = (ColorAdapter)targets[i];
					}
				}

				return m_ColorModifiers;
			}
		}

		#endregion


		#region Internal Methods

		private void Undo_UndoRedoPerformed() {
			ForEachColorModifierInScene(cm => cm.ApplyColor());
		}

		private void ForEachTargetColorModifier(Action<ColorAdapter> action) {
			foreach (ColorAdapter colorModifier in ColorModifiers) {
				action(colorModifier);
			}
		}

		#endregion


	}
}
