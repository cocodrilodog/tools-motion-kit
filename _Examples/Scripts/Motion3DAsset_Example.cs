namespace CocodriloDog.Animation.Examples {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Motion3DAsset_Example : MonoBehaviour {


		#region Public Properties

		public Vector3 Prop3D { 
			get => transform.position; 
			set => transform.position = value; 
		}

		public Vector2 Prop2D {
			get => transform.position;
			set => transform.position = value;
		}

		#endregion


		#region Public Methods

		public void Set3DValue(Vector3 value) => transform.position = value;

		public void Set2DValue(Vector2 value) => transform.position = value;

		#endregion


	}

}