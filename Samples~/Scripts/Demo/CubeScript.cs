namespace CocodriloDog.MotionKit.Examples {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class CubeScript : MonoBehaviour {


		#region Public Methods

		public void ToggleUpScaled() {

			m_IsUpScaled = !m_IsUpScaled;
			
			if (m_IsUpScaled) {
				MotionKit.GetMotion(this, "Scale", s => transform.localScale = s)
					.SetEasing(MotionKitEasing.ElasticOut)
					.Play(transform.localScale, new Vector3(1, 2, 2), 1.2f);
			} else {
				MotionKit.GetMotion(this, "Scale", s => transform.localScale = s)
					.SetEasing(MotionKitEasing.ElasticOut)
					.Play(transform.localScale, new Vector3(1, 1, 1), 1.2f);
			}

		}

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private bool m_IsUpScaled;

		#endregion


	}
}