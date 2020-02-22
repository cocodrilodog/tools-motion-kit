namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	[CustomPropertyDrawer(typeof(AnimateCurve))]
	public class AnimateCurvePropertyDrawer : PropertyDrawerBase {


		#region Public Methods

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return FieldHeight;
		}

		#endregion


		#region Unity Methods

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			base.OnGUI(position, property, label);
			EditorGUI.PropertyField(GetNextPosition(), CurveProperty, Label);
		}

		#endregion


		#region Protected Methods

		protected override void InitializePropertiesForOnGUI() {
			base.InitializePropertiesForOnGUI();
			CurveProperty = Property.FindPropertyRelative("Curve");
		}

		#endregion


		#region Private Properties

		private SerializedProperty CurveProperty { get; set; }

		#endregion


	}

}