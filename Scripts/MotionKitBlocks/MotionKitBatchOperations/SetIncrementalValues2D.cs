namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Sets the values of the <see cref="Motion2DBlock"/> at <see cref="MotionKitBatchOperationPath.Path"/>
	/// in a incremental way proportional to the <c>index</c> of the block.
	/// </summary>
	[Serializable]
	public class SetIncrementalValues2D : SetIncrementalValuesBase<Vector2> {


		#region Protected Methods

		/// <summary>
		/// Sets the values of the provided <paramref name="childBlock"/> in a incremental way
		/// proportional to the <paramref name="index"/> of the parent block.
		/// </summary>
		/// <param name="childBlock">The child block located at the <c>Path</c>.</param>
		/// <param name="index">The index of the parent <see cref="MotionKitBlock"/> in the list.</param>
		/// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise</returns>
		protected override bool PerformOnChildBlock(ref MotionKitBlock childBlock, int index) {
			var motion2DBlock = childBlock as Motion2DBlock;
			if (motion2DBlock != null) {
				motion2DBlock.InitialValue = BaseInitialValue + index * InitialValueIncrement;
				motion2DBlock.FinalValue = BaseFinalValue + index * FinalValueIncrement;
				return true;
			} else {
				return false;
			}
		}

		#endregion


	}

}