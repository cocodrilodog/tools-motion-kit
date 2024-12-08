namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(AnimationClipAdapter))]
	public class AnimationClipAdapterEditor : CDEditor {


		#region Unity Methods

		protected override void OnEnable() {
			base.OnEnable();
			Undo.undoRedoPerformed += Undo_undoRedoPerfomed;
		}

		public override void OnInspectorGUI() {
			
			base.OnInspectorGUI();

			var animationClipAdapter = target as AnimationClipAdapter;
			
			EditorGUI.BeginDisabledGroup(!Application.isPlaying || !animationClipAdapter.enabled);
			
			// Time
			EditorGUI.BeginChangeCheck();
			var time = EditorGUILayout.FloatField(
				new GUIContent("Time", "The playhead position in seconds."), 
				animationClipAdapter.Time
			);
			if (EditorGUI.EndChangeCheck()) {
				animationClipAdapter.Time = time;
			}

			// NormalizedTime
			EditorGUI.BeginChangeCheck();
			var normalizedTime = EditorGUILayout.FloatField(
				new GUIContent("Normalized Time", "The playhead position from 0 to 1."), 
				animationClipAdapter.NormalizedTime
			);
			if (EditorGUI.EndChangeCheck()) {
				animationClipAdapter.NormalizedTime = normalizedTime;
			}

			EditorGUI.EndDisabledGroup();

		}

		private void OnDisable() {
			Undo.undoRedoPerformed -= Undo_undoRedoPerfomed;
		}

		#endregion


		#region Event Handlers

		private void Undo_undoRedoPerfomed() {
			(target as AnimationClipAdapter).Pause();
		}

		#endregion


	}

}