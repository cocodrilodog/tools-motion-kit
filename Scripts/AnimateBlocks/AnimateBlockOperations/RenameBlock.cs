namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Renames the <see cref="AnimateBlock"/> provided at <see cref="PerformOnPathBlock(AnimateBlock, int)"/>
	/// and optionally appends the <c>index</c> + 1 to the <see cref="NewName"/>.
	/// </summary>
	[Serializable]
	public class RenameBlock : PathBlockOperation {


		#region Protected Methods

		/// <summary>
		/// Renames the <paramref name="pathBlock"/> and if <see cref="AppendSerialNumber"/> is <c>true</c>, appends the 
		/// <c>index</c> + 1 to the <see cref="NewName"/>.
		/// </summary>
		/// <param name="animateBlock">The <see cref="AnimateBlock"/>.</param>
		/// <param name="index">The index of the <see cref="AnimateBlock"/> when it belongs to a list or array.</param>
		protected override AnimateBlock PerformOnPathBlock(AnimateBlock pathBlock, int index) {
			pathBlock.Name = NewName + (AppendSerialNumber ? $"{index + 1}" : "");
			return pathBlock;
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