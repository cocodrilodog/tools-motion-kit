namespace CocodriloDog.Animation.Examples {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AnimateAssets_Example : MonoBehaviour {


		#region Public Properties

		public Vector3 Vector3 { 
			get => transform.position; 
			set => transform.position = value; 
		}

		public Vector2 Vector2 {
			get => transform.position;
			set => transform.position = value;
		}

		public float Float {
			get => transform.position.y;
			set {
				var pos = transform.position;
				pos.y = value;
				transform.position = pos;
			}
		}

		#endregion


		#region Public Methods

		public void SetVector3(Vector3 value) => transform.position = value;

		public void SetVector2(Vector2 value) => transform.position = value;

		public void SetFloat(float value) => Float = value;

		#endregion


	}

}