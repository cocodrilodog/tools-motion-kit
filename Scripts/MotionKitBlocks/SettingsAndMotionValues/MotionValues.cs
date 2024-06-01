namespace CocodriloDog.MotionKit {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class MotionValues<ValueT> : ScriptableObject {


		#region Public Properties

		public ValueT InitialValue {
			get => m_InitialValue;
			set {
				var raiseEvent = !value.Equals(m_InitialValue);
				m_InitialValue = value;
				if (raiseEvent) {
					OnValuesChange?.Invoke();
				}
			}
		}

		public bool InitialValueIsRelative => m_InitialValueIsRelative;

		public ValueT FinalValue {
			get => m_FinalValue;
			set {
				var raiseEvent = !value.Equals(m_FinalValue);
				m_FinalValue = value;
				if (raiseEvent) {
					OnValuesChange?.Invoke();
				}
			}
		}

		public bool FinalValueIsRelative => m_FinalValueIsRelative;

		#endregion


		#region Public Events

		public event Action OnValuesChange;

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