namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Sets the duration of the provided <see cref="AnimateBlock"/> in a incremental way
	/// proportional to the <c>index</c> of the block.
	/// </summary>
	[Serializable]
	public class IncrementalDuration : PathBlockOperation {


		#region Protected Methods

		/// <summary>
		/// Sets the duration of the provided <paramref name="animateBlock"/> in a incremental way
		/// proportional to the <paramref name="index"/> of the block.
		/// </summary>
		/// <param name="animateBlock">The <see cref="AnimateBlock"/>.</param>
		/// <param name="index">The index of the <see cref="AnimateBlock"/> when it belongs to a list or array.</param>
		protected override void PerformOnPathBlock(AnimateBlock pathBlock, int index) {
			pathBlock.DurationInput = BaseDuration + index * DurationIncrement;
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private float m_BaseDuration;

		[SerializeField]
		private float m_DurationIncrement;

		#endregion


		#region Private Properties

		private float BaseDuration => m_BaseDuration;

		private float DurationIncrement => m_DurationIncrement;

		#endregion


	}

}