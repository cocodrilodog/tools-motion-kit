namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	public abstract class AnimateAssetEditor : MonoScriptableObjectEditor {


		#region Unity Methods

		protected override void OnEnable() {

			base.OnEnable();

			DurationProperty = serializedObject.FindProperty("m_Duration");
			TimeModeProperty = serializedObject.FindProperty("m_TimeMode");
			EasingProperty = serializedObject.FindProperty("m_Easing");

			OnStartProperty = serializedObject.FindProperty("m_OnStart");
			OnUpdateProperty = serializedObject.FindProperty("m_OnUpdate");
			OnInterruptProperty = serializedObject.FindProperty("m_OnInterrupt");
			OnCompleteProperty = serializedObject.FindProperty("m_OnComplete");

		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			serializedObject.Update();
			DrawBeforeSettings();
			DrawSettings();
			DrawCallbacks();
			serializedObject.ApplyModifiedProperties();
		}

		#endregion


		#region Protected Methods

		protected virtual void DrawBeforeSettings() { }

		protected virtual void DrawSettings() {
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(DurationProperty);
			EditorGUILayout.PropertyField(TimeModeProperty);
			EditorGUILayout.PropertyField(EasingProperty);
		}

		#endregion


		#region Private Fields

		private int m_CallbackSelection;

		private string[] m_CallbackOptions = new string[] { "On Start", "On Update", "On Interrupt", "On Complete" };

		#endregion


		#region Private Properties

		private SerializedProperty DurationProperty { get; set; }

		private SerializedProperty TimeModeProperty { get; set; }

		private SerializedProperty EasingProperty { get; set; }

		private SerializedProperty OnStartProperty { get; set; }

		private SerializedProperty OnUpdateProperty { get; set; }

		private SerializedProperty OnInterruptProperty { get; set; }

		private SerializedProperty OnCompleteProperty { get; set; }

		#endregion


		#region Private Methods

		private void DrawCallbacks() {

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Callbacks", EditorStyles.boldLabel);

			m_CallbackSelection = GUILayout.Toolbar(m_CallbackSelection, m_CallbackOptions);

			switch (m_CallbackSelection) {
				case 0: EditorGUILayout.PropertyField(OnStartProperty); break;
				case 1: EditorGUILayout.PropertyField(OnUpdateProperty); break;
				case 2: EditorGUILayout.PropertyField(OnInterruptProperty); break;
				case 3: EditorGUILayout.PropertyField(OnCompleteProperty); break;
			}

		}

		#endregion


	}

}