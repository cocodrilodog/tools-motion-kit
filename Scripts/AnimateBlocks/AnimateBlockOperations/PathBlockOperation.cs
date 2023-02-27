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
	public abstract class PathBlockOperation : AnimateBlockOperation {


		#region Public Methods

		/// <summary>
		/// Finds the block at <see cref="Path"/> and invokes <see cref="PerformOnPathBlock(AnimateBlock, int)"/>
		/// on it.
		/// </summary>
		/// <param name="animateBlock">The <see cref="AnimateBlock"/>.</param>
		/// <param name="index">The index of the <see cref="AnimateBlock"/> when it belongs to a list or array.</param>
		public sealed override AnimateBlock Perform(AnimateBlock animateBlock, int index) {
			if (string.IsNullOrEmpty(Path)) {
				if (animateBlock != null) {
					return PerformOnPathBlock(animateBlock, index);
				}
			} else {
				var childBlock = (animateBlock as IAnimateParent)?.GetChildBlockAtPath(Path);
				if (childBlock != null) {
					return PerformOnPathBlock(childBlock, index);
				}
			}
			return null;
		}

		#endregion


		#region Protected Methods

		/// <summary>
		/// Override this to perform an operation on the <paramref name="pathBlock"/>
		/// </summary>
		/// 
		/// <param name="pathBlock">
		/// A <see cref="AnimateBlock"/> that is located at the <see cref="Path"/> relative to the
		/// <see cref="AnimateBlock"/> provided at <see cref="Perform(AnimateBlock, int)"/>.
		/// If the <see cref="Path"/> is left empty, the action is performed on the <see cref="AnimateBlock"/>
		/// provided at <see cref="Perform(AnimateBlock, int)"/>
		/// </param>
		/// 
		/// <param name="index">The index of the <see cref="AnimateBlock"/> when it belongs to a list or array.</param>
		protected abstract AnimateBlock PerformOnPathBlock(AnimateBlock pathBlock, int index);

		#endregion


		#region Private Fields

		[SerializeField]
		private string m_Path;

		#endregion


		#region Private Properties

		/// <summary>
		/// A relative path to a child block in which the operation will be performed through
		/// <see cref="PerformOnPathBlock(AnimateBlock, int)"/>.
		/// 
		/// If it is left empty, the action is performed on the <see cref="AnimateBlock"/>
		/// provided at <see cref="Perform(AnimateBlock, int)"/>
		/// </summary>
		private string Path => m_Path;

		#endregion


	}

}