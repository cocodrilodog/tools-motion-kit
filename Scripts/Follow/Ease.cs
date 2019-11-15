namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public abstract class Ease<T> : Follow<T> {


		#region Public Fields

		[SerializeField]
		public float Easing = 10;

		#endregion


		#region Public Properties

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
			return Add(currentValue, MultiplyByFloat(Speed, Time.deltaTime));
		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private T m_Speed;

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