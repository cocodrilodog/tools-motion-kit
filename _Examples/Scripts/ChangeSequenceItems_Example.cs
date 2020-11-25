namespace CocodriloDog.Animation.Examples {

    using CocodriloDog.Animation;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ChangeSequenceItems_Example : MonoBehaviour {


		#region Unity Methods

		private void Start() {
            Forward();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Back();
            }
        }

        #endregion


        #region Private Fields

        [SerializeField]
        private Transform m_Cube;

        #endregion


        #region Private Properties

        [SerializeField]
        private Transform Cube => m_Cube;

		#endregion


		#region Private Methods

		void Forward() {
            Animate.GetSequence(this, "TheSequence",
                Animate.GetMotion(p => Cube.transform.position = p).SetValuesAndDuration(Vector3.zero, Vector3.right * 2, 1),
                Animate.GetMotion(p => Cube.transform.position = p).SetValuesAndDuration(Vector3.right * 2, new Vector3(2, 2, 0), 1)
            ).Play().SetOnComplete(Back);
        }

        void Back() {
            Animate.GetSequence(this, "TheSequence",
                Animate.GetMotion(p => Cube.transform.position = p).SetValuesAndDuration(new Vector3(2, 2, 0), Vector3.right * 2, 1),
                Animate.GetMotion(p => Cube.transform.position = p).SetValuesAndDuration(Vector3.right * 2, Vector3.zero, 1)
            ).Play().SetOnComplete(Forward);
        }

		#endregion


	}

}