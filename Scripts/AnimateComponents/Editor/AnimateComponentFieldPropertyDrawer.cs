namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(AnimateComponentField))]
	public class AnimateComponentFieldPropertyDrawer : MonoScriptableFieldPropertyDrawer {


		#region Protected Properties

		protected override List<Type> MonoScriptableTypes {
			get {
				if (m_MonoScriptableTypes == null) {
					m_MonoScriptableTypes = new List<Type> {
						typeof(Motion3DComponent),
						typeof(Motion2DComponent),
						typeof(MotionFloatComponent),
						typeof(MotionColorComponent),
						typeof(TimerComponent),
						typeof(SequenceComponent),
						typeof(ParallelAsset),
					};
				}
				return m_MonoScriptableTypes;
			}
		}

		#endregion


		#region Private Fields

		private List<Type> m_MonoScriptableTypes;

		#endregion


	}

}