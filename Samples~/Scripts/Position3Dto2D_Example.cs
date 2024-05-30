namespace CocodriloDog.MotionKit.Examples {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[AddComponentMenu("")]
	public class Position3Dto2D_Example : MonoBehaviour {


		#region Public Properties

		public Vector2 Position2D {
			get => transform.localPosition;
			set => transform.localPosition = value;
		}

		#endregion


	}

}