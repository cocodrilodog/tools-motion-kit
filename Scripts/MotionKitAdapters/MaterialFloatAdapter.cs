namespace CocodriloDog.MotionKit {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class MaterialFloatAdapter : MaterialPropertyAdapter<float> {


		#region Protected Methods

		protected override void SetPropertyValue(MaterialPropertyBlock materialPB, string property, float value) {
			materialPB.SetFloat(property, value);
		}

		#endregion


	}

}
