namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	// [CustomPropertyDrawer(typeof(MotionBaseBlock<>), true)] // This fallback doesn't work
	public class MotionBlockPropertyDrawer<ValueT> : MotionKitBlockPropertyDrawer {


		#region Protected Constants

		protected const string NoFunctionString = "No Function";

		protected const string IsRelativeString = "Is Relative";

		protected const float IsRelativeLabelWidth = 64;

		#endregion


		#region Protected Methods

		protected override void Edit_InitializePropertiesForGetHeight() {

			base.Edit_InitializePropertiesForGetHeight();

			ObjectProperty					= Property.FindPropertyRelative("m_Object");
			SetterStringProperty			= Property.FindPropertyRelative("m_SetterString");
			GetterStringProperty			= Property.FindPropertyRelative("m_GetterString");

			SharedValuesProperty = Property.FindPropertyRelative("m_SharedValues");

			if (SharedValuesProperty.objectReferenceValue != null) {
				// use the properties from the shared asset
				InitialValueProperty = SharedValuesSerializedObject.FindProperty("m_InitialValue");
				InitialValueIsRelativeProperty = SharedValuesSerializedObject.FindProperty("m_InitialValueIsRelative");
				FinalValueProperty = SharedValuesSerializedObject.FindProperty("m_FinalValue");
				FinalValueIsRelativeProperty = SharedValuesSerializedObject.FindProperty("m_FinalValueIsRelative");
			} else {
				// Use the propreties from the MotionBlock
				InitialValueProperty = Property.FindPropertyRelative("m_InitialValue");
				InitialValueIsRelativeProperty = Property.FindPropertyRelative("m_InitialValueIsRelative");
				FinalValueProperty = Property.FindPropertyRelative("m_FinalValue");
				FinalValueIsRelativeProperty = Property.FindPropertyRelative("m_FinalValueIsRelative");
			}
			
		}

		#endregion


		#region Protected Properties

		protected SerializedProperty InitialValueProperty { get; set; }

		protected SerializedProperty InitialValueIsRelativeProperty { get; set; }

		protected SerializedProperty FinalValueProperty { get; set; }

		protected SerializedProperty FinalValueIsRelativeProperty { get; set; }

		protected SerializedProperty SharedValuesProperty { get; set; }

		protected override float BeforeSettingsHeight {
			get {
				var height = SpaceHeight;		// <- Before settings space
				height += HorizontalLineHeight; // <- Initial horizontal line
				height += FieldHeight;			// <- Setter label
				height += EditorGUI.GetPropertyHeight(ObjectProperty) + 2; // <- Object + setter field
				if (ShowSetterHelp) {
					height += FieldHeight * 2;	// <- Help box
				}
				height += 2;					// <- Gentle space before the line
				height += HorizontalLineHeight; // <- Final horizontal line
				return height;
			}
		}

		protected override float SettingsHeight {
			get {
				var height = base.SettingsHeight;
				height += SpaceHeight;				// <- Values space
				height += FieldHeight;				// <- Values label
				height += EditorGUI.GetPropertyHeight(InitialValueProperty) + 2;
				height += EditorGUI.GetPropertyHeight(FinalValueProperty) + 2;
				if (ShowGetter) {
					height += SpaceHeight;			// <- Getter space
					height += HorizontalLineHeight; // <- Initial horizontal line
					height += FieldHeight;          // <- Getter label
					height += EditorGUI.GetPropertyHeight(ObjectProperty) + 2;
					if (ShowGetterHelp) {
						height += FieldHeight * 2;	// <- Help box
					}
					height += 2;                    // <- Gentle space before the line
					height += HorizontalLineHeight; // <- Final horizontal line
				}
				return height;
			}		
		}

		protected SerializedObject SharedValuesSerializedObject {
			get {
				if (SharedValuesProperty.objectReferenceValue != null && m_SharedValuesSerializedObject == null) {
					m_SharedValuesSerializedObject = new SerializedObject(SharedValuesProperty.objectReferenceValue);
				}
				return m_SharedValuesSerializedObject;
			}
		}

		#endregion


		#region Protected Methods

		protected override void DrawBeforeSettings() {
			DrawObjectAndSetter();
		}

		protected override void DrawSettings() {

			base.DrawSettings();
			GetNextPosition(SpaceHeight);

			var rect = GetNextPosition();

			// Change the color to shared
			var color = GUI.contentColor;
			if (SharedValuesProperty.objectReferenceValue != null) {
				EditorStyles.label.normal.textColor = SharedColor;
				EditorStyles.boldLabel.normal.textColor = SharedColor;
			}

			// Values label
			var labelRect = rect;
			labelRect.width = EditorGUIUtility.labelWidth;
			EditorGUI.LabelField(
				labelRect,
				SharedValuesProperty.objectReferenceValue != null ? "Values (Shared)" : "Values",
				EditorStyles.boldLabel
			);

			// Shared settings field
			SharedValuesSerializedObject?.Update();
			var sharedSettingsRect = rect;
			sharedSettingsRect.xMin += labelRect.width;
			EditorGUIUtility.labelWidth = 50;
			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(sharedSettingsRect, SharedValuesProperty, new GUIContent("Shared"));
			if (EditorGUI.EndChangeCheck()) {
				// This renews SerializedSharedValues either if the new value is null or not
				m_SharedValuesSerializedObject = null;
			}
			EditorGUIUtility.labelWidth = 0;

			// Draw the values
			DrawInitialValueAndIsRelative();
			DrawFinalValueAndIsRelative();
			SharedValuesSerializedObject?.ApplyModifiedProperties();

			// Reset the color
			EditorStyles.label.normal.textColor = color;
			EditorStyles.boldLabel.normal.textColor = color;

			if (ShowGetter) {
				DrawDisabledObjectAndGetter();
			}

		}

		protected void GetValueRects(Rect fieldRect, out Rect valueRect, out Rect isRelativeRect) {
			valueRect = fieldRect;
			valueRect.width -= 90;
			isRelativeRect = fieldRect;
			isRelativeRect.xMin = valueRect.xMax + 5;
		}

		protected virtual void DrawInitialValue(Rect valueRect) => EditorGUI.PropertyField(valueRect, InitialValueProperty);
		
		protected virtual void DrawFinalValue(Rect valueRect) => EditorGUI.PropertyField(valueRect, FinalValueProperty);

		#endregion


		#region Private Fields

		private List<string> m_SetterOptions;

		private int m_SetterOptionIndex;

		private List<string> m_GetterOptions;

		private int m_GetterOptionIndex;

		private SerializedObject m_SharedValuesSerializedObject;

		#endregion


		#region Private Properties

		private SerializedProperty ObjectProperty { get; set; }

		private SerializedProperty SetterStringProperty { get; set; }

		private SerializedProperty GetterStringProperty { get; set; }

		private List<string> SetterOptions => m_SetterOptions;

		private bool ShowSetterHelp => ObjectProperty.objectReferenceValue == null || m_SetterOptionIndex == 0;

		private List<string> GetterOptions => m_GetterOptions;

		public bool ShowGetter => InitialValueIsRelativeProperty.boolValue || FinalValueIsRelativeProperty.boolValue;

		private bool ShowGetterHelp => ObjectProperty.objectReferenceValue == null || m_GetterOptionIndex == 0;

		private float HorizontalLineHeight => 5;

		#endregion


		#region Private Methods

		private void DrawObjectAndSetter() {

			GetNextPosition(SpaceHeight);

			// Title
			CDEditorUtility.DrawHorizontalLine(GetNextPosition(HorizontalLineHeight));
			EditorGUI.LabelField(GetNextPosition(), "Setter", EditorStyles.boldLabel);

			// Object + setter field
			var fieldRect = GetNextPosition();
			GetObjectAndAccesorRects(fieldRect, out Rect objectRect, out Rect setterRect);

			// Object field
			EditorGUI.PropertyField(objectRect, ObjectProperty, GUIContent.none);

			// Setter string field
			UpdateSetterOptions();
			// Clamping converts -1 into 0 when there is an empty string or it is not found
			var index = Mathf.Clamp(SetterOptions.IndexOf(SetterStringProperty.stringValue), 0, int.MaxValue);

			EditorGUI.BeginChangeCheck();
			m_SetterOptionIndex = EditorGUI.Popup(setterRect, index, SetterOptions.ToArray());
			// Changing the value only when the popup changes makes the current setter to resist
			// as much as possible and never assign the "No Function" (m_SetterOptionIndex = 0)
			// instead assign "" for consistency with the initial state of the property
			if (EditorGUI.EndChangeCheck()) {
				SetterStringProperty.stringValue = m_SetterOptionIndex == 0 ? "" : SetterOptions[m_SetterOptionIndex];
			}

			// Help box
			if (ShowSetterHelp) {
				EditorGUI.HelpBox(
					GetNextPosition(2), 
					"For a motion to have effect, an object and a function must be assigned.", 
					MessageType.Error
				);
			}

			GetNextPosition(2f); // <- small gentle space before the line
			CDEditorUtility.DrawHorizontalLine(GetNextPosition(HorizontalLineHeight));

		}

		private void DrawDisabledObjectAndGetter() {

			GetNextPosition(SpaceHeight);

			// Title
			CDEditorUtility.DrawHorizontalLine(GetNextPosition(HorizontalLineHeight));
			EditorGUI.LabelField(GetNextPosition(), "Getter", EditorStyles.boldLabel);

			// Object + setter field
			var fieldRect = GetNextPosition();
			GetObjectAndAccesorRects(fieldRect, out Rect objectRect, out Rect getterRect);

			// Object field
			EditorGUI.BeginDisabledGroup(true);
			EditorGUI.PropertyField(objectRect, ObjectProperty, GUIContent.none);
			EditorGUI.EndDisabledGroup();

			// Getter string field
			UpdateGetterOptions();
			// Clamping converts -1 into 0 when there is an empty string or it is not found
			var index = Mathf.Clamp(GetterOptions.IndexOf(GetterStringProperty.stringValue), 0, int.MaxValue);

			EditorGUI.BeginChangeCheck();
			m_GetterOptionIndex = EditorGUI.Popup(getterRect, index, GetterOptions.ToArray());
			// Changing the value only when the popup changes makes the current getter to resist
			// as much as possible and never assign the "No Function" (m_GetterOptionIndex = 0)
			// instead assign "" for consistency with the initial state of the property
			if (EditorGUI.EndChangeCheck()) {
				GetterStringProperty.stringValue = m_GetterOptionIndex == 0 ? "" : GetterOptions[m_GetterOptionIndex];
			}

			// Help box
			if (ShowGetterHelp) {
				EditorGUI.HelpBox(
					GetNextPosition(2),
					"For a relative initial or final value to work, an object and a function must be assigned.",
					MessageType.Error
				);
			}

			GetNextPosition(2f); // <- small gentle space before the line
			CDEditorUtility.DrawHorizontalLine(GetNextPosition(HorizontalLineHeight));

		}

		private void GetObjectAndAccesorRects(Rect fieldRect, out Rect objectRect, out Rect accesorRect) {
			objectRect = fieldRect;
			objectRect.width = fieldRect.width * 0.33f;
			accesorRect = fieldRect;
			accesorRect.xMin = objectRect.xMax + 2;
		}

		private void UpdateSetterOptions() {

			m_SetterOptions = new List<string>();
			m_SetterOptions.Add(NoFunctionString);

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
			m_GetterOptions.Add(NoFunctionString);

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

		private void DrawInitialValueAndIsRelative() {

			GetValueRects(GetNextPosition(InitialValueProperty), out Rect valueRect, out Rect isRelativeRect);

			// Value field
			DrawInitialValue(valueRect);

			// Is relative field
			EditorGUIUtility.labelWidth = IsRelativeLabelWidth;
			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(isRelativeRect, InitialValueIsRelativeProperty, new GUIContent(IsRelativeString));
			if (EditorGUI.EndChangeCheck()) {
				if (Application.isPlaying) {
					throw new InvalidOperationException($"{InitialValueIsRelativeProperty.displayName} can not be set at runtime.");
				}
			}
			EditorGUIUtility.labelWidth = 0;

		}

		private void DrawFinalValueAndIsRelative() {

			GetValueRects(GetNextPosition(InitialValueProperty), out Rect valueRect, out Rect isRelativeRect);

			// Value field
			DrawFinalValue(valueRect);

			// Is relative field
			EditorGUIUtility.labelWidth = IsRelativeLabelWidth;
			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(isRelativeRect, FinalValueIsRelativeProperty, new GUIContent(IsRelativeString));
			if (EditorGUI.EndChangeCheck()) {
				if (Application.isPlaying) {
					throw new InvalidOperationException($"{FinalValueIsRelativeProperty.displayName} can not be set at runtime.");
				}
			}
			EditorGUIUtility.labelWidth = 0;

		}

		#endregion


	}

}