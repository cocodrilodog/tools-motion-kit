namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AnimateBehaviour : MonoBehaviour {


		#region Public Properties

		public float Progress => (float)DefaultAnimateAsset?.Object?.Progress;

		public float CurrentTime => (float)DefaultAnimateAsset?.Object?.CurrentTime;

		public float Duration => (float)DefaultAnimateAsset?.Object?.Duration;

		public bool IsPlaying => (bool)DefaultAnimateAsset?.Object?.IsPlaying;

		public bool IsPaused => (bool)DefaultAnimateAsset?.Object?.IsPaused;

		#endregion


		#region Public Methods

		private void Initialize() {
			foreach (var asset in AnimateAssets) {
				asset.Object?.Initialize();
			}
		}

		public void Play() => DefaultAnimateAsset?.Object?.Play();

		public void Stop() => DefaultAnimateAsset?.Object?.Stop();

		public void Pause() => DefaultAnimateAsset?.Object?.Pause();

		public void Resume() => DefaultAnimateAsset?.Object?.Resume();

		public void Dispose() {
			foreach (var asset in AnimateAssets) {
				asset.Object?.Dispose();
			}
		}

		#endregion


		#region Unity Methods

		private void Start() {
			if (PlayOnStart) {
				Play();
			} else {
				// This avoids errors OnDestroy in case it was not played at all.
				Initialize();
			}
		}

		private void OnDestroy() {
			Dispose();
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private List<AnimateAssetField> m_AnimateAssets;

		[SerializeField]
		private bool m_PlayOnStart;

		#endregion


		#region Private Properties

		private List<AnimateAssetField> AnimateAssets => m_AnimateAssets;

		private AnimateAssetField DefaultAnimateAsset => AnimateAssets.Count > 0 ? AnimateAssets[0] : null;

		private bool PlayOnStart => m_PlayOnStart;

		#endregion


	}

}