namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AnimateBehaviour : MonoBehaviour {


		#region Unity Methods

		private void Start() {
			if (PlayOnStart) {
				Motion3DAsset?.GetMotion().Play();
				Motion2DAsset?.GetMotion().Play();
				MotionFloatAsset?.GetMotion().Play();
				MotionColorAsset?.GetMotion().Play();
			} else {
				// This avoids errors OnDestroy in case it was not played at all.
				Motion3DAsset?.GetMotion();
				Motion2DAsset?.GetMotion();
				MotionFloatAsset?.GetMotion();
				MotionColorAsset?.GetMotion();
			}
		}

		private void OnDestroy() {
			Motion3DAsset?.Clear();
			MotionFloatAsset?.Clear();
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


	}

}