namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Scales the prompt based on a bell curve, depending on the horizontal 
	/// distance from the <see cref="DistanceFrom"/> object. 
	/// </summary>
	/// 
	/// <remarks>
	/// When the prompt is very close the <see cref="DistanceFrom"/> object its 
	/// scale is the <see cref="MaxScale"/>. When the object is farther away than
	/// half of the <see cref="BellWidth"/>, scale is <see cref="MinScale"/>.
	/// </remarks>
	public class BellScaler : MonoBehaviour {


		#region Public Fields

		/// <summary>
		/// The object from where the distance in X is measured.
		/// </summary>
		[SerializeField]
		public Transform DistanceFrom;

		/// <summary>
		/// The width of the bell in world units.
		/// </summary>
		[SerializeField]
		public float BellWidth;

		/// <summary>
		/// The maximum scale at the top of the bell.
		/// </summary>
		[SerializeField]
		public float MaxScale;

		/// <summary>
		/// The minimum scale at the bottom of the bell.
		/// </summary>
		[SerializeField]
		public float MinScale;

		#endregion


		#region MonoBehaviour Methods

		private void Start() {
			if(DistanceFrom == null) {
				Debug.LogWarningFormat(
					"{0}: DistanceFrom has not been assigned. No scaling will occur.",
					name
				);
			}
		}

		private void Reset() {
			BellWidth = 12;
			MaxScale = 1;
			MinScale = 0.8f;
		}

		public void Update() {
			if (DistanceFrom != null) {
				float distanceX = transform.position.x - DistanceFrom.position.x;

				if (!Mathf.Approximately(distanceX, m_LastDistanceX)) {

					float scale = 0;
					float bellHalfWidth = BellWidth * 0.5f;

					//
					// Cosine is used to generate this graph:
					//
					//              . * .
					//            *       *
					//          *           *
					//  * * * '               ' * * * *
					//		 	  bellWidth
					//      |<----------------->|
					//

					if (distanceX >= -bellHalfWidth && distanceX <= bellHalfWidth) {
						float minMax01 = Mathf.Cos(distanceX * 2 * Mathf.PI / BellWidth) * 0.5f + 0.5f;
						scale = Mathf.Lerp(MinScale, MaxScale, minMax01);
					} else {
						scale = MinScale;
					}

					transform.localScale = scale * Vector3.one;

					m_LastDistanceX = distanceX;

				}
			} 
		}

		#endregion


		#region Internal Fields

		[NonSerialized]
		private float m_LastDistanceX;

		#endregion


	}

}