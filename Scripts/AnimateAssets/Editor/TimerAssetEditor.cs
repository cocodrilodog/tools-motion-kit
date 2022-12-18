namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(TimerAsset))]
	public class TimerAssetEditor : AnimateAssetEditor {


		#region Unity Methods

		protected override void OnEnable() {
			base.OnEnable();
			//MotionProperty = serializedObject.FindProperty("Timer");
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			//serializedObject.Update();
			//EditorGUILayout.PropertyField(MotionProperty);
			//serializedObject.ApplyModifiedProperties();
		}

		#endregion


	}

}