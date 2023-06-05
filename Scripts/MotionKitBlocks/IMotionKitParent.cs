namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public interface IMotionKitParent {

		/// <summary>
		/// Gets the <see cref="PlaybackBlock"/> named <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The <see cref="PlaybackBlock.Name"/></param>
		/// <returns>The <see cref="PlaybackBlock"/></returns>
		PlaybackBlock GetChildBlock(string name);

		/// <summary>
		/// Finds a child block at the specified path.
		/// </summary>
		/// <param name="blockPath">The path of the block. For example "Parallel/Sequence1/Motion2D"</param>
		/// <returns>The <see cref="PlaybackBlock"/> if it was found</returns>
		PlaybackBlock GetChildBlockAtPath(string blockPath);

		/// <summary>
		/// Gets all the children <see cref="PlaybackBlock"/>.
		/// </summary>
		/// <returns>An array with the children.</returns>
		PlaybackBlock[] GetChildrenBlocks();

	}

}