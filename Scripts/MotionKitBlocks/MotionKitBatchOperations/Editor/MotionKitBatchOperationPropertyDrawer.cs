namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(MotionKitBatchOperation))]
	public class MotionKitBatchOperationPropertyDrawer : CompositePropertyDrawer {


		#region Protected Properties

		protected override List<Type> CompositeTypes {
			get {
				if (m_CompositeTypes == null) {
					m_CompositeTypes = new List<Type> {
						typeof(CopyTemplateBlock),
						typeof(RenameBlock),
						typeof(SetSetterObject),
						typeof(SetIncrementalDuration),
						typeof(SetIncrementalValues3D),
						typeof(SetIncrementalValuesFloat),
						typeof(SetIncrementalValuesColor),
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