namespace CocodriloDog.Animation.Examples {

	using CocodriloDog.Animation;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;


	[Serializable]
	public class Motion3DSetter : UnityEvent<Vector3> { }

	public class MotionInspector_Example : MonoBehaviour {


		#region Unity Methods

		private void Start() {
			Animate.GetMotion(p => Setter.Invoke(p))
				.SetEasing(Easing.Vector3Easing)
				.Play(InitialValue, FinalValue, Duration);
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private Vector3 m_InitialValue;

		[SerializeField]
		private Vector3 m_FinalValue;

		[SerializeField]
		private float m_Duration;

		[SerializeField]
		private AnimateEasingField m_Easing = new AnimateEasingField();

		[SerializeField]
		private Motion3DSetter m_Setter;

		#endregion


		#region Private Fields

		private Vector3 InitialValue => m_InitialValue;

		private Vector3 FinalValue => m_FinalValue;

		private float Duration => m_Duration;

		private AnimateEasingField Easing => m_Easing;

		private Motion3DSetter Setter => m_Setter;

		#endregion


	}

}