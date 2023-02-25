namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public interface IAnimateParent {

		/// <summary>
		/// Gets the <see cref="AnimateBlock"/> named <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The <see cref="AnimateBlock.Name"/></param>
		/// <returns>The <see cref="AnimateBlock"/></returns>
		AnimateBlock GetChildBlock(string name);

		/// <summary>
		/// Finds a child block at the specified path.
		/// </summary>
		/// <param name="blockPath">The path of the block. For example "Parallel/Sequence1/Motion2D"</param>
		/// <returns>The <see cref="AnimateBlock"/> if it was found</returns>
		AnimateBlock GetChildBlockAtPath(string blockPath);

		/// <summary>
		/// Gets all the children <see cref="AnimateBlock"/>.
		/// </summary>
		/// <returns>An array with the children.</returns>
		AnimateBlock[] GetChildrenBlocks();

	}

}