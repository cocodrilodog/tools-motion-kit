namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AnimateBehaviour : MonoBehaviour {


		#region Unity Methods

		private void Start() {
			if (PlayOnStart) {
				Motion3DAsset?.GetMotion().Play();
			} else {
				// This avoids errors OnDestroy in case it is not played at all.
				Motion3DAsset?.GetMotion();
			}
		}

		private void OnDestroy() {
			Motion3DAsset?.Clear();
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

		#endregion


	}

}