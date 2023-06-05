namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Base class for operations that will be performed on <see cref="PlaybackBlock"/>s that are 
	/// children of the <see cref="PlaybackBlock"/> provided at <see cref="Perform(PlaybackBlock, int)"/>.
	/// To specify the child, you need to set <see cref="Path"/>.
	/// </summary>
	[Serializable]
	public abstract class MotionKitBatchOperationPath : MotionKitBatchOperation {


		#region Public Methods

		/// <summary>
		/// Finds the block at <see cref="Path"/> and invokes <see cref="PerformOnChildBlock(PlaybackBlock, int)"/>
		/// on it.
		/// </summary>
		/// <param name="motionKitBlock">Each <see cref="PlaybackBlock"/> in the list.</param>
		/// <param name="index">The index of the <see cref="PlaybackBlock"/> in the list.</param>
		/// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise</returns>
		public sealed override bool Perform(ref PlaybackBlock motionKitBlock, int index) {
			base.Perform(ref motionKitBlock, index);
			if (string.IsNullOrEmpty(Path)) {
				if (motionKitBlock != null) {
					return PerformOnChildBlock(ref motionKitBlock, index);
				}
			} else {
				var childBlock = (motionKitBlock as IMotionKitParent)?.GetChildBlockAtPath(Path);
				if (childBlock != null) {
					return PerformOnChildBlock(ref childBlock, index);
				}
			}
			return false;
		}

		#endregion


		#region Protected Properties

		/// <summary>
		/// A relative path to a child block in which the operation will be performed through
		/// <see cref="PerformOnChildBlock(PlaybackBlock, int)"/>.
		/// 
		/// If it is left empty, the action is performed on the <see cref="PlaybackBlock"/>
		/// provided at <see cref="Perform(PlaybackBlock, int)"/>
		/// </summary>
		protected string Path => m_Path;

		#endregion


		#region Protected Methods

		/// <summary>
		/// Override this to perform an operation on the <paramref name="childBlock"/>
		/// </summary>
		/// 
		/// <param name="childBlock">
		/// A <see cref="PlaybackBlock"/> that is located at the <see cref="Path"/> relative to the
		/// <see cref="PlaybackBlock"/> provided at <see cref="Perform(PlaybackBlock, int)"/>.
		/// If the <see cref="Path"/> is left empty, the action is performed on the <see cref="PlaybackBlock"/>
		/// provided at <see cref="Perform(PlaybackBlock, int)"/>
		/// </param>
		/// 
		/// <param name="index">The index of the <see cref="PlaybackBlock"/> when it belongs to a list or array.</param>
		/// 
		/// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise</returns>
		protected abstract bool PerformOnChildBlock(ref PlaybackBlock childBlock, int index);

		#endregion


		#region Private Fields

		[SerializeField]
		private string m_Path;

		#endregion


	}

}