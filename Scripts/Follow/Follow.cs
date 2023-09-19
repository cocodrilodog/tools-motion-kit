namespace CocodriloDog.MotionKit {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Base class for objects that animate properties which values tend toward a target value
	/// over time.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public abstract class Follow<T> {


		#region Public Fields

		/// <summary>
		/// The <see cref="CocodriloDog.MotionKit.TimeMode"/> of this follow animation.
		/// </summary>
		public TimeMode TimeMode;

		/// <summary>
		/// This field is used to set the animation in idle state when the difference
		/// between the target value and current value is less than the
		/// <see cref="Threshold"/> value. The animation is idle when <see cref="IsActive"/>
		/// <c> == false</c>.
		/// </summary>
		[SerializeField]
		public float Threshold = 0.001f;

		#endregion


		#region Public Properties

		/// <summary>
		/// Is this animation active?
		/// </summary>
		public bool IsActive {
			get { return m_IsActive; }
			protected set {
				if (value != m_IsActive) {
					m_IsActive = value;
					if (!m_IsActive) {
						m_Setter(m_TargetValue);
						OnBecameIdle?.Invoke();
					}
				}
			}
		}

		#endregion


		#region Constructors

		/// <summary>
		/// Creates an instance of a Follow animation.
		/// </summary>
		/// <param name="getter">The getter of the property to animate.</param>
		/// <param name="setter">The setter of the property to animate.</param>
		public Follow(Func<T> getter, Action<T> setter) {
			m_Getter = getter ?? throw new ArgumentNullException(nameof(getter));
			m_Setter = setter ?? throw new ArgumentNullException(nameof(setter));
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Updates this follow animation according to the specific implementation.
		/// </summary>
		/// <param name="targetValue">The value that this animation tends to over time.</param>
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


		#region Public Events

		/// <summary>
		/// Raised when this animation becomes inactive.
		/// </summary>
		public event Action OnBecameIdle;

		#endregion


		#region Protected Properties

		/// <summary>
		/// The delta time.
		/// </summary>
		protected float DeltaTime {
			get {
				switch (TimeMode) {
					case TimeMode.Normal: return Time.deltaTime;
					case TimeMode.Unscaled: return Time.unscaledDeltaTime;
					case TimeMode.Smooth: return Time.smoothDeltaTime;
					case TimeMode.Fixed: return Time.fixedDeltaTime;
					case TimeMode.FixedUnscaled: return Time.fixedUnscaledTime;
					default: return Time.deltaTime;
				}
			}
		}

		#endregion


		#region Protected Methods

		/// <summary>
		/// Implement this to process the algorithm that will generate an animation.
		/// </summary>
		/// <param name="currentValue">The current value of the animated property.</param>
		/// <param name="difference">The target value of the animated property</param>
		/// <returns></returns>
		protected abstract T UpdateValue(T currentValue, T difference);

		/// <summary>
		/// Implement this to define a <c>float</c> magnitude for the provided <paramref name="value"/>.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		protected abstract float Magnitude(T value);

		/// <summary>
		/// Implement this to define a sum of the <c>T</c> type.
		/// </summary>
		/// <param name="a">First object to sum.</param>
		/// <param name="b">Second object to sum.</param>
		/// <returns>The result of the sum</returns>
		protected abstract T Add(T a, T b);

		/// <summary>
		/// Implement this to define a subtraction of the <c>T</c> type.
		/// </summary>
		/// <param name="a">Object to subtract from.</param>
		/// <param name="b">Object to subtract.</param>
		/// <returns>The result of the subtraction.</returns>
		protected abstract T Subtract(T a, T b);

		/// <summary>
		/// Implement this to define a multiplication between the <c>T</c> type and <c>float</c>.
		/// </summary>
		/// <param name="a">The <c>T</c> value.</param>
		/// <param name="f">The <c>float</c> value.</param>
		/// <returns>The result of the multiplication.</returns>
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