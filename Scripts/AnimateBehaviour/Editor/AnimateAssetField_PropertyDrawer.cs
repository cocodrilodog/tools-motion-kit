namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(AnimateAssetField))]
	public class AnimateAssetField_PropertyDrawer : MonoScriptableFieldPropertyDrawer {


		#region Protected Properties

		protected override List<Type> MonoScriptableTypes {
			get {
				if (m_MonoScriptableTypes == null) {
					m_MonoScriptableTypes = new List<Type> {
						typeof(MotionAsset),
						typeof(TimerAsset),
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