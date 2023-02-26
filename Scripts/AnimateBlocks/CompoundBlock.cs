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
	public abstract class CompoundBlock : AnimateBlock {


		#region Public Properties

		public abstract List<AnimateBlock> Items { get; }

		#endregion


		#region Public Methods

		public void PerformBatchOperations() { 
			foreach(var operation in BatchOperations) {
				for(int i = 0; i < Items.Count; i++) {
					operation.Perform(Items[i], i);
				}
			}
		}

		#endregion


		#region Protected Properties

		protected abstract List<AnimateBlockOperation> BatchOperations { get; }

		#endregion


	}

}