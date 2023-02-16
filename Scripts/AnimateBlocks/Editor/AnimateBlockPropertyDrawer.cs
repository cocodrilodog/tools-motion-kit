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
						typeof(SequenceBlock),
						typeof(ParallelBlock),
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

		protected override void Edit_InitializePropertiesForGetHeight() {

			base.Edit_InitializePropertiesForGetHeight();

			OwnerProperty = Property.FindPropertyRelative("m_Owner");
			ReuseIDProperty = Property.FindPropertyRelative("m_ReuseID");
			DurationProperty = Property.FindPropertyRelative("m_Duration");
			TimeModeProperty = Property.FindPropertyRelative("m_TimeMode");
			EasingProperty = Property.FindPropertyRelative("m_Easing");

			OnStartProperty = Property.FindPropertyRelative("m_OnStart");
			OnUpdateProperty = Property.FindPropertyRelative("m_OnUpdate");
			OnInterruptProperty = Property.FindPropertyRelative("m_OnInterrupt");
			OnCompleteProperty = Property.FindPropertyRelative("m_OnComplete");
			CallbackSelectionProperty = Property.FindPropertyRelative("m_CallbackSelection");

			OnStartCallsProperty = OnStartProperty.FindPropertyRelative("m_PersistentCalls.m_Calls");
			OnUpdateCallsProperty = OnUpdateProperty.FindPropertyRelative("m_PersistentCalls.m_Calls");
			OnInterruptCallsProperty = OnInterruptProperty.FindPropertyRelative("m_PersistentCalls.m_Calls");
			OnCompleteCallsProperty = OnCompleteProperty.FindPropertyRelative("m_PersistentCalls.m_Calls");

		}

		protected override float Edit_GetPropertyHeight(SerializedProperty property, GUIContent label) {

			// Base height
			var height = base.Edit_GetPropertyHeight(property, label);

			// Owner and Reuse ID
			height += EditorGUI.GetPropertyHeight(OwnerProperty) + 2;
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

		protected override void Edit_OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			base.Edit_OnGUI(position, property, label);
			EditorGUI.PropertyField(GetNextPosition(OwnerProperty), OwnerProperty, new GUIContent("Owner (Optional)"));
			EditorGUI.PropertyField(GetNextPosition(ReuseIDProperty), ReuseIDProperty, new GUIContent("Reuse ID (Optional)"));
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

		private SerializedProperty OwnerProperty { get; set; }

		private SerializedProperty ReuseIDProperty { get; set; }

		private SerializedProperty DurationProperty { get; set; }

		private SerializedProperty TimeModeProperty { get; set; }

		private SerializedProperty EasingProperty { get; set; }

		private SerializedProperty OnStartProperty { get; set; }

		private SerializedProperty OnUpdateProperty { get; set; }

		private SerializedProperty OnInterruptProperty { get; set; }

		private SerializedProperty OnCompleteProperty { get; set; }

		private SerializedProperty CallbackSelectionProperty { get; set; }

		private SerializedProperty OnStartCallsProperty { get; set; }

		private SerializedProperty OnUpdateCallsProperty { get; set; }

		private SerializedProperty OnInterruptCallsProperty { get; set; }

		private SerializedProperty OnCompleteCallsProperty { get; set; }

		#endregion


		#region Private Fields

		private List<Type> m_CompositeTypes;

		private string[] m_CallbackOptions = new string[] { "On Start", "On Update", "On Interrupt", "On Complete" };

		#endregion


		#region Private Methods

		private void DrawCallbacks() {

			GetNextPosition(SpaceHeight);
			EditorGUI.LabelField(GetNextPosition(), "Callbacks", EditorStyles.boldLabel);

			var options = new string[4];
			options[0] = m_CallbackOptions[0] + (OnStartCallsProperty.arraySize > 0 ? $" ({OnStartCallsProperty.arraySize})" : "");
			options[1] = m_CallbackOptions[1] + (OnUpdateCallsProperty.arraySize > 0 ? $" ({OnUpdateCallsProperty.arraySize})" : "");
			options[2] = m_CallbackOptions[2] + (OnInterruptCallsProperty.arraySize > 0 ? $" ({OnInterruptCallsProperty.arraySize})" : "");
			options[3] = m_CallbackOptions[3] + (OnCompleteCallsProperty.arraySize > 0 ? $" ({OnCompleteCallsProperty.arraySize})" : "");

			CallbackSelectionProperty.intValue = GUI.Toolbar(GetNextPosition(), CallbackSelectionProperty.intValue, options);

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