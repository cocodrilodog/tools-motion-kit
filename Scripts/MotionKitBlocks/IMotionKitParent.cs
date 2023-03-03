namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public interface IMotionKitParent {

		/// <summary>
		/// Gets the <see cref="MotionKitBlock"/> named <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The <see cref="MotionKitBlock.Name"/></param>
		/// <returns>The <see cref="MotionKitBlock"/></returns>
		MotionKitBlock GetChildBlock(string name);

		/// <summary>
		/// Finds a child block at the specified path.
		/// </summary>
		/// <param name="blockPath">The path of the block. For example "Parallel/Sequence1/Motion2D"</param>
		/// <returns>The <see cref="MotionKitBlock"/> if it was found</returns>
		MotionKitBlock GetChildBlockAtPath(string blockPath);

		/// <summary>
		/// Gets all the children <see cref="MotionKitBlock"/>.
		/// </summary>
		/// <returns>An array with the children.</returns>
		MotionKitBlock[] GetChildrenBlocks();

	}

}