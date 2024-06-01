namespace CocodriloDog.MotionKit.Examples {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// This example uses <see cref="MotionKitBlock"/>s, but sets values and settings at runtime.
	/// </summary>
	[AddComponentMenu("")]
	public class DynamicMotionBlocks_Example : MonoBehaviour {


		#region Unity Methods

		private void Start() {
			PlayValues();
			PlaySettings();
			PlaySharedValues();
			PlaySharedSettings();
		}

		#endregion


		#region Event Handlers

		/// <summary>
		/// Plays the motion block. This is called from Start() and from the on complete of the motion,
		/// set in the inspector.
		/// </summary>
		public void PlayValues() {
			var motion3DBlock = m_MotionKitComponent.GetChild<Motion3DBlock>("ValuesMotion3D");
			motion3DBlock.FinalValue = Random.onUnitSphere * 2;
			motion3DBlock.Play();
		}

		/// <summary>
		/// Plays the sequence block. This is called from Start() and from the on complete of the sequence,
		/// set in the inspector.
		/// </summary>
		public void PlaySettings() {
			var sequence = m_MotionKitComponent.GetChild<SequenceBlock>("SettingsSequence");
			sequence.DurationInput = Random.Range(0.5f, 3);
			sequence.Play();
		}

		/// <summary>
		/// Plays the motion block. This is called from Start() and from the on complete of the motion,
		/// set in the inspector.
		/// </summary>
		public void PlaySharedValues() {
			
			var motion3DBlock = m_MotionKitComponent.GetChild<Motion3DBlock>("SharedValuesMotion3D");
			
			// Set the value in the shared values asset
			m_Shared3DValues.FinalValue = Random.onUnitSphere * 2;
			motion3DBlock.Play();

		}

		/// <summary>
		/// Plays the sequence block. This is called from Start() and from the on complete of the sequence,
		/// set in the inspector.
		/// </summary>
		public void PlaySharedSettings() {
			
			var sequence = m_MotionKitComponent.GetChild<SequenceBlock>("SharedSettingsSequence");

			// Set the duration in the shared settings asset
			m_SharedSettings.Duration = Random.Range(0.5f, 3);
			sequence.Play();

		}

		#endregion


		#region Private Fields

		[SerializeField]
		private MotionKitComponent m_MotionKitComponent;

		[Header("Shared Values and Settings")]

		[SerializeField]
		private Motion3DValues m_Shared3DValues;
		
		[SerializeField]
		private MotionKitSettings m_SharedSettings;

		#endregion


	}

}