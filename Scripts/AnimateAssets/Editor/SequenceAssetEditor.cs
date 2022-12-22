namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(SequenceAsset))]
	public class SequenceAssetEditor : AnimateAssetEditor {


		#region Unity Methods

		protected override void OnEnable() {
			base.OnEnable();
			SequenceItemsProperty = serializedObject.FindProperty("m_SequenceItems");
		}

		#endregion


		#region Protected Methods

		protected override void DrawAfterSettings() {
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(SequenceItemsProperty);
		}

		#endregion


		#region Private Properties

		private SerializedProperty SequenceItemsProperty { get; set; }

		#endregion


	}

}