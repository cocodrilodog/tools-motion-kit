namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Renames the <see cref="PlaybackBlock"/> at <see cref="MotionKitBatchOperationPath.Path"/>
	/// and optionally appends the <c>index</c> + 1 to the <see cref="NewName"/>.
	/// </summary>
	[Serializable]
	public class RenameBlock : MotionKitBatchOperationPath {


		#region Protected Methods

		/// <summary>
		/// Renames the <paramref name="childBlock"/> and if <see cref="AppendSerialNumber"/> is <c>true</c>, appends the 
		/// <c>index</c> + 1 to the <see cref="NewName"/>.
		/// </summary>
		/// <param name="childBlock">The child block located at the <c>Path</c>.</param>
		/// <param name="index">The index of the parent <see cref="PlaybackBlock"/> in the list.</param>
		/// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise</returns>
		protected override bool PerformOnChildBlock(ref PlaybackBlock childBlock, int index) {
			childBlock.Name = NewName + (AppendSerialNumber ? $"{index + 1}" : "");
			return true;
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private string m_NewName;

		[SerializeField]
		private bool m_AppendSerialNumber;

		#endregion


		#region Private Properties

		/// <summary>
		/// The new name for the <see cref="PlaybackBlock"/>.
		/// </summary>
		private string NewName => m_NewName;

		/// <summary>
		/// If checked, appends the <c>index</c> + 1 to the <see cref="NewName"/>.
		/// </summary>
		private bool AppendSerialNumber => m_AppendSerialNumber;

		#endregion


	}

}