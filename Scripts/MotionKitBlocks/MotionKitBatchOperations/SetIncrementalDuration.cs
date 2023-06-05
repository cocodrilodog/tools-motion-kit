namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Sets the duration of the <see cref="MotionKitBlock"/> at <see cref="MotionKitBatchOperationPath.Path"/>
	/// in a incremental way proportional to the <c>index</c> of the block.
	/// </summary>
	[Serializable]
	public class SetIncrementalDuration : MotionKitBatchOperationPath {


		#region Protected Methods

		/// <summary>
		/// Sets the duration of the provided <paramref name="childBlock"/> in a incremental way
		/// proportional to the <paramref name="index"/> of the parent block.
		/// </summary>
		/// <param name="childBlock">The child block located at the <c>Path</c>.</param>
		/// <param name="index">The index of the parent <see cref="MotionKitBlock"/> in the list.</param>
		/// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise</returns>
		protected override bool PerformOnChildBlock(ref MotionKitBlock childBlock, int index) {
			childBlock.DurationInput = BaseDuration + index * DurationIncrement;
			return true;
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