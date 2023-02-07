namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(Motion2DComponent))]
	public class Motion2DComponentEditor : MotionBaseComponentEditor<Vector2> {


		#region Protected Methods

		/// <summary>
		/// This override changes the initial value property from 3D to 2D.
		/// </summary>
		protected override void DrawInitialValue() {
			EditorGUILayout.BeginHorizontal();
			DrawVector2FieldFromVector3Property(InitialValueProperty);
			EditorGUIUtility.labelWidth = 64;
			EditorGUILayout.PropertyField(InitialValueIsRelativeProperty, new GUIContent("Is Relative"), GUILayout.Width(80));
			EditorGUIUtility.labelWidth = 0;
			EditorGUILayout.EndHorizontal();
		}

		/// <summary>
		/// This override changes the final value property from 3D to 2D.
		/// </summary>
		protected override void DrawFinalValue() {
			EditorGUILayout.BeginHorizontal();
			DrawVector2FieldFromVector3Property(FinalValueProperty);
			EditorGUIUtility.labelWidth = 64;
			EditorGUILayout.PropertyField(FinalValueIsRelativeProperty, new GUIContent("Is Relative"), GUILayout.Width(80));
			EditorGUIUtility.labelWidth = 0;
			EditorGUILayout.EndHorizontal();
		}

		#endregion


		#region Private Methods

		/// <summary>
		/// Draws a <see cref="Vector3"/> property in a <see cref="Vector2"/> field.
		/// </summary>
		/// <param name="vector3Property"></param>
		private void DrawVector2FieldFromVector3Property(SerializedProperty vector3Property) {

			var xProperty = vector3Property.FindPropertyRelative("x");
			var yProperty = vector3Property.FindPropertyRelative("y");

			Vector2 currentValue = new Vector2(xProperty.floatValue, yProperty.floatValue);
			var newValue = EditorGUILayout.Vector2Field(vector3Property.displayName, currentValue);

			xProperty.floatValue = newValue.x;
			yProperty.floatValue = newValue.y;

		}

		#endregion


	}

}