namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public abstract class Spring<T> {


		#region Public Fields

		[SerializeField]
		public float Strength = 32;

		[SerializeField]
		public float Friction = 8;

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

		public Spring(Func<T> getter, Action<T> setter) {
			m_Getter = getter ?? throw new ArgumentNullException(nameof(getter));
			m_Setter = setter ?? throw new ArgumentNullException(nameof(setter));
		}

		#endregion


		#region Public Methods
		public void Update(T targetValue) {

			m_TargetValue = targetValue;
			T currentValue = m_Getter();
			T difference = Subtract(m_TargetValue, currentValue);
			//Speed = MultiplyByFloat(difference, Easing);


			//TargetContentX = GetContentXForPage(CurrentPage);
			//deltaX = TargetContentX - ContentX;



			//float acceleration = deltaX * spring;
			//ContentSpeedX += acceleration * Time.deltaTime;
			//ContentSpeedX -= friction * ContentSpeedX * Time.deltaTime;
			//ContentX += ContentSpeedX * Time.deltaTime;


			if (Mathf.Abs(Magnitude(difference)) > Threshold) {
				T acceleration = MultiplyByFloat(difference, Strength);
				Speed = Add(Speed, MultiplyByFloat(acceleration, Time.deltaTime));
				Speed = Subtract(Speed, MultiplyByFloat(Speed, Friction * Time.deltaTime));
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
		private bool m_IsActive;

		[NonSerialized]
		private T m_Speed;

		[NonSerialized]
		private T m_TargetValue;

		#endregion


	}

	[Serializable]
	public class SpringFloat : Spring<float> {


		#region Constructors

		public SpringFloat(Func<float> getter, Action<float> setter) : base(getter, setter) { }

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