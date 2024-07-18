namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(MotionKitBatchOperationsGroup))]
	public class MotionKitBatchOperationsGroupPropertyDrawer : CompositeObjectPropertyDrawer {


		#region Protected Methods

		protected override void DrawPropertyField(Rect propertyRect, GUIContent guiContent, string name) {
			
			base.DrawPropertyField(propertyRect, guiContent, name);

			if (Property.managedReferenceValue != null) {
				// Draw the m_AffectedBlockIndex field
				var intFieldRect = propertyRect;
				intFieldRect.xMin += propertyRect.width - 20;
				intFieldRect.width = 20;
				var affectedBlockIndexProperty = Property.FindPropertyRelative("m_AffectedBlockIndex");
				affectedBlockIndexProperty.intValue = EditorGUI.IntField(intFieldRect, affectedBlockIndexProperty.intValue);
				DrawControlTooltip(intFieldRect, "Affected block index ");
			}

		}

		#endregion


		#region Private Methods

		private void DrawControlTooltip(Rect propertyControlRect, string text) {
			if (propertyControlRect.Contains(Event.current.mousePosition)) {

				var content = new GUIContent(text);
				var size = EditorStyles.helpBox.CalcSize(content);
				var rect = new Rect(propertyControlRect.position + Vector2.left * size.x, size);

				// Use the dark color of the editor
				EditorGUI.DrawRect(rect, new Color(0.216f, 0.216f, 0.216f));
				GUI.Box(rect, content, EditorStyles.helpBox);

			}
		}

		#endregion


	}

}