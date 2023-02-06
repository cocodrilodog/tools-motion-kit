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
	/// Base class for all Motion component inspectors.
	/// </summary>
	/// <typeparam name="ValueT"></typeparam>
	public abstract class MotionBaseComponentEditor<ValueT> : AnimateBaseComponentEditor {


		#region Unity Methods

		protected override void OnEnable() {
			base.OnEnable();
			if (target != null) {
				ObjectProperty = serializedObject.FindProperty("m_Object");
				SetterStringProperty = serializedObject.FindProperty("m_SetterString");
				GetterStringProperty = serializedObject.FindProperty("m_GetterString");
				InitialValueProperty = serializedObject.FindProperty("m_InitialValue");
				InitialValueIsRelativeProperty = serializedObject.FindProperty("m_InitialValueIsRelative");
				FinalValueProperty = serializedObject.FindProperty("m_FinalValue");
				FinalValueIsRelativeProperty = serializedObject.FindProperty("m_FinalValueIsRelative");
			}
		}

		#endregion


		#region Protected Properties

		protected SerializedProperty InitialValueProperty { get; set; }

		protected SerializedProperty InitialValueIsRelativeProperty { get; set; }

		protected SerializedProperty FinalValueProperty { get; set; }

		protected SerializedProperty FinalValueIsRelativeProperty { get; set; }

		#endregion


		#region Protected Methods

		protected virtual void DrawInitialValue() {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(InitialValueProperty);
			EditorGUIUtility.labelWidth = 64;
			EditorGUILayout.PropertyField(InitialValueIsRelativeProperty, new GUIContent("Is Relative"), GUILayout.Width(80));
			EditorGUIUtility.labelWidth = 0;
			EditorGUILayout.EndHorizontal();
		}

		protected virtual void DrawFinalValue() {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(FinalValueProperty);
			EditorGUIUtility.labelWidth = 64;
			EditorGUILayout.PropertyField(FinalValueIsRelativeProperty, new GUIContent("Is Relative"), GUILayout.Width(80));
			EditorGUIUtility.labelWidth = 0;
			EditorGUILayout.EndHorizontal();
		}

		protected override void DrawBeforeSettings() {
			DrawObjectAndSetter();
		}

		protected override void DrawSettings() {
			base.DrawSettings();
			DrawInitialValue();
			DrawFinalValue();
			if (InitialValueIsRelativeProperty.boolValue || FinalValueIsRelativeProperty.boolValue) {
				DrawDisabledObjectAndGetter();
			}
		}

		#endregion


		#region Private Fields

		private List<string> m_SetterOptions;

		private List<string> m_GetterOptions;

		#endregion


		#region Private Properties

		private SerializedProperty ObjectProperty { get; set; }

		private SerializedProperty SetterStringProperty { get; set; }

		private SerializedProperty GetterStringProperty { get; set; }

		private List<string> SetterOptions => m_SetterOptions;

		private List<string> GetterOptions => m_GetterOptions;

		#endregion


		#region Private Methods

		private void DrawObjectAndSetter() {

			EditorGUILayout.Space();

			// Title
			CDEditorUtility.DrawHorizontalLine();
			EditorGUILayout.LabelField("Setter", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();

			// Object field
			EditorGUILayout.PropertyField(
				ObjectProperty, GUIContent.none,
				GUILayout.Width((EditorGUIUtility.currentViewWidth - 20) * 0.33f)
			);

			// Setter string field
			//
			// I tried to call UpdateSetterOptions() only on enable and when the object field
			// changes, but it is creating issues with undo and handling the undo event doesn't
			// fix the issue.
			// This may cause an overhead on the inspector processing, but hopefully not too much.
			UpdateSetterOptions(); 
			var index = Mathf.Clamp(SetterOptions.IndexOf(SetterStringProperty.stringValue), 0, int.MaxValue);
			var newIndex = EditorGUILayout.Popup(index, SetterOptions.ToArray());
			SetterStringProperty.stringValue = SetterOptions[newIndex];

			EditorGUILayout.EndHorizontal();

			if (ObjectProperty.objectReferenceValue == null || SetterStringProperty.stringValue == "No Function") {
				EditorGUILayout.HelpBox("For a motion to have effect, an object and a function must be assigned.", MessageType.Error);
			}

			CDEditorUtility.DrawHorizontalLine();

		}

		private void DrawDisabledObjectAndGetter() {

			EditorGUILayout.Space();

			// Title
			CDEditorUtility.DrawHorizontalLine();
			EditorGUILayout.LabelField("Getter", EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();

			// Object field
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(
				ObjectProperty, GUIContent.none,
				GUILayout.Width((EditorGUIUtility.currentViewWidth - 20) * 0.33f)
			);
			EditorGUI.EndDisabledGroup();

			// Getter string field
			UpdateGetterOptions();
			var index = Mathf.Clamp(GetterOptions.IndexOf(GetterStringProperty.stringValue), 0, int.MaxValue);
			var newIndex = EditorGUILayout.Popup(index, GetterOptions.ToArray());
			GetterStringProperty.stringValue = GetterOptions[newIndex];

			EditorGUILayout.EndHorizontal();

			if (ObjectProperty.objectReferenceValue == null || GetterStringProperty.stringValue == "No Function") {
				EditorGUILayout.HelpBox("For a relative initial or final value to work, an object and a function must be assigned.", MessageType.Error);
			}

			CDEditorUtility.DrawHorizontalLine();

		}

		private void UpdateSetterOptions() {

			m_SetterOptions = new List<string>();
			m_SetterOptions.Add("No Function");

			if (ObjectProperty.objectReferenceValue != null) {
				if (ObjectProperty.objectReferenceValue is GameObject) {

					var gameObject = ObjectProperty.objectReferenceValue as GameObject;
					var components = gameObject.GetComponents(typeof(Component));

					foreach (var component in components) {

						var methods = GetMethodsBySignature(component.GetType(), typeof(void), typeof(ValueT));
						foreach (var setter in methods) {
							m_SetterOptions.Add($"{component.GetType().Name}/{setter.Name}");
						}

						var properties = GetPropertiesByType(component.GetType(), typeof(ValueT));
						foreach (var property in properties) {
							m_SetterOptions.Add($"{component.GetType().Name}/{property.Name}");
						}

					}

				} else {
					// TODO: Possibly work with ScriptableObjects (and fields)
				}
			}

		}

		private void UpdateGetterOptions() {

			m_GetterOptions = new List<string>();
			m_GetterOptions.Add("No Function");

			if (ObjectProperty.objectReferenceValue != null) {
				if (ObjectProperty.objectReferenceValue is GameObject) {

					var gameObject = ObjectProperty.objectReferenceValue as GameObject;
					var components = gameObject.GetComponents(typeof(Component));

					foreach (var component in components) {

						var methods = GetMethodsBySignature(component.GetType(), typeof(ValueT));
						foreach (var getter in methods) {
							m_GetterOptions.Add($"{component.GetType().Name}/{getter.Name}");
						}

						var properties = GetPropertiesByType(component.GetType(), typeof(ValueT));
						foreach (var property in properties) {
							m_GetterOptions.Add($"{component.GetType().Name}/{property.Name}");
						}

					}

				} else {
					// TODO: Possibly work with ScriptableObjects (and fields)
				}
			}

		}

		private MethodInfo[] GetMethodsBySignature(Type ownerType, Type returnType, Type parameterType = null) {

			return ownerType.GetMethods().Where((m) => {
				if (m.ReturnType != returnType || m.IsSpecialName) {
					return false;
				}
				
				var parameters = m.GetParameters();
				if (parameterType != null) {
					if (parameters.Length == 1 && parameterType == parameters[0].ParameterType) {
						return true;
					}
				} else {
					if (parameters.Length == 0) {
						return true;
					}
				}

				return false;

			}).ToArray();
		}

		private PropertyInfo[] GetPropertiesByType(Type ownerType, Type propertyType) {
			return ownerType.GetProperties().Where((p) => propertyType == p.PropertyType).ToArray();
		}

		#endregion


	}

}