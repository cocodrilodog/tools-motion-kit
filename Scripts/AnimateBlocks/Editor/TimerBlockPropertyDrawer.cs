namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(TimerBlock))]
	public class TimerBlockPropertyDrawer : AnimateBlockPropertyDrawer {


		#region Protected Properties

		protected override bool DoesDrawEasing => false;

		#endregion


	}

}