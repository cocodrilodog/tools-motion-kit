namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(Motion3DAsset))]
	public class Motion3DAssetEditor : MonoScriptableObjectEditor {


		#region Unity Methods

		protected override void OnEnable() {
			base.OnEnable();
			OnCompleteProperty = serializedObject.FindProperty("m_OnComplete");
			ObjectProperty = serializedObject.FindProperty("m_Object");
			SetterStringProperty = serializedObject.FindProperty("m_SetterString");
		}

		public override void OnInspectorGUI() {

			base.OnInspectorGUI();			
			serializedObject.Update();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Setter", EditorStyles.boldLabel);
			DrawObjectAndSetter();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Callbacks", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(OnCompleteProperty);

			serializedObject.ApplyModifiedProperties();

		}

		#endregion


		#region Private Properties

		private SerializedProperty OnCompleteProperty { get; set; }

		private SerializedProperty ObjectProperty { get; set; }

		private SerializedProperty SetterStringProperty { get; set; }

		#endregion


		#region Private Methods

		private void DrawObjectAndSetter() {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(
				ObjectProperty, GUIContent.none, 
				GUILayout.Width((EditorGUIUtility.currentViewWidth - 20) * 0.33f)
			);
			var setterOptions = GetSetterOptions();
			var index = Mathf.Clamp(setterOptions.IndexOf(SetterStringProperty.stringValue), 0, int.MaxValue);
			var newIndex = EditorGUILayout.Popup(index, setterOptions.ToArray());
			SetterStringProperty.stringValue = setterOptions[newIndex];
			EditorGUILayout.EndHorizontal();
		}

		private List<string> GetSetterOptions() {
			if (ObjectProperty.objectReferenceValue != null) {
				if (ObjectProperty.objectReferenceValue is GameObject) {

					var gameObject = ObjectProperty.objectReferenceValue as GameObject;
					var options = new List<string>();

					options.Add("No Function");

					var components = gameObject.GetComponents(typeof(Component));
					foreach(var component in components) {

						var setters3D = GetMethodsBySignature(component.GetType(), typeof(void), typeof(Vector3));
						foreach(var setter in setters3D) {
							options.Add($"{component.GetType().Name}/{setter.Name}");
						}

						var setters2D = GetMethodsBySignature(component.GetType(), typeof(void), typeof(Vector2));
						foreach (var setter in setters2D) {
							options.Add($"{component.GetType().Name}/{setter.Name}");
						}

						var properties3D = GetPropertiesByType(component.GetType(), typeof(Vector3));
						foreach(var property in properties3D) {
							options.Add($"{component.GetType().Name}/{property.Name}");
						}

						var properties2D = GetPropertiesByType(component.GetType(), typeof(Vector2));
						foreach (var property in properties2D) {
							options.Add($"{component.GetType().Name}/{property.Name}");
						}

					}

					return options;

				} else if (ObjectProperty.objectReferenceValue is Component) {
					return null;
				}
			}
			return null;
		}

		private MethodInfo[] GetMethodsBySignature(Type type, Type returnType, Type parameterType) {
			return type.GetMethods().Where((m) => {
				if (m.ReturnType != returnType || m.IsSpecialName) {
					return false;
				}
				var parameters = m.GetParameters();
				if (parameters.Length == 1 && parameterType == parameters[0].ParameterType) {
					return true;
				}
				return false;
			}).ToArray();
		}

		private PropertyInfo[] GetPropertiesByType(Type type, Type propertyType) {
			return type.GetProperties().Where((p) => propertyType == p.PropertyType).ToArray();
		}

		#endregion


	}

}