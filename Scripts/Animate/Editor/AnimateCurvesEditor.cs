namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;
	using UnityEditorInternal;

	/// <summary>
	/// The custom editor for the <see cref="AnimateCurves"/> assets.
	/// </summary>
	[CustomEditor(typeof(AnimateCurves))]
	public class AnimateCurvesEditor : Editor {


		#region Unity Methods

		public override void OnInspectorGUI() {
			serializedObject.Update();
			AnimationsList.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}

		#endregion


		#region Private Fields

		private ReorderableList m_AnimationsList;

		#endregion


		#region Internal Properties

		private ReorderableList AnimationsList {
			get {

				if(m_AnimationsList == null) {

					m_AnimationsList = new ReorderableList(
						serializedObject, serializedObject.FindProperty("m_Animations"),
						true, true, true, true
					);

					m_AnimationsList.elementHeight *= 2;
					m_AnimationsList.drawHeaderCallback = DrawHeaderCallback;
					m_AnimationsList.drawElementCallback = DrawElementCalback;

				}

				return m_AnimationsList;

			}
		}

		#endregion


		#region Internal Methods

		void DrawHeaderCallback(Rect rect) {
			EditorGUI.LabelField(rect, "Animate Curves");
		}

		void DrawElementCalback(Rect rect, int index, bool isActive, bool isFocused) {

			SerializedProperty element = AnimationsList.serializedProperty.GetArrayElementAtIndex(index);

			rect.y += 2;

			EditorGUI.PropertyField(
				new Rect(
					rect.x,
					rect.y, 
					EditorGUIUtility.labelWidth,
					EditorGUIUtility.singleLineHeight * 1
				),
				element.FindPropertyRelative("m_Key"),
				GUIContent.none
			);

			// PropertyField doesn't render properly when reordering. Intead, use curve field:
			EditorGUI.BeginChangeCheck();
			AnimationCurve animationCurve = EditorGUI.CurveField(
				new Rect(
					rect.x + EditorGUIUtility.labelWidth + 4, 
					rect.y, 
					rect.width - EditorGUIUtility.labelWidth - 4, 
					EditorGUIUtility.singleLineHeight * 2
				),
				GUIContent.none,
				element.FindPropertyRelative("m_AnimationCurve").animationCurveValue
			);
			if(EditorGUI.EndChangeCheck()) {
				element.FindPropertyRelative("m_AnimationCurve").animationCurveValue = animationCurve;
				EditorUtility.SetDirty(target);
			}
		}

		#endregion


	}
}