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

		public float Speed { get { return m_Speed; } }

		#endregion


		#region Public methods

		public float Follow(float targetValue, float value) {
			float difference = targetValue - value;
			m_Speed = difference * Easing;
			value += m_Speed * Time.deltaTime;
			return value;
		}

		#endregion


		#region 

		[NonSerialized]
		private float m_Speed;

		#endregion


	}

	[Serializable]
	public class FollowVector3 : Follow {


		#region Public Properties

		public Vector3 Speed { get { return m_Speed; } }

		#endregion


		#region Public methods

		public Vector3 Follow(Vector3 targetValue, Vector3 value) {
			Vector3 difference = targetValue - value;
			m_Speed = difference * Easing;
			value += m_Speed * Time.deltaTime;
			return value;
		}

		#endregion


		#region 

		[NonSerialized]
		private Vector3 m_Speed;

		#endregion


	}
}