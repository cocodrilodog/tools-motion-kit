namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public abstract class Spring<T> : Follow<T> {


		#region Public Fields

		[SerializeField]
		public float Strength = 32;

		[SerializeField]
		public float Friction = 8;

		#endregion


		#region Public Properties

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
			Speed = Add(Speed, MultiplyByFloat(acceleration, Time.deltaTime));
			Speed = Subtract(Speed, MultiplyByFloat(Speed, Friction * Time.deltaTime));
			return Add(currentValue, MultiplyByFloat(Speed, Time.deltaTime));
		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private T m_Speed;

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