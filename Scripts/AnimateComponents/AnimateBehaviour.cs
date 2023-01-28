namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// This is the component that will be used to contain and manage <see cref="AnimateComponent"/>s.
	/// </summary>
	public class AnimateBehaviour : MonoScriptableRoot {


		#region Public Properties

		/// <summary>
		/// Gets the <c>Progress</c> of the <see cref="DefaultAnimateComponent"/> managed by this MonoBehavoiur.
		/// </summary>
		public float Progress {
			get => (float)DefaultAnimateComponent?.Progress;
			set {
				if (DefaultAnimateComponent != null) {
					DefaultAnimateComponent.Progress = value;
				}
			}
		}

		/// <summary>
		/// Gets the <c>CurrentTime</c> of the <see cref="DefaultAnimateComponent"/> managed by this MonoBehavoiur.
		/// </summary>
		public float CurrentTime { 
			get {
				if (DefaultAnimateComponent != null) {
					return DefaultAnimateComponent.CurrentTime;
				} else {
					return 0;
				}
			} 
		}

		/// <summary>
		/// Gets the <c>Duration</c> of the <see cref="DefaultAnimateComponent"/> managed by this MonoBehavoiur.
		/// </summary>
		public float Duration {
			get {
				if (DefaultAnimateComponent != null) {
					return DefaultAnimateComponent.Duration;
				} else {
					return 0;
				}
			}
		}

		/// <summary>
		/// Gets the <c>IsPlaying</c> property of the <see cref="DefaultAnimateComponent"/> managed by this MonoBehavoiur.
		/// </summary>
		public bool IsPlaying {
			get {
				if (DefaultAnimateComponent != null) {
					return DefaultAnimateComponent.IsPlaying;
				} else {
					return false;
				}
			}
		}

		/// <summary>
		/// Gets the <c>IsPaused</c> property of the <see cref="DefaultAnimateComponent"/> managed by this MonoBehavoiur.
		/// </summary>
		public bool IsPaused {
			get {
				if (DefaultAnimateComponent != null) {
					return DefaultAnimateComponent.IsPaused;
				} else {
					return false;
				}
			}
		}

		/// <summary>
		/// The first <see cref="AnimateComponent"/> of this <see cref="AnimateBehaviour"/> if any, or <c>null</c>.
		/// </summary>
		public AnimateComponent DefaultAnimateComponent => AnimateAssetFields.Count > 0 ? AnimateAssetFields[0].Object : null;

		#endregion


		#region Public Methods

		/// <summary>
		/// Initializes all the <see cref="AnimateComponent"/>s of this compopnent.
		/// </summary>
		private void Initialize() => AnimateAssetFields.ForEach(af => {
			if (af.Object != null) {
				af.Object.Initialize();
			}
		});

		/// <summary>
		/// Plays the <see cref="DefaultAnimateComponent"/>.
		/// </summary>
		public void Play() => DefaultAnimateComponent.Play();

		/// <summary>
		/// Plays the <see cref="AnimateComponent"/> with the specified <paramref name="assetName"/>.
		/// </summary>
		/// <param name="assetName">The name of the asset.</param>
		public void Play(string assetName) => GetAnimateAsset(assetName)?.Play();

		/// <summary>
		/// Plays all the <see cref="AnimateComponent"/>s managed by this behaviour.
		/// </summary>
		public void PlayAll() => AnimateAssetFields.ForEach(af => {
			if (af.Object != null) {
				af.Object.Play();
			}
		});

		/// <summary>
		/// Stops the <see cref="DefaultAnimateComponent"/>.
		/// </summary>
		public void Stop() => DefaultAnimateComponent?.Stop();

		/// <summary>
		/// Stops the <see cref="AnimateComponent"/> with the specified <paramref name="assetName"/>.
		/// </summary>
		/// <param name="assetName">The name of the asset.</param>
		public void Stop(string assetName) => GetAnimateAsset(assetName)?.Stop();

		/// <summary>
		/// Stops all the <see cref="AnimateComponent"/>s managed by this behaviour.
		/// </summary>
		public void StopAll() => AnimateAssetFields.ForEach(af => {
			if (af.Object != null) {
				af.Object.Stop();
			}
		});

		/// <summary>
		/// Pauses the <see cref="DefaultAnimateComponent"/>.
		/// </summary>
		public void Pause() => DefaultAnimateComponent?.Pause();

		/// <summary>
		/// Pauses the <see cref="AnimateComponent"/> with the specified <paramref name="assetName"/>.
		/// </summary>
		/// <param name="assetName">The name of the asset.</param>
		public void Pause(string assetName) => GetAnimateAsset(assetName)?.Pause();

		/// <summary>
		/// Pauses all the <see cref="AnimateComponent"/>s managed by this behaviour.
		/// </summary>
		public void PauseAll() => AnimateAssetFields.ForEach(af => af.Object?.Pause());

		/// <summary>
		/// Resumes the <see cref="DefaultAnimateComponent"/>.
		/// </summary>
		public void Resume() => DefaultAnimateComponent?.Resume();

		/// <summary>
		/// Resumes the <see cref="AnimateComponent"/> with the specified <paramref name="assetName"/>.
		/// </summary>
		/// <param name="assetName">The name of the asset.</param>
		public void Resume(string assetName) => GetAnimateAsset(assetName)?.Resume();

		/// <summary>
		/// Resumes all the <see cref="AnimateComponent"/>s managed by this behaviour.
		/// </summary>
		public void ResumeAll() => AnimateAssetFields.ForEach(af => af.Object?.Resume());

		/// <summary>
		/// Calls <see cref="IMotionBaseComponent.ResetMotion()"/> and all the motions that it finds.
		/// </summary>
		public void ResetAllMotions() => AnimateAssetFields.ForEach(af => ResetMotion(af.Object));

		/// <summary>
		/// Disposes all the <see cref="AnimateComponent"/>s of this compopnent.
		/// </summary>
		public void Dispose() => AnimateAssetFields.ForEach(af => af.Object?.Dispose());

		public override MonoScriptableFieldBase[] GetMonoScriptableFields() {
			// HACK: This
			var list = new List<MonoScriptableFieldBase>();
			list.AddRange(AnimateAssetFields);
			list.AddRange(AnimateComponentFields);
			return list.ToArray();
		}

		public override void ConfirmOwnership() {
			foreach (var field in GetMonoScriptableFields()) {
				if (field.ObjectBase != null) {
					field.ObjectBase.SetOwner(this);
				}
			}
		}

		#endregion


		#region Unity Methods

		private void Start() {
			if (PlayAllOnStart) {
				PlayAll();
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
		/// A list of <see cref="AnimateComponentField"/>.
		/// </summary>
		/// <remarks>
		/// This name is friendlier than m_AnimateAssetFields in the inspector.
		/// </remarks>
		[SerializeField]
		private List<AnimateComponentField> m_AnimateAssets = new List<AnimateComponentField>();

		[SerializeField]
		private List<AnimateComponentField> m_AnimateComponents = new List<AnimateComponentField>();

		[SerializeField]
		private bool m_PlayAllOnStart;

		#endregion


		#region Private Properties

		private List<AnimateComponentField> AnimateAssetFields => m_AnimateAssets;

		private List<AnimateComponentField> AnimateComponentFields => m_AnimateAssets;

		private bool PlayAllOnStart => m_PlayAllOnStart;

		#endregion


		#region Private Methods

		private AnimateComponent GetAnimateAsset(string assetName) {
			var assetField = AnimateAssetFields.FirstOrDefault(af => af.Object != null && af.Object.ObjectName == assetName);
			return assetField?.Object;
		}

		private void ResetMotion(AnimateComponent animateComponent) {
			if (animateComponent is IMotionBaseComponent) {
				var motionComponent = animateComponent as IMotionBaseComponent;
				motionComponent.ResetMotion();
			}
			if (animateComponent is SequenceComponent) {
				var sequenceAsset = animateComponent as SequenceComponent;
				foreach (var itemField in sequenceAsset.SequenceItemsFields) {
					// Recursion
					ResetMotion(itemField.Object);
				}
			}
			if (animateComponent is ParallelAsset) {
				var parallelAsset = animateComponent as ParallelAsset;
				foreach (var itemField in parallelAsset.ParallelItemsFields) {
					// Recursion
					ResetMotion(itemField.Object);
				}
			}
		}

		#endregion


	}

}