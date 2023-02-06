namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(TimerComponent))]
	public class TimerComponentEditor : AnimateBaseComponentEditor {


		#region Protected Properties

		protected override bool WillDrawEasing => false;

		#endregion


	}

}