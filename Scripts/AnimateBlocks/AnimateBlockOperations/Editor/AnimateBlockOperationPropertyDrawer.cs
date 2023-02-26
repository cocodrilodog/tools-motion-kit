namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(AnimateBlockOperation))]
	public class AnimateBlockOperationPropertyDrawer : CompositePropertyDrawer {


		#region Protected Properties

		protected override List<Type> CompositeTypes {
			get {
				if (m_CompositeTypes == null) {
					m_CompositeTypes = new List<Type> {
						typeof(Rename),
						typeof(IncrementalDuration),
					};
				}
				return m_CompositeTypes;
			}
		}

		#endregion


		#region Private Fields

		private List<Type> m_CompositeTypes;

		#endregion


	}

}