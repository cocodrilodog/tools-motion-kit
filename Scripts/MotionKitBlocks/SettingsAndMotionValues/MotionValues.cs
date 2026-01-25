namespace CocodriloDog.MotionKit {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Base class for MotionValues assets.
	/// </summary>
	/// <typeparam name="ValueT"></typeparam>
	public class MotionValues<ValueT> : ScriptableObject {


		#region Public Properties

		public virtual ValueT InitialValue {
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

		public virtual ValueT FinalValue {
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

		[Tooltip("The initial value for the motion.")]
		[SerializeField]
		private ValueT m_InitialValue;

		[Tooltip("If checked, the animation will begin at the current value plus this value.")]
		[SerializeField]
		private bool m_InitialValueIsRelative;

		[Tooltip("The final value for the motion.")]
		[SerializeField]
		private ValueT m_FinalValue;

		[Tooltip("If checked, the animation will end at the current value plus this value.")]
		[SerializeField]
		private bool m_FinalValueIsRelative;

		#endregion


	}

}