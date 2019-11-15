namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public abstract class Follow<T> {


		#region Public Fields

		[SerializeField]
		public float Threshold = 0.001f;

		#endregion


		#region Public Properties

		public bool IsActive {
			get { return m_IsActive; }
			protected set {
				if (value != m_IsActive) {
					m_IsActive = value;
					Debug.LogFormat("IsActive: {0}; {1}", m_IsActive, Time.time);
					if (!m_IsActive) {
						m_Setter(m_TargetValue);
					}
				}
			}
		}

		#endregion


		#region Constructors

		public Follow(Func<T> getter, Action<T> setter) {
			m_Getter = getter ?? throw new ArgumentNullException(nameof(getter));
			m_Setter = setter ?? throw new ArgumentNullException(nameof(setter));
		}

		#endregion


		#region Public Methods

		public void Update(T targetValue) {

			m_TargetValue = targetValue;
			T currentValue = m_Getter();
			T difference = Subtract(m_TargetValue, m_Getter());

			if (Mathf.Abs(Magnitude(difference)) > Threshold) {
				m_Setter(UpdateValue(currentValue, difference));
				IsActive = true;
			} else {
				IsActive = false;
			}

		}

		#endregion


		#region Protected Methods

		protected abstract T UpdateValue(T currentValue, T difference);

		protected abstract float Magnitude(T value);

		protected abstract T Add(T a, T b);

		protected abstract T Subtract(T a, T b);

		protected abstract T MultiplyByFloat(T a, float f);

		#endregion


		#region Private Fields

		[NonSerialized]
		private Func<T> m_Getter;

		[NonSerialized]
		private Action<T> m_Setter;

		[NonSerialized]
		private bool m_IsActive;

		[NonSerialized]
		private T m_TargetValue;

		#endregion


	}
}