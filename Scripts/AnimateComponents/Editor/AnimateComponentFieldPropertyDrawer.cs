namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(AnimateComponentField))]
	public class AnimateComponentFieldPropertyDrawer : MonoCompositeFieldPropertyDrawer {


		#region Protected Properties

		protected override List<Type> MonoCompositeTypes {
			get {
				if (m_MonoCompositeTypes == null) {
					m_MonoCompositeTypes = new List<Type> {
						typeof(Motion3DComponent),
						typeof(Motion2DComponent),
						typeof(MotionFloatComponent),
						typeof(MotionColorComponent),
						typeof(TimerComponent),
						typeof(SequenceComponent),
						typeof(ParallelComponent),
					};
				}
				return m_MonoCompositeTypes;
			}
		}

		#endregion


		#region Private Fields

		private List<Type> m_MonoCompositeTypes;

		#endregion


	}

}