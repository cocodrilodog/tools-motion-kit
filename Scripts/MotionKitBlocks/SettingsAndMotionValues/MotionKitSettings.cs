namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(menuName = "Cocodrilo Dog/MotionKit/Settings")]
	public class MotionKitSettings : ScriptableObject {


		#region Public Properties

		public float Duration => m_Duration;

		public TimeMode TimeMode => m_TimeMode;

		public MotionKitEasingField Easing => m_Easing;

		#endregion


		#region Private Fields

		[SerializeField]
		private float m_Duration;

		[SerializeField]
		private TimeMode m_TimeMode;

		[SerializeField]
		private MotionKitEasingField m_Easing;

		#endregion


	}

}