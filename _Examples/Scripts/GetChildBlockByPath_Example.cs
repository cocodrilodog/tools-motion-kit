namespace CocodriloDog.Animation.Examples {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class GetChildBlockByPath_Example : MonoBehaviour {


		#region Public Fields

		[SerializeField]
		public AnimateComponent AnimateComponent;

		#endregion


		#region Unity Methods

		private IEnumerator Start() {
			yield return new WaitForSeconds(1);
			Debug.Log(AnimateComponent.GetChildBlockAtPath("Sequence/Motion2"));
		}

		#endregion


	}

}