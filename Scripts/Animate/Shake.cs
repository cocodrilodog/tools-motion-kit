namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[Serializable]
	public class Shake {


		#region Public Fields

		[SerializeField]
		public float Speed = 10;

		[SerializeField]
		public float Magnitude = 2;

		[SerializeField]
		public bool Dampered = true;

		#endregion


		#region Public Properties

		public MotionFloat.Easing FloatEasing {
			get {

				// This avoids that 2 or more animations that started at the same time look the same.
				float timeOffset = UnityEngine.Random.Range(0, 1000);

				return (a, b, t) => {

					float lerp = Mathf.Lerp(a, b, t);

					float magnitude = Magnitude;
					if (Dampered) {
						magnitude *= Damper.Evaluate(t);
					}

					// PerlinNoise returns values from 0 to 1. For that reason we subtract magnitude / 2 
					return lerp + (Mathf.PerlinNoise((Time.time + timeOffset) * Speed, 0f) * magnitude) - (magnitude / 2);

				};

			}
		}

		#endregion


		#region Private Fields - Serialized

		[SerializeField]
		private AnimationCurve m_Damper = new AnimationCurve(
			new Keyframe(0f, 1f), new Keyframe(0.9f, .33f, -2f, -2f), new Keyframe(1f, 0f, -5.65f, -5.65f)
		);

		#endregion


		#region Private Properties

		private AnimationCurve Damper {
			get { return m_Damper; }
		}

		#endregion


	}

}
