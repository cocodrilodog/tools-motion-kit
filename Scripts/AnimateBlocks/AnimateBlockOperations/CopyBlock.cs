namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class CopyBlock : AnimateBlockOperation {


		#region Public Methods

		public override AnimateBlock Perform(AnimateBlock animateBlock, int index) {
			CompositeCopier.Copy(Original);
			return CompositeCopier.Paste() as AnimateBlock;
		}

		#endregion


		#region Private Fields

		[SerializeReference]
		private AnimateBlock m_Original;

		#endregion


		#region Private Properties

		private AnimateBlock Original => m_Original;

		#endregion


	}

}