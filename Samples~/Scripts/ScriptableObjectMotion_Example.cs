namespace CocodriloDog.MotionKit.Examples {
	
	using CocodriloDog.Core;
	using System;
	using UnityEngine;

	[AddComponentMenu("")]
	public class ScriptableObjectMotion_Example : MonoBehaviour {


		#region Unity Methods

		private void Update() {
			transform.position = m_AnimatedPosition.Value;
			ColorAdapter.Alpha = m_AnimatedAlpha.Value;
		}

		#endregion


		#region Private Fields

		[CreateAsset]
		[SerializeField]
		private ScriptableVector3 m_AnimatedPosition;

		[CreateAsset]
		[SerializeField]
		private ScriptableFloat m_AnimatedAlpha;

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private ColorAdapter m_ColorAdapter;

		#endregion


		#region Private Properties

		private ColorAdapter ColorAdapter {
			get {
				if(m_ColorAdapter == null) {
					m_ColorAdapter = GetComponent<ColorAdapter>();
				}
				return m_ColorAdapter;
			}
		}

		#endregion


	}

}