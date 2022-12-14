namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AnimateAsset : MonoScriptableObject {


		#region Public Methods
		
		public void Clear() {
			Animate.ClearPlaybacks(this);
		}

		#endregion


	}

}