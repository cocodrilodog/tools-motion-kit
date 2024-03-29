namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public static class MotionKitBlockUtility {


		#region Public Static Methods


		/// <summary>
		/// Searches recursively for the first motions that modify properties and sets their progress
		/// to 0 so that the value of the property is set to the initial value.
		/// </summary>
		/// 
		/// <remarks>
		/// In <see cref="Parallel"/>s this searches on all parallel items. In <see cref="Sequence"/>s this
		/// searches until it finds the first motion.
		/// </remarks>
		/// 
		/// <param name="motionKitBlock">The animate asset</param>
		/// <returns><c>true</c> if the provided asset is a motion asset and its progress is set to 0</returns>
		public static bool SetInitialValues(MotionKitBlock motionKitBlock) {
			if (motionKitBlock is IMotionBaseBlock) {
				var motionBlock = motionKitBlock as IMotionBaseBlock;
				// The motion is initialized first and its relative values are calculated before applying
				// progress
				motionBlock.Progress = 0;
				return true;
			}
			if (motionKitBlock is SequenceBlock) {
				var sequenceBlock = motionKitBlock as SequenceBlock;
				foreach (var item in sequenceBlock.Items) {
					// Recursion
					if (SetInitialValues(item)) {
						// Break on the first value set on the sequence
						break;
					}
				}
			}
			if (motionKitBlock is ParallelBlock) {
				var parallelBlock = motionKitBlock as ParallelBlock;
				foreach (var item in parallelBlock.Items) {
					// Recursion
					SetInitialValues(item);
				}
			}
			return false;
		}

		#endregion


	}

}
