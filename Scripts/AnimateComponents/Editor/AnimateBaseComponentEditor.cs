namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Base class for all <see cref="AnimateBaseComponent"/> inspectors.
	/// </summary>
	public abstract class AnimateBaseComponentEditor : MonoCompositeObjectEditor {


		#region Unity Methods

		protected override void OnEnable() {

			base.OnEnable();

			if (target != null) {

				ReuseIDProperty = serializedObject.FindProperty("m_ReuseID");
				DurationProperty = serializedObject.FindProperty("m_Duration");
				TimeModeProperty = serializedObject.FindProperty("m_TimeMode");
				EasingProperty = serializedObject.FindProperty("m_Easing");

				OnStartProperty = serializedObject.FindProperty("m_OnStart");
				OnUpdateProperty = serializedObject.FindProperty("m_OnUpdate");
				OnInterruptProperty = serializedObject.FindProperty("m_OnInterrupt");
				OnCompleteProperty = serializedObject.FindProperty("m_OnComplete");
				CallbackSelectionProperty = serializedObject.FindProperty("m_CallbackSelection");

			}

		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			serializedObject.Update();
			DrawReuseID();
			DrawBeforeSettings();
			DrawSettings();
			DrawAfterSettings();
			DrawCallbacks();
			serializedObject.ApplyModifiedProperties();
		}

		#endregion


		#region Protected Properties

		protected virtual bool WillDrawEasing => true;

		#endregion


		#region Protected Methods

		protected virtual void DrawBeforeSettings() { }

		protected virtual void DrawSettings() {
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(DurationProperty);
			EditorGUILayout.PropertyField(TimeModeProperty);
			if (WillDrawEasing) {
				EditorGUILayout.PropertyField(EasingProperty);
			}
		}

		protected virtual void DrawAfterSettings() { }

		#endregion


		#region Private Fields

		private string[] m_CallbackOptions = new string[] { "On Start", "On Update", "On Interrupt", "On Complete" };

		#endregion


		#region Private Properties

		private SerializedProperty ReuseIDProperty { get; set; }

		private SerializedProperty DurationProperty { get; set; }

		private SerializedProperty TimeModeProperty { get; set; }

		private SerializedProperty EasingProperty { get; set; }

		private SerializedProperty OnStartProperty { get; set; }

		private SerializedProperty OnUpdateProperty { get; set; }

		private SerializedProperty OnInterruptProperty { get; set; }

		private SerializedProperty OnCompleteProperty { get; set; }

		private SerializedProperty CallbackSelectionProperty { get; set; }

		#endregion


		#region Private Methods

		private void DrawReuseID() => EditorGUILayout.PropertyField(ReuseIDProperty);

		private void DrawCallbacks() {

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Callbacks", EditorStyles.boldLabel);

			CallbackSelectionProperty.intValue = GUILayout.Toolbar(CallbackSelectionProperty.intValue, m_CallbackOptions);

			switch (CallbackSelectionProperty.intValue) {
				case 0: EditorGUILayout.PropertyField(OnStartProperty); break;
				case 1: EditorGUILayout.PropertyField(OnUpdateProperty); break;
				case 2: EditorGUILayout.PropertyField(OnInterruptProperty); break;
				case 3: EditorGUILayout.PropertyField(OnCompleteProperty); break;
			}

		}

		#endregion


	}

}