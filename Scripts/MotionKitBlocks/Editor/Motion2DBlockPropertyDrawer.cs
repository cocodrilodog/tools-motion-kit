namespace CocodriloDog.MotionKit {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(Motion2DBlock))]
	public class Motion2DBlockPropertyDrawer : MotionBlockPropertyDrawer<Vector2> {


		#region Protected Methods

		/// <summary>
		/// This override changes the initial value property from 3D to 2D.
		/// </summary>
		protected override void DrawInitialValue() {

			GetValueRects(GetNextPosition(InitialValueProperty), out Rect valueRect, out Rect isRelativeRect);

			// Value field
			DrawVector2FieldFromVector3Property(valueRect, InitialValueProperty);

			// Is relative field
			EditorGUIUtility.labelWidth = IsRelativeLabelWidth;
			EditorGUI.PropertyField(isRelativeRect, InitialValueIsRelativeProperty, new GUIContent(IsRelativeString));
			EditorGUIUtility.labelWidth = 0;

		}

		/// <summary>
		/// This override changes the final value property from 3D to 2D.
		/// </summary>
		protected override void DrawFinalValue() {

			GetValueRects(GetNextPosition(InitialValueProperty), out Rect valueRect, out Rect isRelativeRect);

			// Value field
			DrawVector2FieldFromVector3Property(valueRect, FinalValueProperty);

			// Is relative field
			EditorGUIUtility.labelWidth = IsRelativeLabelWidth;
			EditorGUI.PropertyField(isRelativeRect, FinalValueIsRelativeProperty, new GUIContent(IsRelativeString));
			EditorGUIUtility.labelWidth = 0;

		}

		#endregion


		#region Private Methods

		/// <summary>
		/// Draws a <see cref="Vector3"/> property in a <see cref="Vector2"/> field.
		/// </summary>
		/// <param name="vector3Property"></param>
		private void DrawVector2FieldFromVector3Property(Rect rect, SerializedProperty vector3Property) {

			var xProperty = vector3Property.FindPropertyRelative("x");
			var yProperty = vector3Property.FindPropertyRelative("y");

			Vector2 currentValue = new Vector2(xProperty.floatValue, yProperty.floatValue);
			var newValue = EditorGUI.Vector2Field(rect, vector3Property.displayName, currentValue);

			xProperty.floatValue = newValue.x;
			yProperty.floatValue = newValue.y;

		}

		#endregion


	}

}