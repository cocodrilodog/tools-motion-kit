namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Creates a copy of <see cref="TemplateBlock"/> and assigns it to the list at the corresponding index.
	/// </summary>
	[Serializable]
	public class CopyTemplateBlock : MotionKitBatchOperation {


		#region Public Methods

		/// <summary>
		/// Creates a copy of <see cref="TemplateBlock"/> assigns it to <paramref name="motionKitBlock"/>.
		/// </summary>
		/// <param name="motionKitBlock">Each <see cref="PlaybackBlock"/> in the list.</param>
		/// <param name="index">The index of the <see cref="PlaybackBlock"/> in the list.</param>
		/// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise</returns>
		public override bool Perform(ref PlaybackBlock motionKitBlock, int index) {
			base.Perform(ref motionKitBlock, index);
			CompositeCopier.Copy(TemplateBlock);
			// Assign it to the ref parameter so that it replaces the existing one in the list
			motionKitBlock = CompositeCopier.Paste() as PlaybackBlock;
			return true;
		}

		#endregion


		#region Private Fields

		[SerializeReference]
		private PlaybackBlock m_TemplateBlock;

		#endregion


		#region Private Properties

		private PlaybackBlock TemplateBlock => m_TemplateBlock;

		#endregion


	}

}