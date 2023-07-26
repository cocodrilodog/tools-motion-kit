namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// Base block for <see cref="SequenceBlock"/> and <see cref="ParallelBlock"/>.
	/// </summary>
	/// <remarks>
	/// This class was created to handle the batch operations and other functions that
	/// are common to both classes.
	/// </remarks>
	[Serializable]
	public abstract class CompoundBlock : MotionKitBlock {


		#region Public Properties

		/// <summary>
		/// The items of this <see cref="CompoundBlock"/>.
		/// </summary>
		public abstract ReadOnlyCollection<MotionKitBlock> Items { get; }

		#endregion


		#region Public Methods

		public override void TryResetPlayback(bool recursive) {
			base.TryResetPlayback(recursive);
			// TryReset recursively
			if (recursive) {
				foreach (var item in Items) {
					item.TryResetPlayback(recursive);
				}
			}
		}

		public override void LockResetPlayback(bool recursive) {
			base.LockResetPlayback(recursive);
			if (recursive) { 
				foreach (var item in Items) {
					item.LockResetPlayback(recursive);
				}
			}
		}

		public override void UnlockResetPlayback(bool recursive) {
			base.UnlockResetPlayback(recursive);
			if (recursive) {
				foreach (var item in Items) {
					item.UnlockResetPlayback(recursive);
				}
			}
		}

		/// <summary>
		/// Sets the item at he specified index.
		/// </summary>
		/// <param name="index">The index</param>
		/// <param name="block">The <see cref="MotionKitBlock"/> to set</param>
		public abstract void SetItem(int index, MotionKitBlock block);

		/// <summary>
		/// Gets the batch operation at the specified <paramref name="index"/>
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public MotionKitBatchOperation GetBatchOperation(int index) => BatchOperations[index];

		/// <summary>
		/// Performs all the batch operations of this <see cref="CompoundBlock"/>.
		/// </summary>
		/// <remarks>
		/// This is used only by <see cref="MotionKit"/> the editor tools.
		/// </remarks>
		/// <returns>An array of the number of successful operations per batch operation</returns>
		public int[] PerformAllBatchOperations() {
			var successful = new int[BatchOperations.Count];
			for(int i = 0; i < BatchOperations.Count; i++) {
				successful[i] = PerformBatchOperation(i);
			}
			return successful;
		}

		/// <summary>
		/// Performs the batch operation at <paramref name="index"/>
		/// </summary>
		/// <param name="index">The indexx of the operation</param>
		/// <returns>The number of successful operations</returns>
		public int PerformBatchOperation(int index) {
			var operation = BatchOperations[index];
			var successful = 0;
			for (int i = 0; i < Items.Count; i++) {
				// Get the item for it to be valid as ref
				var item = Items[i];
				// Perform the operation
				if (operation.Perform(ref item, i)) {
					successful++;
				}
				// After the operation, reasssign the item, in case it was renewed
				// It may be the same one or a new one, depending on the operation
				SetItem(i, item);
			}
			return successful;
		}

		#endregion


		#region Protected Properties

		/// <summary>
		/// The list of batch operations to be performed by this <see cref="CompoundBlock"/>.
		/// </summary>
		protected abstract List<MotionKitBatchOperation> BatchOperations { get; }

		#endregion


	}

}