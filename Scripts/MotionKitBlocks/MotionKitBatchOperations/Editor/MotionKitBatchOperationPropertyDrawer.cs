namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(MotionKitBatchOperation))]
	public class MotionKitBatchOperationPropertyDrawer : CompositePropertyDrawer {


		#region Protected Properties

		protected override bool UseDefaultDrawer => false;

		#endregion


		#region Protected Methods

		protected override void DrawPropertyField(Rect propertyRect, GUIContent guiContent, string name) {

			base.DrawPropertyField(propertyRect, guiContent, name);

			if (Property.managedReferenceValue != null) {

				// Draw the PlayOnStart toggle
				var enabledRect = propertyRect;
				enabledRect.xMin += propertyRect.width - 20;
				enabledRect.width = 10;
				var enabledProperty = Property.FindPropertyRelative("m_Enabled");
				enabledProperty.boolValue = EditorGUI.Toggle(enabledRect, enabledProperty.boolValue);
				DrawTooltip(enabledRect, "Enabled");

				void DrawTooltip(Rect toggleRect, string text) {
					if (toggleRect.Contains(Event.current.mousePosition)) {

						var content = new GUIContent(text);
						var size = EditorStyles.helpBox.CalcSize(content);
						var rect = new Rect(toggleRect.position + Vector2.left * size.x, size);

						// Use the dark color of the editor
						EditorGUI.DrawRect(rect, new Color(0.216f, 0.216f, 0.216f));
						GUI.Box(rect, content, EditorStyles.helpBox);

					}
				}

			}
		}

		#endregion


		#region Private Fields

		private List<Type> m_CompositeTypes;

		#endregion


	}

}