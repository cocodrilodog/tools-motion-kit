namespace CocodriloDog.MotionKit.Examples {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[AddComponentMenu("")]
	public class DynamicMotionBlocks_Example : MonoBehaviour {


		#region Unity Methods

		private void Start() {
			PlayValues();
		}

		#endregion


		#region Event Handlers

		/// <summary>
		/// Plays the motion block. This is called from start and from the on complete of the motion.
		/// </summary>
		public void PlayValues() {
			var motion3DBlock = m_MotionKitComponent.GetChild<Motion3DBlock>("ValuesMotion3D");
			motion3DBlock.FinalValue = (Vector3.right * -4) + Random.onUnitSphere * 2;
			motion3DBlock.Play();
		}

		/// <summary>
		/// Plays the sequence block. This is called from start and from the on complete of the sequence.
		/// </summary>
		public void PlaySettings() {
			var sequence = m_MotionKitComponent.GetChild<SequenceBlock>("SettingsSequence");
			sequence.DurationInput = Random.Range(0.5f, 3);
			sequence.Play();
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private MotionKitComponent m_MotionKitComponent;

		#endregion


	}

}