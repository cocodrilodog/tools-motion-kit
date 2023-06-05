namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Sets the setter object on each <see cref="IMotionBaseBlock"/> on the list at the
	/// <see cref="MotionKitBatchOperationPath.Path"/> if it is found.
	/// </summary>
	public class SetSetterObject : MotionKitBatchOperationPath {


		#region Protected Methods


		/// <summary>
		/// Sets the setter object of the <paramref name="childBlock"/> if it is a <see cref="IMotionBaseBlock"/>.
		/// </summary>
		/// <param name="childBlock">The child block located at the <c>Path</c>.</param>
		/// <param name="index">The index of the parent <see cref="MotionKitBlock"/> in the list.</param>
		/// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise</returns>
		protected override bool PerformOnChildBlock(ref MotionKitBlock childBlock, int index) {
			if (childBlock is IMotionBaseBlock && index >= 0 && index < SetterObjects.Length) {
				(childBlock as IMotionBaseBlock).Object = SetterObjects[index];
				return true;
			}
			return false;
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private UnityEngine.Object[] m_SetterObjects;

		#endregion


		#region Private Properties

		/// <summary>
		/// A list of objects whose items will be set in each <see cref="IMotionBaseBlock"/>'s setter object in 
		/// the list at the corresponding index.
		/// </summary>
		private UnityEngine.Object[] SetterObjects => m_SetterObjects;

		#endregion


	}

}