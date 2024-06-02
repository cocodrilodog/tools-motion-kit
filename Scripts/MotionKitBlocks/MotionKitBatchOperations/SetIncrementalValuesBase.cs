namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Sets the values of the <see cref="IMotionBaseBlock"/> at <see cref="MotionKitBatchOperationPath.Path"/>
	/// in a incremental way proportional to the <c>index</c> of the block.
	/// </summary>
	[Serializable]
	public abstract class SetIncrementalValuesBase<T> : MotionKitBatchOperationPath {


		#region Private Fields

		[Tooltip("The initial value that the first block in the target list will have.")]
		[SerializeField]
		private T m_BaseInitialValue;

		[Tooltip("The amount that the initial value will increment per each subsequent block.")]
		[SerializeField]
		private T m_InitialValueIncrement;

		[Tooltip("The final value that the first block in the target list will have.")]
		[SerializeField]
		private T m_BaseFinalValue;

		[Tooltip("The amount that the final value will increment per each subsequent block.")]
		[SerializeField]
		private T m_FinalValueIncrement;

		#endregion


		#region Private Properties

		/// <summary>
		/// The initial value of the first element in the list.
		/// </summary>
		protected T BaseInitialValue => m_BaseInitialValue;

		/// <summary>
		/// The increment of the initial value per element in the list.
		/// </summary>
		protected T InitialValueIncrement => m_InitialValueIncrement;

		/// <summary>
		/// The final value of the first element in the list.
		/// </summary>
		protected T BaseFinalValue => m_BaseFinalValue;

		/// <summary>
		/// The increment of the final value per element in the list.
		/// </summary>
		protected T FinalValueIncrement => m_FinalValueIncrement;

		#endregion


	}

}