namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// A spring out <see cref="Follow{T}"/> animation.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class Spring<T> : Follow<T> {


		#region Public Fields

		/// <summary>
		/// The spring strength. The higher the value, the stronger the spring is.
		/// </summary>
		[SerializeField]
		public float Strength = 32;

		/// <summary>
		/// The friction that eventually stops the srping. The higher the value, the faster 
		/// the movement stops.
		/// </summary>
		[SerializeField]
		public float Friction = 8;

		#endregion


		#region Public Properties

		/// <summary>
		/// The current speed of the animation.
		/// </summary>
		public T Speed {
			get { return m_Speed; }
			set { m_Speed = value; }
		}

		#endregion


		#region Protected Constructors

		protected Spring(Func<T> getter, Action<T> setter) : base(getter, setter) { }

		#endregion


		#region Protected Methods

		protected override T UpdateValue(T currentValue, T difference) {
			T acceleration = MultiplyByFloat(difference, Strength);
			Speed = Add(Speed, MultiplyByFloat(acceleration, DeltaTime));
			Speed = Subtract(Speed, MultiplyByFloat(Speed, Friction * DeltaTime));
			return Add(currentValue, MultiplyByFloat(Speed, DeltaTime));
		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private T m_Speed;

		#endregion


	}

	/// <summary>
	/// An spring out <see cref="Follow{T}"/> animation for floats.
	/// </summary>
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