namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// ...
	/// </summary>
	[Serializable]
	public class Rename : PathBlockOperation {


		#region Protected Methods

		/// <summary>
		/// ...
		/// </summary>
		/// <param name="animateBlock">The <see cref="AnimateBlock"/>.</param>
		/// <param name="index">The index of the <see cref="AnimateBlock"/> when it belongs to a list or array.</param>
		protected override void PerformOnPathBlock(AnimateBlock pathBlock, int index) {
			pathBlock.Name = NewName + (AppendIndex ? $"{index + 1}" : "");
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private string m_NewName;

		[SerializeField]
		private bool m_AppendIndex;

		#endregion


		#region Private Properties

		private string NewName => m_NewName;

		private bool AppendIndex => m_AppendIndex;

		#endregion


	}

}