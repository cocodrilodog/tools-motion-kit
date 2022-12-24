namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// This is the component that will be used to contain and manages <see cref="AnimateAsset"/>s.
	/// </summary>
	public class AnimateBehaviour : MonoBehaviour, IMonoScriptableOwner {


		#region Public Properties

		/// <summary>
		/// Gets the <c>Progress</c> of the <see cref="DefaultAnimateAsset"/> managed by this MonoBehavoiur.
		/// </summary>
		public float Progress => (float)DefaultAnimateAsset?.Progress;

		/// <summary>
		/// Gets the <c>CurrentTime</c> of the <see cref="DefaultAnimateAsset"/> managed by this MonoBehavoiur.
		/// </summary>
		public float CurrentTime => (float)DefaultAnimateAsset?.CurrentTime;

		/// <summary>
		/// Gets the <c>Duration</c> of the <see cref="DefaultAnimateAsset"/> managed by this MonoBehavoiur.
		/// </summary>
		public float Duration => (float)DefaultAnimateAsset?.Duration;

		/// <summary>
		/// Gets the <c>IsPlaying</c> property of the <see cref="DefaultAnimateAsset"/> managed by this MonoBehavoiur.
		/// </summary>
		public bool IsPlaying => (bool)DefaultAnimateAsset?.IsPlaying;

		/// <summary>
		/// Gets the <c>IsPaused</c> property of the <see cref="DefaultAnimateAsset"/> managed by this MonoBehavoiur.
		/// </summary>
		public bool IsPaused => (bool)DefaultAnimateAsset?.IsPaused;

		/// <summary>
		/// The first <see cref="AnimateAsset"/> of this <see cref="AnimateBehaviour"/> if any, or <c>null</c>.
		/// </summary>
		public AnimateAsset DefaultAnimateAsset => AnimateAssetFields.Count > 0 ? AnimateAssetFields[0].Object : null;

		#endregion


		#region Public Methods

		/// <summary>
		/// Initializes all the <see cref="AnimateAsset"/>s of this compopnent.
		/// </summary>
		private void Initialize() {
			foreach (var assetField in AnimateAssetFields) {
				assetField.Object?.Initialize();
			}
		}

		/// <summary>
		/// Plays the <see cref="DefaultAnimateAsset"/>.
		/// </summary>
		public void Play() => DefaultAnimateAsset?.Play();

		/// <summary>
		/// Plays the <see cref="AnimateAsset"/> with the specified <paramref name="assetName"/>.
		/// </summary>
		/// <param name="assetName">The name of the asset.</param>
		public void Play(string assetName) => GetAnimateAsset(assetName)?.Play();

		/// <summary>
		/// Stops the <see cref="DefaultAnimateAsset"/>.
		/// </summary>
		public void Stop() => DefaultAnimateAsset?.Stop();

		/// <summary>
		/// Stops the <see cref="AnimateAsset"/> with the specified <paramref name="assetName"/>.
		/// </summary>
		/// <param name="assetName">The name of the asset.</param>
		public void Stop(string assetName) => GetAnimateAsset(assetName)?.Stop();

		/// <summary>
		/// Pauses the <see cref="DefaultAnimateAsset"/>.
		/// </summary>
		public void Pause() => DefaultAnimateAsset?.Pause();

		/// <summary>
		/// Pauses the <see cref="AnimateAsset"/> with the specified <paramref name="assetName"/>.
		/// </summary>
		/// <param name="assetName">The name of the asset.</param>
		public void Pause(string assetName) => GetAnimateAsset(assetName)?.Pause();

		/// <summary>
		/// Resumes the <see cref="DefaultAnimateAsset"/>.
		/// </summary>
		public void Resume() => DefaultAnimateAsset?.Resume();

		/// <summary>
		/// Resumes the <see cref="AnimateAsset"/> with the specified <paramref name="assetName"/>.
		/// </summary>
		/// <param name="assetName">The name of the asset.</param>
		public void Resume(string assetName) => GetAnimateAsset(assetName)?.Resume();

		/// <summary>
		/// Disposes all the <see cref="AnimateAsset"/>s of this compopnent.
		/// </summary>
		public void Dispose() {
			foreach (var asset in AnimateAssetFields) {
				asset.Object?.Dispose();
			}
		}

		public void RecreateMonoScriptableObjects() {
			MonoScriptableUtility.RecreateMonoScriptableObjects(AnimateAssetFields.ToArray(), this);
		}

		public void RecreateRepeatedMonoScriptableArrayOrListItems() {
			MonoScriptableUtility.RecreateRepeatedMonoScriptableArrayOrListItems(AnimateAssetFields.ToArray(), this);
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