namespace CocodriloDog.Core {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Allows to set the <c>x</c>, <c>y</c> and <c>z</c> positions individually in both local
	/// and world space.
	/// </summary>
	public class PositionXYZAdapter : MonoBehaviour {


		#region Public Properties

		public float X {
			get => transform.position.x;
			set {
				var pos = transform.position;
				pos.x = value;
				transform.position = pos;
			}
		}

		public float Y {
			get => transform.position.y;
			set {
				var pos = transform.position;
				pos.y = value;
				transform.position = pos;
			}
		}

		public float Z {
			get => transform.position.z;
			set {
				var pos = transform.position;
				pos.z = value;
				transform.position = pos;
			}
		}

		public float LocalX {
			get => transform.localPosition.x;
			set {
				var pos = transform.localPosition;
				pos.x = value;
				transform.localPosition = pos;
			}
		}

		public float LocalY {
			get => transform.localPosition.y;
			set {
				var pos = transform.localPosition;
				pos.y = value;
				transform.localPosition = pos;
			}
		}

		public float LocalZ {
			get => transform.localPosition.z;
			set {
				var pos = transform.localPosition;
				pos.z = value;
				transform.localPosition = pos;
			}
		}

		#endregion


	}

}