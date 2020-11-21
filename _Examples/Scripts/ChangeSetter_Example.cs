namespace CocodriloDog.Animation.Examples {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ChangeSetter_Example : MonoBehaviour {


		#region Unity Methods

		private void Start() {

			Animate.GetMotion(this, "X", p => Cube.localPosition = p)
				.Play(Cube.localPosition, Cube.localPosition + Vector3.right * 5, 1)
				.SetOnComplete(Scale);

			// The setter can be changed as long as it is of the same type as the one in the 
			// reused motion. For example the scale setter receives a Vector3 as the position 
			// setter does so.
			void Scale(Motion3D motion) {
				motion.Clean(CleanFlag.OnComplete);
				Animate.GetMotion(this, "X", s => Cube.localScale = s)
					.Play(Cube.localScale, Vector3.one * 2, 1);
			}

			// Error test: Try to use a color setter
			//void Scale(Motion3D motion) {
			//	motion.Clean(CleanFlag.OnComplete);
			//	Animate.GetMotion(this, "X", c => Cube.GetComponent<Renderer>().material.color = c)
			//		.Play(Color.black, Color.white, 1);
			//}

		}

		private void OnDestroy() {
			Animate.ClearPlaybacks(this);
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private Transform m_Cube;

		#endregion


		#region Private Properties

		private Transform Cube => m_Cube;
		
		#endregion

	}

}
