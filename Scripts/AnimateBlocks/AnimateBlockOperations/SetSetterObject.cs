namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Sets the setter object of the <see cref="IMotionBlock"/> at <see cref="AnimateBatchOperationPath.Path"/>.
	/// </summary>
	public class SetSetterObject : AnimateBatchOperationPath {


		#region Protected Methods

		protected override void PerformOnChildBlock(AnimateBlock childBlock, int index) {
			if (childBlock is IMotionBlock && index >= 0 && index < SetterObjects.Length) {
				(childBlock as IMotionBlock).Object = SetterObjects[index];
			}
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private UnityEngine.Object[] m_SetterObjects;

		#endregion


		#region Private Properties

		/// <summary>
		/// A list of objects whose items will be set in each <see cref="IMotionBlock"/>'s setter object in 
		/// the list at the corresponding index.
		/// </summary>
		private UnityEngine.Object[] SetterObjects => m_SetterObjects;

		#endregion


	}

}