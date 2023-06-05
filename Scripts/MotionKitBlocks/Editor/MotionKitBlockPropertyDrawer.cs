namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(PlaybackBlock))]
	public class MotionKitBlockPropertyDrawer : CompositePropertyDrawer {


		#region Public Static Properties

		/// <summary>
		/// Taken from the ScriptableObject icon.
		/// </summary>
		public static Color SharedColor => new Color(121f / 255, 204f / 255, 239f / 255);

		#endregion


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

			OwnerProperty				= Property.FindPropertyRelative("m_Owner");
			ReuseIDProperty				= Property.FindPropertyRelative("m_ReuseID");

			SharedSettingsProperty		= Property.FindPropertyRelative("m_SharedSettings");
			
			if (SharedSettingsProperty.objectReferenceValue != null) {
				// Use the properties from the shared asset
				DurationProperty	= SerializedSharedSettings.FindProperty("m_Duration");
				TimeModeProperty	= SerializedSharedSettings.FindProperty("m_TimeMode");
				EasingProperty		= SerializedSharedSettings.FindProperty("m_Easing");
			} else {
				// Use the properties from the MotionKitBlock
				DurationProperty	= Property.FindPropertyRelative("m_Duration");
				TimeModeProperty	= Property.FindPropertyRelative("m_TimeMode");
				EasingProperty		= Property.FindPropertyRelative("m_Easing");
			}

			OnStartProperty				= Property.FindPropertyRelative("m_OnStart");
			OnUpdateProperty			= Property.FindPropertyRelative("m_OnUpdate");
			OnInterruptProperty			= Property.FindPropertyRelative("m_OnInterrupt");
			OnCompleteProperty			= Property.FindPropertyRelative("m_OnComplete");
			CallbackSelectionProperty	= Property.FindPropertyRelative("m_CallbackSelection");

			OnStartCallsProperty		= OnStartProperty.FindPropertyRelative("m_PersistentCalls.m_Calls");
			OnUpdateCallsProperty		= OnUpdateProperty.FindPropertyRelative("m_PersistentCalls.m_Calls");
			OnInterruptCallsProperty	= OnInterruptProperty.FindPropertyRelative("m_PersistentCalls.m_Calls");
			OnCompleteCallsProperty		= OnCompleteProperty.FindPropertyRelative("m_PersistentCalls.m_Calls");

		}

		protected override float Edit_GetPropertyHeight(SerializedProperty property, GUIContent label) {

			// Base height
			var height = base.Edit_GetPropertyHeight(property, label);

			// Owner and Reuse ID
			height += EditorGUI.GetPropertyHeight(OwnerProperty) + 2;

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
			DrawOwnerAndReuseID();
			DrawBeforeSettings();
			DrawSettings();
			DrawAfterSettings();
			DrawCallbacks();
		}

		protected virtual void DrawBeforeSettings() { }

		protected virtual void DrawSettings() {

			GetNextPosition(SpaceHeight);

			var rect = GetNextPosition();

			// Change the color to shared
			var color = GUI.contentColor;
			if (SharedSettingsProperty.objectReferenceValue != null) {
				EditorStyles.label.normal.textColor = SharedColor;
				EditorStyles.boldLabel.normal.textColor = SharedColor;
			}

			// Settings label
			var labelRect = rect;
			labelRect.width = EditorGUIUtility.labelWidth;
			EditorGUI.LabelField(
				labelRect, 
				SharedSettingsProperty.objectReferenceValue != null ? "Settings (Shared)" : "Settings",
				EditorStyles.boldLabel
			);

			// Shared settings
			SerializedSharedSettings?.Update();
			var sharedSettingsRect = rect;
			sharedSettingsRect.xMin += labelRect.width;
			EditorGUIUtility.labelWidth = 50;
			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(sharedSettingsRect, SharedSettingsProperty, new GUIContent("Shared"));
			if (EditorGUI.EndChangeCheck()) {
				// This renews SerializedSharedSettings either if the new value is null or not
				m_SerializedSharedSettings = null;
			}
			EditorGUIUtility.labelWidth = 0;

			// Settings properties
			EditorGUI.PropertyField(GetNextPosition(DurationProperty), DurationProperty);
			EditorGUI.PropertyField(GetNextPosition(TimeModeProperty), TimeModeProperty);
			if (DoesDrawEasing) {
				EditorGUI.PropertyField(GetNextPosition(EasingProperty), EasingProperty);
			}

			// Reset the color
			EditorStyles.label.normal.textColor = color;
			EditorStyles.boldLabel.normal.textColor = color;

			SerializedSharedSettings?.ApplyModifiedProperties();

			// Restore the color when the seelection changes.
			//
			// If the user double clicks the shared settings assset, the shared color would remain, causing
			// other inspectors to use that color. For that reason, we need to handle the selectionChange
			// event and reset the color there too. Since the MotionBlock classes inherit MotionKitBlock,
			// this code will handle also the case in which the user double clicks the shared values asset.
			if (!m_IsSubscribedToSelectionChange) {
				m_IsSubscribedToSelectionChange = true;
				Selection.selectionChanged += Selection_selectionChanged;
			}

			void Selection_selectionChanged() {
				m_IsSubscribedToSelectionChange = false;
				Selection.selectionChanged -= Selection_selectionChanged;
				EditorStyles.label.normal.textColor = color;
				EditorStyles.boldLabel.normal.textColor = color;
			}

		}

		protected virtual void DrawAfterSettings() { }

		#endregion


		#region Private Fields

		private SerializedObject m_SerializedSharedSettings;

		private bool m_IsSubscribedToSelectionChange;

		#endregion


		#region Private Properties

		private SerializedProperty OwnerProperty { get; set; }

		private SerializedProperty ReuseIDProperty { get; set; }

		private SerializedProperty DurationProperty { get; set; }

		private SerializedProperty TimeModeProperty { get; set; }

		private SerializedProperty EasingProperty { get; set; }

		private SerializedProperty SharedSettingsProperty { get; set; }

		private SerializedObject SerializedSharedSettings {
			get {
				if (SharedSettingsProperty.objectReferenceValue != null && m_SerializedSharedSettings == null) {
					m_SerializedSharedSettings = new SerializedObject(SharedSettingsProperty.objectReferenceValue);
				}
				return m_SerializedSharedSettings;
			}
		}

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

		private bool m_EditOwnerAndReuseID;

		#endregion


		#region Private Methods

		private void DrawOwnerAndReuseID() {

			var mainRect = GetNextPosition(OwnerProperty);
			if (Property.managedReferenceValue != null && m_EditOwnerAndReuseID) {

				var fieldsRect = mainRect;
				fieldsRect.xMax -= 60;

				EditorGUIUtility.labelWidth = 70;

				var ownerRect = fieldsRect;
				ownerRect.width = (fieldsRect.width / 2) - 5;
				EditorGUI.PropertyField(ownerRect, OwnerProperty);

				var reuseIDRect = fieldsRect;
				reuseIDRect.xMin += (fieldsRect.width / 2) + 5;
				EditorGUI.PropertyField(reuseIDRect, ReuseIDProperty);

				var closeButtonRect = mainRect;
				closeButtonRect.xMin += fieldsRect.width + 10;

				if (GUI.Button(closeButtonRect, "Done!")) {
					m_EditOwnerAndReuseID = false;
				}

				EditorGUIUtility.labelWidth = 0;

			} else {
				var buttonRect = mainRect;
				var ownerString = OwnerProperty.objectReferenceValue != null ? $"[{OwnerProperty.objectReferenceValue.name}] " : "";
				var reuseIDString = !string.IsNullOrEmpty(ReuseIDProperty.stringValue) ? $"[{ReuseIDProperty.stringValue}] " : "";
				if (GUI.Button(buttonRect, $"Edit Owner {ownerString}and/or Reuse ID {reuseIDString}")) {
					if (Property.managedReferenceValue != null) {
						m_EditOwnerAndReuseID = true;
					}
				}
			}

		}

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