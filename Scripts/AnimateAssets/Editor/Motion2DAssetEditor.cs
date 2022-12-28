namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(Motion2DAsset))]
	public class Motion2DAssetEditor : MotionBaseAssetEditor<Vector2> {


		#region Protected Methods

		/// <summary>
		/// This override changes the initial value property from 3D to 2D.
		/// </summary>
		protected override void DrawInitialValue() => DrawVector2FieldFromVector3Property(InitialValueProperty);

		/// <summary>
		/// This override changes the final value property from 3D to 2D.
		/// </summary>
		protected override void DrawFinalValue() => DrawVector2FieldFromVector3Property(FinalValueProperty);

		#endregion


		#region Private Properties

		/// <summary>
		/// Draws a <see cref="Vector3"/> property in a <see cref="Vector2"/> field.
		/// </summary>
		/// <param name="vector3Property"></param>
		private void DrawVector2FieldFromVector3Property(SerializedProperty vector3Property) {

			var xProperty = vector3Property.FindPropertyRelative("x");
			var yProperty = vector3Property.FindPropertyRelative("y");

			Vector2 currentValue = new Vector2(xProperty.floatValue, yProperty.floatValue);
			var newValue = EditorGUILayout.Vector2Field(InitialValueProperty.displayName, currentValue);

			xProperty.floatValue = newValue.x;
			yProperty.floatValue = newValue.y;

		}

		#endregion


	}

}