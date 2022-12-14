namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AnimateBehaviour : MonoBehaviour {


		#region Unity Methods

		private void Start() {
			Motion3DAsset motionAsset = (Motion3DAsset)AssetField.Object;
			motionAsset.GetMotion();
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private AnimateAssetField m_AssetField;

		#endregion


		#region Private Properties

		private AnimateAssetField AssetField => m_AssetField;

		#endregion


	}

}