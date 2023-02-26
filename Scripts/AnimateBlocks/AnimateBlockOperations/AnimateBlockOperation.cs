namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class AnimateBlockOperation : CompositeObject {


		#region Public Methods

		public virtual void Perform(AnimateBlock animateBlock, int index) { }

		#endregion


	}

}