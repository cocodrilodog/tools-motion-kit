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
			ObjectProperty = serializedObject.FindProperty("m_Object");
			SetterStringProperty = serializedObject.FindProperty("m_SetterString");
			InitialValueProperty = serializedObject.FindProperty("m_InitialValue");
			FinalValueProperty = serializedObject.FindProperty("m_FinalValue");
			DurationProperty = serializedObject.FindProperty("m_Duration");
			EasingProperty = serializedObject.FindProperty("m_Easing");
			OnCompleteProperty = serializedObject.FindProperty("m_OnComplete");
		}

		public override void OnInspectorGUI() {

			base.OnInspectorGUI();			
			serializedObject.Update();

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Setter", EditorStyles.boldLabel);
			DrawObjectAndSetter();

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(InitialValueProperty);
			EditorGUILayout.PropertyField(FinalValueProperty);
			EditorGUILayout.PropertyField(DurationProperty);
			EditorGUILayout.PropertyField(EasingProperty);

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Callbacks", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(OnCompleteProperty);

			serializedObject.ApplyModifiedProperties();

		}

		#endregion


		#region Private Properties

		private SerializedProperty ObjectProperty { get; set; }

		private SerializedProperty SetterStringProperty { get; set; }

		private SerializedProperty InitialValueProperty { get; set; }

		private SerializedProperty FinalValueProperty { get; set; }

		private SerializedProperty DurationProperty { get; set; }

		private SerializedProperty EasingProperty { get; set; }

		private SerializedProperty OnCompleteProperty { get; set; }

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

			var options = new List<string>();
			options.Add("No Function");

			if (ObjectProperty.objectReferenceValue != null) {
				if (ObjectProperty.objectReferenceValue is GameObject) {

					var gameObject = ObjectProperty.objectReferenceValue as GameObject;
					var components = gameObject.GetComponents(typeof(Component));

					foreach(var component in components) {

						var setters = GetMethodsBySignature(component.GetType(), typeof(void), typeof(Vector3));
						foreach(var setter in setters) {
							options.Add($"{component.GetType().Name}/{setter.Name}");
						}

						var properties = GetPropertiesByType(component.GetType(), typeof(Vector3));
						foreach(var property in properties) {
							options.Add($"{component.GetType().Name}/{property.Name}");
						}

					}

				} // TODO: Implement for ScriptableObjects
			}
			return options;
		}

		private MethodInfo[] GetMethodsBySignature(Type ownerType, Type returnType, Type parameterType) {
			return ownerType.GetMethods().Where((m) => {
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

		private PropertyInfo[] GetPropertiesByType(Type ownerType, Type propertyType) {
			return ownerType.GetProperties().Where((p) => propertyType == p.PropertyType).ToArray();
		}

		#endregion


	}

}