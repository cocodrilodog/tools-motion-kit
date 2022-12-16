namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AnimateBehaviour : MonoBehaviour {


		#region Public Methods

		public void Play() {
			Motion3DAsset?.GetMotion().Play();
			Motion2DAsset?.GetMotion().Play();
			MotionFloatAsset?.GetMotion().Play();
			MotionColorAsset?.GetMotion().Play();
		}
		
		public void Pause() {
			Motion3DAsset?.GetMotion().Pause();
			Motion2DAsset?.GetMotion().Pause();
			MotionFloatAsset?.GetMotion().Pause();
			MotionColorAsset?.GetMotion().Pause();
		}	
		
		public void Resume() {
			Motion3DAsset?.GetMotion().Resume();
			Motion2DAsset?.GetMotion().Resume();
			MotionFloatAsset?.GetMotion().Resume();
			MotionColorAsset?.GetMotion().Resume();
		}
		
		public void Stop() {
			Motion3DAsset?.GetMotion().Stop();
			Motion2DAsset?.GetMotion().Stop();
			MotionFloatAsset?.GetMotion().Stop();
			MotionColorAsset?.GetMotion().Stop();
		}

		public void Dispose() {
			Motion3DAsset?.Dispose();
			Motion2DAsset?.Dispose();
			MotionFloatAsset?.Dispose();
			MotionColorAsset?.Dispose();
		}

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