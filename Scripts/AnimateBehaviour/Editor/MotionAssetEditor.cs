namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(MotionAsset))]
	public class MotionAsset_Editor : MonoScriptableObjectEditor {


		#region Unity Methods

		protected override void OnEnable() {
			base.OnEnable();
			MotionProperty = serializedObject.FindProperty("Motion");
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			serializedObject.Update();
			EditorGUILayout.PropertyField(MotionProperty);
			serializedObject.ApplyModifiedProperties();
		}

		#endregion


		#region Private Properties

		private SerializedProperty MotionProperty { get; set; }

		#endregion


	}

}