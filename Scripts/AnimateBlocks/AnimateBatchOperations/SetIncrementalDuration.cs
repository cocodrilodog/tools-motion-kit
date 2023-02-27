namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Sets the duration of the <see cref="AnimateBlock"/> at <see cref="AnimateBatchOperationPath.Path"/>
	/// in a incremental way proportional to the <c>index</c> of the block.
	/// </summary>
	[Serializable]
	public class SetIncrementalDuration : AnimateBatchOperationPath {


		#region Protected Methods

		/// <summary>
		/// Sets the duration of the provided <paramref name="animateBlock"/> in a incremental way
		/// proportional to the <paramref name="index"/> of the block.
		/// </summary>
		/// <param name="childBlock">The child block located at the <c>Path</c>.</param>
		/// <param name="index">The index of the parent <see cref="AnimateBlock"/> in the list.</param>
		protected override void PerformOnChildBlock(AnimateBlock childBlock, int index) {
			childBlock.DurationInput = BaseDuration + index * DurationIncrement;
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private float m_BaseDuration;

		[SerializeField]
		private float m_DurationIncrement;

		#endregion


		#region Private Properties

		/// <summary>
		/// The duration of the first element in the list.
		/// </summary>
		private float BaseDuration => m_BaseDuration;

		/// <summary>
		/// The increment of the duration per element in the list.
		/// </summary>
		private float DurationIncrement => m_DurationIncrement;

		#endregion


	}

}