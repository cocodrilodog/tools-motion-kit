namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// An operation to be performed on a <see cref="AnimateBlock"/>.
	/// </summary>
	/// <remarks>
	/// This is used only by <see cref="Animate"/> the editor tools.
	/// </remarks>
	[Serializable]
	public abstract class AnimateBlockOperation : CompositeObject {


		#region Public Methods

		/// <summary>
		/// Override this to perform an operation on the provided <paramref name="animateBlock"/>
		/// </summary>
		/// <param name="animateBlock">The <see cref="AnimateBlock"/>.</param>
		/// <param name="index">The index of the <see cref="AnimateBlock"/> when it belongs to a list or array.</param>
		public abstract AnimateBlock Perform(AnimateBlock animateBlock, int index);

		#endregion


	}

}