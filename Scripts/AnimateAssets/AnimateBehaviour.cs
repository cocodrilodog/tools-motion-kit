namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	public class AnimateBehaviour : MonoBehaviour, IMonoScriptableOwner {


		#region Public Properties

		public float Progress => (float)DefaultAnimateAsset?.Progress;

		public float CurrentTime => (float)DefaultAnimateAsset?.CurrentTime;

		public float Duration => (float)DefaultAnimateAsset?.Duration;

		public bool IsPlaying => (bool)DefaultAnimateAsset?.IsPlaying;

		public bool IsPaused => (bool)DefaultAnimateAsset?.IsPaused;

		#endregion


		#region Public Methods

		private void Initialize() {
			foreach (var assetField in AnimateAssetFields) {
				assetField.Object?.Initialize();
			}
		}

		public void Play() => DefaultAnimateAsset?.Play();

		public void Play(string assetName) => GetAnimateAsset(assetName)?.Play();

		public void Stop() => DefaultAnimateAsset?.Stop();

		public void Stop(string assetName) => GetAnimateAsset(assetName)?.Stop();

		public void Pause() => DefaultAnimateAsset?.Pause();

		public void Pause(string assetName) => GetAnimateAsset(assetName)?.Pause();

		public void Resume() => DefaultAnimateAsset?.Resume();

		public void Resume(string assetName) => GetAnimateAsset(assetName)?.Resume();

		public void Dispose() {
			foreach (var asset in AnimateAssetFields) {
				asset.Object?.Dispose();
			}
		}

		public void RecreateMonoScriptableObjects() {
			foreach (var assetField in AnimateAssetFields) {
				if (assetField.Object != null) {

					var clone = Instantiate(assetField.Object);
					clone.name = assetField.Object.name;

					assetField.SetObject(clone);
					assetField.Object.SetOwner(this);
					if(assetField.Object is IMonoScriptableOwner) {
						((IMonoScriptableOwner)assetField.Object).RecreateMonoScriptableObjects();
					}

				}
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

		/// <summary>
		/// A list of <see cref="AnimateAssetField"/>.
		/// </summary>
		/// <remarks>
		/// This name is friendlier than m_AnimateAssetFields in the inspector.
		/// </remarks>
		[SerializeField]
		private List<AnimateAssetField> m_AnimateAssets;

		[SerializeField]
		private bool m_PlayOnStart;

		#endregion


		#region Private Properties

		private List<AnimateAssetField> AnimateAssetFields => m_AnimateAssets;

		private AnimateAsset DefaultAnimateAsset => AnimateAssetFields.Count > 0 ? AnimateAssetFields[0].Object : null;

		private bool PlayOnStart => m_PlayOnStart;

		#endregion


		#region Private Methods

		private AnimateAsset GetAnimateAsset(string assetName) {
			var assetField = AnimateAssetFields.FirstOrDefault(af => af.Object != null && af.Object.name == assetName);
			return assetField?.Object;
		}

		#endregion


	}

}