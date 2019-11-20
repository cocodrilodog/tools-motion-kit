namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// An ease out <see cref="Follow{T}"/> animation.
	/// </summary>
	/// <typeparam name="T">The type to animate.</typeparam>
	[Serializable]
	public abstract class Ease<T> : Follow<T> {


		#region Public Fields
		
		/// <summary>
		/// Ease out amount. The higher this multiplier is the faster the ease out happens.
		/// </summary>
		[SerializeField]
		public float Easing = 10;

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

		protected Ease(Func<T> getter, Action<T> setter) : base(getter, setter) { }

		#endregion


		#region Protected Methods

		protected override T UpdateValue(T currentValue, T difference) {
			Speed = MultiplyByFloat(difference, Easing);
			return Add(currentValue, MultiplyByFloat(Speed, DeltaTime));
		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private T m_Speed;

		#endregion


	}

	/// <summary>
	/// An ease out <see cref="Follow{T}"/> animation for floats.
	/// </summary>
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