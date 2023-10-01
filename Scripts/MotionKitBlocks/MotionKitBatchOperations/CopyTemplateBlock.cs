namespace CocodriloDog.MotionKit {

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
		/// <param name="motionKitBlock">Each <see cref="MotionKitBlock"/> in the list.</param>
		/// <param name="index">The index of the <see cref="MotionKitBlock"/> in the list.</param>
		/// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise</returns>
		public override bool Perform(ref MotionKitBlock motionKitBlock, int index) {
			CompositeCopier.Copy(TemplateBlock);
			// Assign it to the ref parameter so that it replaces the existing one in the list
			motionKitBlock = CompositeCopier.Paste() as MotionKitBlock;
			return true;
		}

		#endregion


		#region Private Fields

		[SerializeReference]
		private MotionKitBlock m_TemplateBlock;

		#endregion


		#region Private Properties

		private MotionKitBlock TemplateBlock => m_TemplateBlock;

		#endregion


	}

}