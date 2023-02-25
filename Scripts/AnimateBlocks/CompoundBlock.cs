namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// Base block for <see cref="SequenceBlock"/> and <see cref="ParallelBlock"/>.
	/// </summary>
	public abstract class CompoundBlock : AnimateBlock {


		#region Protected Properties

		protected List<AnimateBlockOperation> BatchOperations => m_BatchOperations;

		#endregion

		#region Private Fields - Serialized

		[SerializeReference]
		private List<AnimateBlockOperation> m_BatchOperations = new List<AnimateBlockOperation>();

		#endregion


	}

}