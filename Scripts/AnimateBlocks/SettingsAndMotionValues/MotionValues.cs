namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class MotionValues<ValueT> : ScriptableObject {


		#region Public Properties

		public ValueT InitialValue => m_InitialValue;

		public bool InitialValueIsRelative => m_InitialValueIsRelative;

		public ValueT FinalValue => m_FinalValue;

		public bool FinalValueIsRelative => m_FinalValueIsRelative;

		#endregion


		#region Private Fields

		[SerializeField]
		private ValueT m_InitialValue;

		[SerializeField]
		private bool m_InitialValueIsRelative;

		[SerializeField]
		private ValueT m_FinalValue;

		[SerializeField]
		private bool m_FinalValueIsRelative;

		#endregion


	}

}