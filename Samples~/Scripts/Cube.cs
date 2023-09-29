namespace CocodriloDog.MotionKit.Examples {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Cube : MonoBehaviour {


		#region Public Methods

		public void ToggleUpScaled() {
			m_IsUpScaled = !m_IsUpScaled;		
			m_MotionKitComponent.Play(m_IsUpScaled ? "ScaleUp" : "ScaleDown");
		}

		#endregion

		#region Private Fields - Serialized

		[SerializeField]
		private MotionKitComponent m_MotionKitComponent;

		#endregion

		#region Private Fields - Non Serialized

		[NonSerialized]
		private bool m_IsUpScaled;

		#endregion


	}
}