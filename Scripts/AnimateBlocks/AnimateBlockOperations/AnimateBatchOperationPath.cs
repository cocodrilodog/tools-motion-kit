namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Base class for operations that will be performed on <see cref="AnimateBlock"/>s that are 
	/// children of the <see cref="AnimateBlock"/> provided at <see cref="Perform(AnimateBlock, int)"/>.
	/// To specify the child, you need to set <see cref="Path"/>.
	/// </summary>
	[Serializable]
	public abstract class AnimateBatchOperationPath : AnimateBatchOperation {


		#region Public Methods

		/// <summary>
		/// Finds the block at <see cref="Path"/> and invokes <see cref="PerformOnChildBlock(AnimateBlock, int)"/>
		/// on it.
		/// </summary>
		/// <param name="animateBlock">Each <see cref="AnimateBlock"/> in the list.</param>
		/// <param name="index">The index of the <see cref="AnimateBlock"/> in the list.</param>
		/// <returns>The modified <paramref name="animateBlock"/></returns>
		public sealed override AnimateBlock Perform(AnimateBlock animateBlock, int index) {
			if (string.IsNullOrEmpty(Path)) {
				if (animateBlock != null) {
					PerformOnChildBlock(animateBlock, index);
				}
			} else {
				var childBlock = (animateBlock as IAnimateParent)?.GetChildBlockAtPath(Path);
				if (childBlock != null) {
					PerformOnChildBlock(childBlock, index);
				}
			}
			return animateBlock;
		}

		#endregion


		#region Protected Properties

		/// <summary>
		/// A relative path to a child block in which the operation will be performed through
		/// <see cref="PerformOnChildBlock(AnimateBlock, int)"/>.
		/// 
		/// If it is left empty, the action is performed on the <see cref="AnimateBlock"/>
		/// provided at <see cref="Perform(AnimateBlock, int)"/>
		/// </summary>
		protected string Path => m_Path;

		#endregion


		#region Protected Methods

		/// <summary>
		/// Override this to perform an operation on the <paramref name="childBlock"/>
		/// </summary>
		/// 
		/// <param name="childBlock">
		/// A <see cref="AnimateBlock"/> that is located at the <see cref="Path"/> relative to the
		/// <see cref="AnimateBlock"/> provided at <see cref="Perform(AnimateBlock, int)"/>.
		/// If the <see cref="Path"/> is left empty, the action is performed on the <see cref="AnimateBlock"/>
		/// provided at <see cref="Perform(AnimateBlock, int)"/>
		/// </param>
		/// 
		/// <param name="index">The index of the <see cref="AnimateBlock"/> when it belongs to a list or array.</param>
		protected abstract void PerformOnChildBlock(AnimateBlock childBlock, int index);

		#endregion


		#region Private Fields

		[SerializeField]
		private string m_Path;

		#endregion


	}

}