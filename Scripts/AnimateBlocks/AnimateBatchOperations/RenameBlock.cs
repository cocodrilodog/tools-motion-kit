namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Renames the <see cref="AnimateBlock"/> at <see cref="AnimateBatchOperationPath.Path"/>
	/// and optionally appends the <c>index</c> + 1 to the <see cref="NewName"/>.
	/// </summary>
	[Serializable]
	public class RenameBlock : AnimateBatchOperationPath {


		#region Protected Methods

		/// <summary>
		/// Renames the <paramref name="childBlock"/> and if <see cref="AppendSerialNumber"/> is <c>true</c>, appends the 
		/// <c>index</c> + 1 to the <see cref="NewName"/>.
		/// </summary>
		/// <param name="childBlock">The child block located at the <c>Path</c>.</param>
		/// <param name="index">The index of the parent <see cref="AnimateBlock"/> in the list.</param>
		protected override void PerformOnChildBlock(AnimateBlock childBlock, int index) {
			childBlock.Name = NewName + (AppendSerialNumber ? $"{index + 1}" : "");
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
		/// The new name for the <see cref="AnimateBlock"/>.
		/// </summary>
		private string NewName => m_NewName;

		/// <summary>
		/// If checked, appends the <c>index</c> + 1 to the <see cref="NewName"/>.
		/// </summary>
		private bool AppendSerialNumber => m_AppendSerialNumber;

		#endregion


	}

}