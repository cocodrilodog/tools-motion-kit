namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public static class AnimateBlocksUtility {


		#region Public Static Methods

		/// <summary>
		/// Searches recursively for the first motion that can modify a property and sets its progress
		/// to 0 so that the value of the property is set to the initial value.
		/// </summary>
		/// <param name="animateBlock">The animate asset</param>
		/// <returns><c>true</c> if the provided asset is a motion asset and its progress is set to 0</returns>
		public static bool SetInitialValue(AnimateBlock animateBlock) {
			if (animateBlock is IMotionBlock) {
				var motionBlock = animateBlock as IMotionBlock;
				motionBlock.Progress = 0;
				return true;
			}
			if (animateBlock is SequenceBlock) {
				var sequenceBlock = animateBlock as SequenceBlock;
				foreach (var item in sequenceBlock.SequenceItems) {
					// Recursion
					if (SetInitialValue(item)) {
						// Break on the first value set on the sequence
						break;
					}
				}
			}
			if (animateBlock is ParallelBlock) {
				var parallelBlock = animateBlock as ParallelBlock;
				foreach (var item in parallelBlock.ParallelItems) {
					// Recursion
					SetInitialValue(item);
				}
			}
			return false;
		}

		#endregion


	}

}
