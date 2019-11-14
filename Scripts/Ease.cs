namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public abstract class Ease<T> {


		#region Public Fields

		[SerializeField]
		public float Easing = 10;

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

		public T Speed {
			get { return m_Speed; }
			set { m_Speed = value; }
		}

		#endregion


		#region Constructors

		public Ease(Func<T> getter, Action<T> setter) {
			m_Getter = getter ?? throw new ArgumentNullException(nameof(getter));
			m_Setter = setter ?? throw new ArgumentNullException(nameof(setter));
		}

		#endregion


		#region Public Methods
		public void Update(T targetValue) {

			m_TargetValue = targetValue;
			T currentValue = m_Getter();
			T difference = Subtract(m_TargetValue, currentValue);
			Speed = MultiplyByFloat(difference, Easing);

			if (Mathf.Abs(Magnitude(difference)) > Threshold) {
				m_Setter(Add(currentValue, MultiplyByFloat(Speed, Time.deltaTime)));
				IsActive = true;
			} else {
				IsActive = false;
			}

		}
		#endregion


		#region Protected Methods

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
		private T m_TargetValue;

		[NonSerialized]
		private T m_Speed;

		[NonSerialized]
		private bool m_IsActive;

		#endregion


	}

	[Serializable]
	public class EaseFloat : Ease<float> {


		#region Constructors

		public EaseFloat(Func<float> getter, Action<float> setter) : base(getter, setter) { }

		#endregion


		#region Protected methods

		protected override float Magnitude(float value) {
			return value;
		}

		protected override float Add(float a, float b) {
			return a + b;
		}

		protected override float Subtract(float a, float b) {
			return a - b;
		}

		protected override float MultiplyByFloat(float a, float f) {
			return a * f;
		}

		#endregion


	}

}