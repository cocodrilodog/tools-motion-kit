namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// Base block for <see cref="SequenceBlock"/> and <see cref="ParallelBlock"/>.
	/// </summary>
	[Serializable]
	public abstract class CompoundBlock : AnimateBlock {


		#region Public Properties

		/// <summary>
		/// The items of this <see cref="CompoundBlock"/>.
		/// </summary>
		public abstract List<AnimateBlock> Items { get; }

		#endregion


#if UNITY_EDITOR

		#region Public Methods

		/// <summary>
		/// Performs all the batch operations of this <see cref="CompoundBlock"/>.
		/// </summary>
		/// <remarks>
		/// This is used only by <see cref="Animate"/> the editor tools.
		/// </remarks>
		public void PerformAllBatchOperations() { 
			for(int i = 0; i < BatchOperations.Count; i++) {
				PerformBatchOperation(i);
			}
		}

		/// <summary>
		/// Performs the batch operation at <paramref name="index"/>
		/// </summary>
		/// <param name="index">The indexx of the operation</param>
		public void PerformBatchOperation(int index) {
			var operation = BatchOperations[index];
			for (int i = 0; i < Items.Count; i++) {
				Items[i] = operation.Perform(Items[i], i);
			}
		}

		#endregion


		#region Protected Properties

		/// <summary>
		/// The list of batch operations to be performed by this <see cref="CompoundBlock"/>.
		/// </summary>
		protected abstract List<AnimateBatchOperation> BatchOperations { get; }

		#endregion

#endif


	}

}