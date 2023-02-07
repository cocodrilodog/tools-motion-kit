namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(AnimateBlock))]
	public class AnimateBlockPropertyDrawer : CompositePropertyDrawer {


		#region Protected Properties

		protected override List<Type> CompositeTypes {
			get {
				if (m_CompositeTypes == null) {
					m_CompositeTypes = new List<Type> {
						typeof(Motion3DBlock),
						typeof(Motion2DBlock),
						typeof(MotionFloatBlock),
						typeof(MotionColorBlock),
						typeof(TimerBlock),
					};
				}
				return m_CompositeTypes;
			}
		}

		protected virtual bool DoesDrawEasing => true;

		protected virtual float BeforeSettingsHeight => 0;
		
		protected virtual float SettingsHeight {
			get {
				var height = SpaceHeight;	// <- Settings space
				height += FieldHeight;		// <- Settings label
				height += EditorGUI.GetPropertyHeight(DurationProperty) + 2;
				height += EditorGUI.GetPropertyHeight(TimeModeProperty) + 2;
				if (DoesDrawEasing) {
					height += EditorGUI.GetPropertyHeight(EasingProperty) + 2;
				}
				return height;
			}
		}

		protected virtual float AfterSettingsHeight => 0;

		#endregion


		#region Protected Methods

		protected override void InitializePropertiesForGetHeight() {

			base.InitializePropertiesForGetHeight();

			ReuseIDProperty		= Property.FindPropertyRelative("m_ReuseID");
			DurationProperty	= Property.FindPropertyRelative("m_Duration");
			TimeModeProperty	= Property.FindPropertyRelative("m_TimeMode");
			EasingProperty		= Property.FindPropertyRelative("m_Easing");

			OnStartProperty				= Property.FindPropertyRelative("m_OnStart");
			OnUpdateProperty			= Property.FindPropertyRelative("m_OnUpdate");
			OnInterruptProperty			= Property.FindPropertyRelative("m_OnInterrupt");
			OnCompleteProperty			= Property.FindPropertyRelative("m_OnComplete");
			CallbackSelectionProperty	= Property.FindPropertyRelative("m_CallbackSelection");

		}

		protected override float GetEditPropertyHeight(SerializedProperty property, GUIContent label) {

			// Base height
			var height = base.GetEditPropertyHeight(property, label);

			// Reuse ID
			height += EditorGUI.GetPropertyHeight(ReuseIDProperty) + 2;

			// Before settings
			height += BeforeSettingsHeight;

			// Settings
			height += SettingsHeight;
			
			// After settings
			height += AfterSettingsHeight;

			// Callbacks
			height += SpaceHeight; // <- Callbacks space
			height += FieldHeight; // <- Callbacks label
			height += FieldHeight; // <- Callbacks selection toolbar

			switch (CallbackSelectionProperty.intValue) {
				case 0: height += EditorGUI.GetPropertyHeight(OnStartProperty); break;
				case 1: height += EditorGUI.GetPropertyHeight(OnUpdateProperty); break;
				case 2: height += EditorGUI.GetPropertyHeight(OnInterruptProperty); break;
				case 3: height += EditorGUI.GetPropertyHeight(OnCompleteProperty); break;
			}

			return height;
		}

		protected override void OnEditGUI(Rect position, SerializedProperty property, GUIContent label) {
			base.OnEditGUI(position, property, label);
			DrawReuseID();
			DrawBeforeSettings();
			DrawSettings();
			DrawAfterSettings();
			DrawCallbacks();
		}

		protected virtual void DrawBeforeSettings() { }

		protected virtual void DrawSettings() {
			GetNextPosition(SpaceHeight);
			EditorGUI.LabelField(GetNextPosition(), "Settings", EditorStyles.boldLabel);
			EditorGUI.PropertyField(GetNextPosition(DurationProperty), DurationProperty);
			EditorGUI.PropertyField(GetNextPosition(TimeModeProperty), TimeModeProperty);
			if (DoesDrawEasing) {
				EditorGUI.PropertyField(GetNextPosition(EasingProperty), EasingProperty);
			}
		}

		protected virtual void DrawAfterSettings() { }

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


		#region Private Fields

		private List<Type> m_CompositeTypes;

		private string[] m_CallbackOptions = new string[] { "On Start", "On Update", "On Interrupt", "On Complete" };

		#endregion


		#region Private Methods

		private void DrawReuseID() => EditorGUI.PropertyField(GetNextPosition(ReuseIDProperty), ReuseIDProperty);

		private void DrawCallbacks() {

			GetNextPosition(SpaceHeight);
			EditorGUI.LabelField(GetNextPosition(), "Callbacks", EditorStyles.boldLabel);

			CallbackSelectionProperty.intValue = GUI.Toolbar(GetNextPosition(), CallbackSelectionProperty.intValue, m_CallbackOptions);

			switch (CallbackSelectionProperty.intValue) {
				case 0: EditorGUI.PropertyField(GetNextPosition(OnStartProperty), OnStartProperty); break;
				case 1: EditorGUI.PropertyField(GetNextPosition(OnUpdateProperty), OnUpdateProperty); break;
				case 2: EditorGUI.PropertyField(GetNextPosition(OnInterruptProperty), OnInterruptProperty); break;
				case 3: EditorGUI.PropertyField(GetNextPosition(OnCompleteProperty), OnCompleteProperty); break;
			}

		}

		#endregion


	}

}