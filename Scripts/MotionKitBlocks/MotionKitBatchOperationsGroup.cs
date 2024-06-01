namespace CocodriloDog.MotionKit {
	
	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Contains a list of batch operations that will be applied to <see cref="MotionKitBlock"/>s that are hosted
	/// in several <see cref="MotionKitComponent"/>s. This works in conjunction with <see cref="MotionKitBatchComponent"/>.
	/// </summary>
	[Serializable]
	public class MotionKitBatchOperationsGroup : CompositeObject {


		#region Private Fields

		[Tooltip("In the motion kit components, the index of the block that will be affected by the operations.")]
		[SerializeField]
		private int m_AffectedBlockIndex;

		[Tooltip("The batch operations that will affect the blocks with the specified names.")]
		[SerializeField]
		private CompositeList<MotionKitBatchOperation> m_BatchOperations;

		#endregion


	}

}
