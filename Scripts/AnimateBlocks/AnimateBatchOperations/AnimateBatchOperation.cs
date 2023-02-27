namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// An operation to be performed on a list of <see cref="AnimateBlock"/>s that belong to 
	/// <see cref="SequenceBlock"/>s and <see cref="ParallelBlock"/>s
	/// </summary>
	/// <remarks>
	/// This is used only by <see cref="Animate"/> the editor tools.
	/// </remarks>
	[Serializable]
	public abstract class AnimateBatchOperation : CompositeObject {


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
		/// Override this to perform an operation on the provided <paramref name="animateBlock"/> or create a new
		/// <see cref="AnimateBlock"/> that will be assigned to the list at <paramref name="index"/>.
		/// </summary>
		/// <param name="animateBlock">Each <see cref="AnimateBlock"/> in the list.</param>
		/// <param name="index">The index of the <see cref="AnimateBlock"/> in the list.</param>
		/// <returns>The modified <paramref name="animateBlock"/> or a new <see cref="AnimateBlock"/></returns>
		public virtual AnimateBlock Perform(AnimateBlock animateBlock, int index) {
			m_FieldActionIsPending = false;
			return animateBlock;
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