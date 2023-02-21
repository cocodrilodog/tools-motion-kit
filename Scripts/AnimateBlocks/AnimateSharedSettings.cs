namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(menuName = "Cocodrilo Dog/Animate/Shared Settings")]
	public class AnimateSharedSettings : ScriptableObject {


		#region Public Properties

		public float Duration => m_Duration;

		public TimeMode TimeMode => m_TimeMode;

		public AnimateEasingField Easing => m_Easing;

		#endregion


		#region Private Fields

		[SerializeField]
		private float m_Duration;

		[SerializeField]
		private TimeMode m_TimeMode;

		[SerializeField]
		private AnimateEasingField m_Easing;

		#endregion


	}

}