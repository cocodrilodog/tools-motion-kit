namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(PositionXYZAdapter))]
	public class PositionXYZAdapterEditor : CDEditor {


		#region Unity Methods

		public override void OnInspectorGUI() {

			base.OnInspectorGUI();

			var positionXYZ = target as PositionXYZAdapter;

			EditorGUIUtility.labelWidth = 50;
			
			EditorGUILayout.BeginHorizontal();
			DrawX(positionXYZ);
			DrawY(positionXYZ);
			DrawZ(positionXYZ);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			DrawLocalX(positionXYZ);
			DrawLocalY(positionXYZ);
			DrawLocalZ(positionXYZ);
			EditorGUILayout.EndHorizontal();

			EditorGUIUtility.labelWidth = 0;

		}

		#endregion


		#region Private Methods

		private void DrawX(PositionXYZAdapter positionXYZ) {
			var label = new GUIContent("X", "transform.position.x");
			EditorGUI.BeginChangeCheck();
			var x = EditorGUILayout.FloatField(label, positionXYZ.X);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(positionXYZ.transform, "Change X.");
				positionXYZ.X = x;
			}
		}

		private void DrawY(PositionXYZAdapter positionAdapter) {
			var label = new GUIContent("Y", "transform.position.y");
			EditorGUI.BeginChangeCheck();
			var y = EditorGUILayout.FloatField(label, positionAdapter.Y);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(positionAdapter.transform, "Change Y.");
				positionAdapter.Y = y;
			}
		}

		private void DrawZ(PositionXYZAdapter positionAdapter) {
			var label = new GUIContent("Z", "transform.position.z");
			EditorGUI.BeginChangeCheck();
			var z = EditorGUILayout.FloatField(label, positionAdapter.Z);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(positionAdapter.transform, "Change Z.");
				positionAdapter.Z = z;
			}
		}

		private void DrawLocalX(PositionXYZAdapter positionXYZ) {
			var label = new GUIContent("Local X", "transform.localPosition.x");
			EditorGUI.BeginChangeCheck();
			var localX = EditorGUILayout.FloatField(label, positionXYZ.LocalX);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(positionXYZ.transform, "Change localX.");
				positionXYZ.LocalX = localX;
			}
		}

		private void DrawLocalY(PositionXYZAdapter positionXYZ) {
			var label = new GUIContent("Local Y", "transform.localPosition.y");
			EditorGUI.BeginChangeCheck();
			var localY = EditorGUILayout.FloatField(label, positionXYZ.LocalY);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(positionXYZ.transform, "Change LocalY.");
				positionXYZ.LocalY = localY;
			}
		}

		private void DrawLocalZ(PositionXYZAdapter positionXYZ) {
			var label = new GUIContent("Local Z", "transform.localPosition.z");
			EditorGUI.BeginChangeCheck();
			var localZ = EditorGUILayout.FloatField(label, positionXYZ.LocalZ);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(positionXYZ.transform, "Change LocalZ.");
				positionXYZ.LocalZ = localZ;
			}
		}

		#endregion


	}

}