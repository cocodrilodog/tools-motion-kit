namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// This component processes <see cref="MotionKitBatchOperation"/>s on multiple <see cref="MotionKitComponent"/>s.
	/// </summary>
	public class MotionKitBatchComponent : CompositeRoot {


		#region Private Fields

		[Tooltip("The motion kit components that will be affected by the operations.")]
		[SerializeField]
		private List<MotionKitComponent> m_MotionKitComponents;

		[Tooltip("On the motion kit components, each batch operations group will target one motion kit block by index")]
		[SerializeField]
		private CompositeList<MotionKitBatchOperationsGroup> m_BatchOperationGroups;

		#endregion


	}

}
