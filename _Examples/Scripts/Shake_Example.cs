namespace CocodriloDog.Animation.Examples {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Shake_Example : MonoBehaviour {


		#region Public Fields

		[SerializeField]
		public Shake Shake;

		#endregion


		#region Unity Methods

		private void OnMouseDown() {
			
			Animate.GetMotion(this, "ShakeX", v => {
				Vector3 position = transform.position;
				position.x = v;
				transform.position = position;
			}).SetEasing(Shake.FloatEasing)
			.Play(transform.position.x, 0, 1);

			Animate.GetMotion(this, "ShakeY", v => {
				Vector3 position = transform.position;
				position.y = v;
				transform.position = position;
			}).SetEasing(Shake.FloatEasing)
			.Play(transform.position.y, 1, 1);

		}

		#endregion


	}

}