namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public abstract class Follow {


		#region Public Fields

		[SerializeField]
		public float Easing = 10;

		#endregion


	}

	[Serializable]
	public class FollowFloat : Follow {


		#region Public Properties

		public float Speed { 
			get { return m_Speed; }
			set { m_Speed = value; }
		}

		#endregion


		#region Constructors

		public FollowFloat(Func<float> getter, Action<float> setter) {
			m_Getter = getter ?? throw new ArgumentNullException(nameof(getter));
			m_Setter = setter ?? throw new ArgumentNullException(nameof(setter));
		}

		#endregion


		#region Public methods

		public void Update(float targetValue) {
			float currentValue = m_Getter();
			float difference = targetValue - currentValue;
			m_Speed = difference * Easing;
			currentValue += m_Speed * Time.deltaTime;
			m_Setter(currentValue);
		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private Func<float> m_Getter;

		[NonSerialized]
		private Action<float> m_Setter;

		[NonSerialized]
		private float m_Speed;

		#endregion


	}

}