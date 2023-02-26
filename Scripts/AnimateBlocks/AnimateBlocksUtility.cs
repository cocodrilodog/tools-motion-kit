namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public static class AnimateBlocksUtility {


		#region Public Static Methods


		/// <summary>
		/// Finds a child block at the specified path.
		/// </summary>
		/// <param name="parent">The <see cref="IAnimateParent"/> where the search starts</param>
		/// <param name="blockPath">The path of the block. For example "Parallel/Sequence1/Motion2D"</param>
		/// <returns>The <see cref="AnimateBlock"/> if it was found</returns>
		public static AnimateBlock GetChildBlockAtPath(IAnimateParent parent, string blockPath) {
			var pathParts = blockPath.Split('/');			
			AnimateBlock block = null;
			for (int i = 0; i < pathParts.Length; i++) {
				block = parent.GetChildBlock(pathParts[i]);
				if (block is IAnimateParent) {
					parent = (block as IAnimateParent);
				} else {
					break;
				}
			}
			return block;
		}

		/// <summary>
		/// Searches recursively for the first motion that can modify a property and sets its progress
		/// to 0 so that the value of the property is set to the initial value.
		/// </summary>
		/// <param name="animateBlock">The animate asset</param>
		/// <returns><c>true</c> if the provided asset is a motion asset and its progress is set to 0</returns>
		public static bool SetInitialValue(AnimateBlock animateBlock) {
			if (animateBlock is IMotionBlock) {
				var motionBlock = animateBlock as IMotionBlock;
				// The motion is initialized first and its relative values are calculated before applying
				// progress
				motionBlock.Progress = 0;
				// This will prevent the OnStart callback to realculate the relative values when the object
				// has moved from its original position.
				motionBlock.DontResetRelativeValuesOnStart();
				return true;
			}
			if (animateBlock is SequenceBlock) {
				var sequenceBlock = animateBlock as SequenceBlock;
				foreach (var item in sequenceBlock.Items) {
					// Recursion
					if (SetInitialValue(item)) {
						// Break on the first value set on the sequence
						break;
					}
				}
			}
			if (animateBlock is ParallelBlock) {
				var parallelBlock = animateBlock as ParallelBlock;
				foreach (var item in parallelBlock.Items) {
					// Recursion
					SetInitialValue(item);
				}
			}
			return false;
		}

		#endregion


	}

}
