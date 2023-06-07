namespace CocodriloDog.Animation.Examples {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[AddComponentMenu("")]
	public class GetChildBlockByPath_Example : MonoBehaviour {


		#region Public Fields

		[SerializeField]
		public MotionKitComponent MotionKitComponent;

		#endregion


		#region Unity Methods

		private IEnumerator Start() {
			yield return new WaitForSeconds(1);
			Debug.Log(MotionKitComponent.GetChildAtPath("Sequence/Motion2"));
		}

		#endregion


	}

}