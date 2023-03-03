namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Base class for operations that will be performed on <see cref="MotionKitBlock"/>s that are 
	/// children of the <see cref="MotionKitBlock"/> provided at <see cref="Perform(MotionKitBlock, int)"/>.
	/// To specify the child, you need to set <see cref="Path"/>.
	/// </summary>
	[Serializable]
	public abstract class MotionKitBatchOperationPath : MotionKitBatchOperation {


		#region Public Methods

		/// <summary>
		/// Finds the block at <see cref="Path"/> and invokes <see cref="PerformOnChildBlock(MotionKitBlock, int)"/>
		/// on it.
		/// </summary>
		/// <param name="animateBlock">Each <see cref="MotionKitBlock"/> in the list.</param>
		/// <param name="index">The index of the <see cref="MotionKitBlock"/> in the list.</param>
		/// <returns>The modified <paramref name="animateBlock"/></returns>
		public sealed override MotionKitBlock Perform(MotionKitBlock animateBlock, int index) {
			base.Perform(animateBlock, index);
			if (string.IsNullOrEmpty(Path)) {
				if (animateBlock != null) {
					PerformOnChildBlock(animateBlock, index);
				}
			} else {
				var childBlock = (animateBlock as IMotionKitParent)?.GetChildBlockAtPath(Path);
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
		/// <see cref="PerformOnChildBlock(MotionKitBlock, int)"/>.
		/// 
		/// If it is left empty, the action is performed on the <see cref="MotionKitBlock"/>
		/// provided at <see cref="Perform(MotionKitBlock, int)"/>
		/// </summary>
		protected string Path => m_Path;

		#endregion


		#region Protected Methods

		/// <summary>
		/// Override this to perform an operation on the <paramref name="childBlock"/>
		/// </summary>
		/// 
		/// <param name="childBlock">
		/// A <see cref="MotionKitBlock"/> that is located at the <see cref="Path"/> relative to the
		/// <see cref="MotionKitBlock"/> provided at <see cref="Perform(MotionKitBlock, int)"/>.
		/// If the <see cref="Path"/> is left empty, the action is performed on the <see cref="MotionKitBlock"/>
		/// provided at <see cref="Perform(MotionKitBlock, int)"/>
		/// </param>
		/// 
		/// <param name="index">The index of the <see cref="MotionKitBlock"/> when it belongs to a list or array.</param>
		protected abstract void PerformOnChildBlock(MotionKitBlock childBlock, int index);

		#endregion


		#region Private Fields

		[SerializeField]
		private string m_Path;

		#endregion


	}

}