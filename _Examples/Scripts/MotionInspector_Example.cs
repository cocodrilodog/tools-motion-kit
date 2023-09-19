namespace CocodriloDog.MotionKit.Examples {

	using CocodriloDog.MotionKit;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;


	[Serializable]
	public class Motion3DSetter : UnityEvent<Vector3> { }

	/// <summary>
	/// This is an example of how a motion can be defined in an inspector.
	/// </summary>
	/// 
	/// <remarks>
	/// The methods listed in the event popup should only be dynamic methods.
	/// There should be a way of providing a getter via reflection for when we
	/// want the animation to start in the current value.
	/// </remarks>
	[AddComponentMenu("")]
	public class MotionInspector_Example : MonoBehaviour {


		#region Unity Methods

		private void Start() {
			MotionKit.GetMotion(p => Setter.Invoke(p))
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
		private MotionKitEasingField m_Easing = new MotionKitEasingField();

		[SerializeField]
		private Motion3DSetter m_Setter;

		#endregion


		#region Private Fields

		private Vector3 InitialValue => m_InitialValue;

		private Vector3 FinalValue => m_FinalValue;

		private float Duration => m_Duration;

		private MotionKitEasingField Easing => m_Easing;

		private Motion3DSetter Setter => m_Setter;

		#endregion


	}

}