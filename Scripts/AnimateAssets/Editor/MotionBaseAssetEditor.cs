namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	public abstract class MotionBaseAssetEditor<ValueT> : MonoScriptableObjectEditor {


		#region Unity Methods

		protected override void OnEnable() {
			base.OnEnable();

			ObjectProperty			= serializedObject.FindProperty("m_Object");
			SetterStringProperty	= serializedObject.FindProperty("m_SetterString");
			InitialValueProperty	= serializedObject.FindProperty("m_InitialValue");
			FinalValueProperty		= serializedObject.FindProperty("m_FinalValue");
			DurationProperty		= serializedObject.FindProperty("m_Duration");
			TimeModeProperty		= serializedObject.FindProperty("m_TimeMode");
			EasingProperty			= serializedObject.FindProperty("m_Easing");

			OnStartProperty			= serializedObject.FindProperty("m_OnStart");
			OnUpdateProperty		= serializedObject.FindProperty("m_OnUpdate");
			OnInterruptProperty		= serializedObject.FindProperty("m_OnInterrupt");
			OnCompleteProperty		= serializedObject.FindProperty("m_OnComplete");

		}

		public override void OnInspectorGUI() {

			base.OnInspectorGUI();
			serializedObject.Update();

			EditorGUILayout.Space();
			DrawObjectAndSetter();

			EditorGUILayout.Space();
			DrawInitialValue();
			DrawFinalValue();
			EditorGUILayout.PropertyField(DurationProperty);
			EditorGUILayout.PropertyField(TimeModeProperty);
			EditorGUILayout.PropertyField(EasingProperty);
			DrawCallbacks();

			serializedObject.ApplyModifiedProperties();

		}

		#endregion


		#region Protected Properties

		protected SerializedProperty InitialValueProperty { get; set; }

		protected SerializedProperty FinalValueProperty { get; set; }

		#endregion


		#region Protected Methods

		protected virtual void DrawInitialValue() => EditorGUILayout.PropertyField(InitialValueProperty);

		protected virtual void DrawFinalValue() => EditorGUILayout.PropertyField(FinalValueProperty);

		#endregion


		#region Private Fields

		private GUIStyle m_HorizontalLineStyle;

		private int m_CallbackSelection;

		private string[] m_CallbackOptions = new string[] { "On Start", "On Update", "On Interrupt", "On Complete" };

		#endregion


		#region Private Properties

		private SerializedProperty ObjectProperty { get; set; }

		private SerializedProperty SetterStringProperty { get; set; }

		private SerializedProperty DurationProperty { get; set; }

		private SerializedProperty TimeModeProperty { get; set; }

		private SerializedProperty EasingProperty { get; set; }

		private SerializedProperty OnStartProperty { get; set; }

		private SerializedProperty OnUpdateProperty { get; set; }
		
		private SerializedProperty OnInterruptProperty { get; set; }
		
		private SerializedProperty OnCompleteProperty { get; set; }

		private GUIStyle HorizontalLineStyle {
			get {
				if (m_HorizontalLineStyle == null) { 
					m_HorizontalLineStyle = new GUIStyle();
					m_HorizontalLineStyle.normal.background = EditorGUIUtility.whiteTexture;
					m_HorizontalLineStyle.margin = new RectOffset(0, 0, 4, 4);
					m_HorizontalLineStyle.fixedHeight = 1;
				}
				return m_HorizontalLineStyle;
			}
		}

		#endregion


		#region Private Methods

		private void DrawObjectAndSetter() {

			DrawLine();
			EditorGUILayout.LabelField("Setter", EditorStyles.boldLabel);
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
			DrawLine();

		}

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

		private List<string> GetSetterOptions() {

			var options = new List<string>();
			options.Add("No Function");

			if (ObjectProperty.objectReferenceValue != null) {
				if (ObjectProperty.objectReferenceValue is GameObject) {

					var gameObject = ObjectProperty.objectReferenceValue as GameObject;
					var components = gameObject.GetComponents(typeof(Component));

					foreach (var component in components) {

						var methods = GetMethodsBySignature(component.GetType(), typeof(void), typeof(ValueT));
						foreach (var setter in methods) {
							options.Add($"{component.GetType().Name}/{setter.Name}");
						}

						var properties = GetPropertiesByType(component.GetType(), typeof(ValueT));
						foreach (var property in properties) {
							options.Add($"{component.GetType().Name}/{property.Name}");
						}

					}

				} else {
					// TODO: Possibly work with ScriptableObjects (and fields)
				}
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

		private void DrawLine() {
			var color = GUI.color;
			GUI.color = new Color(0.15f, 0.15f, 0.15f);
			GUILayout.Box(GUIContent.none, HorizontalLineStyle);
			GUI.color = color;
		}



		#endregion


	}

}