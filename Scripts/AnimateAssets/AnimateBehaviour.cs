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
				Get();
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

		private Motion3DAsset Motion3DAsset => AssetField.Object as Motion3DAsset;

		private Motion2DAsset Motion2DAsset => AssetField.Object as Motion2DAsset;

		private MotionFloatAsset MotionFloatAsset => AssetField.Object as MotionFloatAsset;

		private MotionColorAsset MotionColorAsset => AssetField.Object as MotionColorAsset;

		#endregion


		#region Private Methods

		private void Get() {
			Motion3DAsset?.GetMotion();
			Motion2DAsset?.GetMotion();
			MotionFloatAsset?.GetMotion();
			MotionColorAsset?.GetMotion();
		}

		#endregion


	}

}