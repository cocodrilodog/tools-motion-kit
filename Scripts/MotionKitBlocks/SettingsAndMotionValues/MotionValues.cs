namespace CocodriloDog.MotionKit {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class MotionValues<ValueT> : ScriptableObject {


		#region Public Properties

		public ValueT InitialValue {
			get => m_InitialValue;
			set => m_InitialValue = value;
		}

		public bool InitialValueIsRelative => m_InitialValueIsRelative;

		public ValueT FinalValue {
			get => m_FinalValue;
			set => m_FinalValue = value;
		}

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