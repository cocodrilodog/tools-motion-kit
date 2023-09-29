namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Sets the values of the <see cref="Motion3DBlock"/> at <see cref="MotionKitBatchOperationPath.Path"/>
	/// in a incremental way proportional to the <c>index</c> of the block.
	/// </summary>
	[Serializable]
	public class SetIncrementalValues3D : SetIncrementalValuesBase<Vector3> {


		#region Protected Methods

		/// <summary>
		/// Sets the values of the provided <paramref name="childBlock"/> in a incremental way
		/// proportional to the <paramref name="index"/> of the parent block.
		/// </summary>
		/// <param name="childBlock">The child block located at the <c>Path</c>.</param>
		/// <param name="index">The index of the parent <see cref="MotionKitBlock"/> in the list.</param>
		/// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise</returns>
		protected override bool PerformOnChildBlock(ref MotionKitBlock childBlock, int index) {
			var motion3DBlock = childBlock as Motion3DBlock;
			if (motion3DBlock != null) {
				motion3DBlock.InitialValue = BaseInitialValue + index * InitialValueIncrement;
				motion3DBlock.FinalValue = BaseFinalValue + index * FinalValueIncrement;
				return true;
			} else {
				return false;
			}
		}

		#endregion


	}

}