namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	public class MotionBlockPropertyDrawer<ValueT> : AnimateBlockPropertyDrawer {


		#region Protected Methods

		protected override void InitializePropertiesForGetHeight() {
			base.InitializePropertiesForGetHeight();
			ObjectProperty					= Property.FindPropertyRelative("m_Object");
			SetterStringProperty			= Property.FindPropertyRelative("m_SetterString");
			GetterStringProperty			= Property.FindPropertyRelative("m_GetterString");
			InitialValueProperty			= Property.FindPropertyRelative("m_InitialValue");
			InitialValueIsRelativeProperty	= Property.FindPropertyRelative("m_InitialValueIsRelative");
			FinalValueProperty				= Property.FindPropertyRelative("m_FinalValue");
			FinalValueIsRelativeProperty	= Property.FindPropertyRelative("m_FinalValueIsRelative");
		}

		#endregion


		#region Protected Properties

		protected SerializedProperty InitialValueProperty { get; set; }

		protected SerializedProperty InitialValueIsRelativeProperty { get; set; }

		protected SerializedProperty FinalValueProperty { get; set; }

		protected SerializedProperty FinalValueIsRelativeProperty { get; set; }

		protected override float BeforeSettingsHeight {
			get {
				var height = SpaceHeight; // <- Before settings space
				height += 5; // <- Horizontal line
				height += FieldHeight; // <- Setter label
				height += FieldHeight; // <- Object + setter field
				if (ShowSetterHelp) {
					height += FieldHeight * 2; // <- Help box
				}
				height += 2; // <- Gentle space before the line
				height += 5; // <- Horizontal line
				return height;
			}
		}

		#endregion


		#region Protected Methods

		protected override void DrawBeforeSettings() {
			DrawObjectAndSetter();
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

		private bool ShowSetterHelp => ObjectProperty.objectReferenceValue == null || SetterStringProperty.stringValue == "No Function";

		private List<string> GetterOptions => m_GetterOptions;

		#endregion


		#region Private Methods

		private void DrawObjectAndSetter() {

			GetNextPosition(SpaceHeight);

			// Title
			CDEditorUtility.DrawHorizontalLine(GetNextPosition(5f));
			EditorGUI.LabelField(GetNextPosition(), "Setter", EditorStyles.boldLabel);

			// Object + setter field
			var fieldRect = GetNextPosition();

			// Object field
			var objectRect = fieldRect;
			objectRect.width = fieldRect.width * 0.33f;
			EditorGUI.PropertyField(objectRect, ObjectProperty, GUIContent.none);

			// Setter string field
			UpdateSetterOptions();
			var index = Mathf.Clamp(SetterOptions.IndexOf(SetterStringProperty.stringValue), 0, int.MaxValue);
			var setterRect = fieldRect;
			setterRect.xMin = objectRect.xMax + 2;
			var newIndex = EditorGUI.Popup(setterRect, index, SetterOptions.ToArray());
			SetterStringProperty.stringValue = SetterOptions[newIndex];

			// Help box
			if (ShowSetterHelp) {
				EditorGUI.HelpBox(
					GetNextPosition(2), 
					"For a motion to have effect, an object and a function must be assigned.", 
					MessageType.Error
				);
			}

			GetNextPosition(2f); // <- small gentle space before the line
			CDEditorUtility.DrawHorizontalLine(GetNextPosition(5f));

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