namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// An operation to be performed on a list of <see cref="MotionKitBlock"/>s that belong to 
	/// <see cref="SequenceBlock"/>s or <see cref="ParallelBlock"/>s
	/// </summary>
	/// <remarks>
	/// This is used only by <see cref="MotionKit"/> the editor tools.
	/// </remarks>
	[Serializable]
	public abstract class MotionKitBatchOperation : CompositeObject {


		#region Public Properties

		public override CompositeFieldAction FieldAction {
			get {
				if (m_FieldAction == null) {
					m_FieldAction = new CompositeFieldAction();
					m_FieldAction.Label = "Run";
					m_FieldAction.Action = () => m_FieldActionIsPending = true;
				}
				return m_FieldAction;
			}
		}

		public bool FieldActionIsPending => m_FieldActionIsPending;

		#endregion


		#region Public Methods

		/// <summary>
		/// Override this to perform an operation on the provided <paramref name="motionKitBlock"/> or create a new
		/// <see cref="MotionKitBlock"/> that will be assigned to the list at <paramref name="index"/>.
		/// </summary>
		/// <param name="motionKitBlock">Each <see cref="MotionKitBlock"/> in the list.</param>
		/// <param name="index">The index of the <see cref="MotionKitBlock"/> in the list.</param>
		/// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise</returns>
		public virtual bool Perform(ref MotionKitBlock motionKitBlock, int index) {
			m_FieldActionIsPending = false;
			return false;
		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private CompositeFieldAction m_FieldAction;

		[NonSerialized]
		private bool m_FieldActionIsPending;

		#endregion


	}

}