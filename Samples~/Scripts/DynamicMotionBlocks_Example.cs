namespace CocodriloDog.MotionKit.Examples {

	using System;
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
			
			// Changing final value at runtime
			var motion3DBlock = m_MotionKitComponent.GetChild<Motion3DBlock>("ValuesMotion3D");
			motion3DBlock.FinalValue = UnityEngine.Random.onUnitSphere * 2;
			
			// Changing callbacks at runtime (Additional to the callbacks in the editor)
			motion3DBlock.SetOnStart_Runtime(
				m_CallingCallback1 ? 
				() => CallBack1(motion3DBlock.Name, "OnStart_Runtime") : 
				() => CallBack2(motion3DBlock.Name, "OnStart_Runtime")
			);

			// Changing callbacks at runtime (Additional to the callbacks in the editor)
			motion3DBlock.SetOnComplete_Runtime(
				m_CallingCallback1 ?
				() => CallBack1(motion3DBlock.Name, "OnComplete_Runtime") :
				() => CallBack2(motion3DBlock.Name, "OnComplete_Runtime")
			);

			m_CallingCallback1 = !m_CallingCallback1;

			// Play the motion
			motion3DBlock.Play();

		}

		/// <summary>
		/// Plays the sequence block. This is called from Start() and from the on complete of the sequence,
		/// set in the inspector.
		/// </summary>
		public void PlaySettings() {
			var sequence = m_MotionKitComponent.GetChild<SequenceBlock>("SettingsSequence");
			sequence.DurationInput = UnityEngine.Random.Range(0.5f, 3);
			sequence.Play();
		}

		/// <summary>
		/// Plays the motion block. This is called from Start() and from the on complete of the motion,
		/// set in the inspector.
		/// </summary>
		public void PlaySharedValues() {
			
			var motion3DBlock = m_MotionKitComponent.GetChild<Motion3DBlock>("SharedValuesMotion3D");
			
			// Set the value in the shared values asset
			m_Shared3DValues.FinalValue = UnityEngine.Random.onUnitSphere * 2;
			motion3DBlock.Play();

		}

		/// <summary>
		/// Plays the sequence block. This is called from Start() and from the on complete of the sequence,
		/// set in the inspector.
		/// </summary>
		public void PlaySharedSettings() {
			
			var sequence = m_MotionKitComponent.GetChild<SequenceBlock>("SharedSettingsSequence");

			// Set the duration in the shared settings asset
			m_SharedSettings.Duration = UnityEngine.Random.Range(0.5f, 3);
			sequence.Play();

		}

		#endregion


		#region Private Fields - Serialized

		[SerializeField]
		private MotionKitComponent m_MotionKitComponent;

		[Header("Shared Values and Settings")]

		[SerializeField]
		private Motion3DValues m_Shared3DValues;
		
		[SerializeField]
		private MotionKitSettings m_SharedSettings;

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private bool m_CallingCallback1;

		#endregion


		#region Private Methods

		private void CallBack1(string motionName, string eventName) {
			Debug.Log($"1 {motionName}: {eventName} {Time.time}");
		}

		private void CallBack2(string motionName, string eventName) {
			Debug.Log($"2 {motionName}: {eventName} {Time.time}");
		}

		#endregion


	}

}