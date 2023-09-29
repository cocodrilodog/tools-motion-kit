namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Renames the <see cref="MotionKitBlock"/> at <see cref="MotionKitBatchOperationPath.Path"/>
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
		/// <param name="index">The index of the parent <see cref="MotionKitBlock"/> in the list.</param>
		/// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise</returns>
		protected override bool PerformOnChildBlock(ref MotionKitBlock childBlock, int index) {
			childBlock.Name = NewName + (AppendSerialNumber ? $"{index + 1}" : "");
			return true;
		}

		#endregion


		#region Private Fields

		[Tooltip("The new name that the block at the specified path will have")]
		[SerializeField]
		private string m_NewName;

		[SerializeField]
		private bool m_AppendSerialNumber;

		#endregion


		#region Private Properties

		/// <summary>
		/// The new name for the <see cref="MotionKitBlock"/>.
		/// </summary>
		private string NewName => m_NewName;

		/// <summary>
		/// If checked, appends the <c>index</c> + 1 to the <see cref="NewName"/>.
		/// </summary>
		private bool AppendSerialNumber => m_AppendSerialNumber;

		#endregion


	}

}