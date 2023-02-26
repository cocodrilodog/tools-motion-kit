namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class IncrementalDuration : AnimateBlockOperation {


		#region Public Methods

		public override void Perform(AnimateBlock animateBlock, int index) {
			if (string.IsNullOrEmpty(Path)) {
				if (animateBlock != null) {
					animateBlock.DurationInput = BaseDuration + index * DurationIncrement;
				}
			} else {
				var childBlock = (animateBlock as IAnimateParent)?.GetChildBlockAtPath(Path);
				if (childBlock != null) {
					childBlock.DurationInput = BaseDuration + index * DurationIncrement;
				}
			}
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private string m_Path;

		[SerializeField]
		private float m_BaseDuration;

		[SerializeField]
		private float m_DurationIncrement;

		#endregion


		#region Private Properties

		private string Path => m_Path;

		private float BaseDuration => m_BaseDuration;

		private float DurationIncrement => m_DurationIncrement;

		#endregion


	}

}