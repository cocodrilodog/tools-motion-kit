namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AnimateBehaviour : MonoBehaviour {


		#region Public Properties

		public float Progress => AssetField.Object.Progress;

		public float CurrentTime => AssetField.Object.CurrentTime;

		public float Duration => AssetField.Object.Duration;

		public bool IsPlaying => AssetField.Object.IsPlaying;

		public bool IsPaused => AssetField.Object.IsPaused;

		#endregion


		#region Public Methods

		private void Init() => AssetField.Object.Init();

		public void Play() => AssetField.Object.Play();

		public void Stop() => AssetField.Object.Stop();

		public void Pause() => AssetField.Object.Pause();

		public void Resume() => AssetField.Object.Resume();

		public void Dispose() => AssetField.Object.Dispose();

		#endregion


		#region Unity Methods

		private void Start() {
			if (PlayOnStart) {
				Play();
			} else {
				// This avoids errors OnDestroy in case it was not played at all.
				Init();
			}
		}

		private void OnDestroy() {
			Dispose();
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private AnimateAssetField m_AssetField;

		[SerializeField]
		private bool m_PlayOnStart;

		#endregion


		#region Private Properties

		private AnimateAssetField AssetField => m_AssetField;

		private bool PlayOnStart => m_PlayOnStart;

		#endregion


	}

}