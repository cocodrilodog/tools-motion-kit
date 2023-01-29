namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(ParallelComponent))]
	public class ParallelComponentEditor : AnimateComponentEditor {


		#region Unity Methods

		protected override void OnEnable() {
			base.OnEnable();
			if (target != null) {
				ParallelItemsProperty = serializedObject.FindProperty("m_ParallelItems");
			}
		}

		#endregion


		#region Protected Methods

		protected override void DrawAfterSettings() {
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(ParallelItemsProperty);
		}

		#endregion


		#region Private Properties

		private SerializedProperty ParallelItemsProperty { get; set; }

		#endregion


	}

}