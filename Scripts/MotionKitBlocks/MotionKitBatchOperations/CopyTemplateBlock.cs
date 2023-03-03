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
		/// Creates a copy of <see cref="TemplateBlock"/> returns it.
		/// </summary>
		/// <param name="animateBlock">Each <see cref="MotionKitBlock"/> in the list.</param>
		/// <param name="index">The index of the <see cref="MotionKitBlock"/> in the list.</param>
		/// <returns>The copy of <see cref="TemplateBlock"/></returns>
		public override MotionKitBlock Perform(MotionKitBlock animateBlock, int index) {
			base.Perform(animateBlock, index);
			CompositeCopier.Copy(TemplateBlock);
			return CompositeCopier.Paste() as MotionKitBlock;
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