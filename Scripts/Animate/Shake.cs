namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Shake easing compatible with <see cref="Animate"/>.
	/// </summary>
	[Serializable]
	public class Shake {


		#region Public Fields

		[SerializeField]
		public float Speed = 10;

		[SerializeField]
		public float Magnitude = 2;

		[SerializeField]
		public bool IsDampered = true;

		#endregion


		#region Public Properties

		public MotionFloat.Easing FloatEasing {
			get {

				// This avoids that 2 or more animations that started at the same time look the same.
				float timeOffset = UnityEngine.Random.Range(0, 1000);

				return (a, b, t) => {

					// Get the base value so that the Perlin noise adds on top of it.
					float lerp = Mathf.Lerp(a, b, t);

					float magnitude = Magnitude;
					if (IsDampered) {
						magnitude *= Damper.Evaluate(t);
					}

					// PerlinNoise returns values from 0 to 1. For that reason we subtract 0.5f
					return lerp + (Mathf.PerlinNoise((Time.time + timeOffset) * Speed, 0f) - 0.5f) * magnitude;

				};

			}
		}

		public Motion3D.Easing Vector3Easing {
			get {

				// This avoids that the animations of x, y and z look the same.
				Vector3 timeOffset = UnityEngine.Random.insideUnitSphere * 1000;

				return (a, b, t) => {

					// Get the base value so that the Perlin noise adds on top of it.
					Vector3 lerp = Vector3.Lerp(a, b, t);

					float magnitude = Magnitude;
					if (IsDampered) {
						magnitude *= Damper.Evaluate(t);
					}

					// PerlinNoise returns values from 0 to 1. For that reason we subtract 0.5f
					return lerp + new Vector3(
						(Mathf.PerlinNoise((Time.time + timeOffset.x) * Speed, 0f) - 0.5f) * magnitude,
						(Mathf.PerlinNoise((Time.time + timeOffset.y) * Speed, 0f) - 0.5f) * magnitude,
						(Mathf.PerlinNoise((Time.time + timeOffset.z) * Speed, 0f) - 0.5f) * magnitude
					);

				};

			}
		}

		public MotionColor.Easing ColorEasing {
			get {

				// This avoids that the animations of x, y and z look the same.
				Color timeOffset = UnityEngine.Random.ColorHSV() * 1000;

				return (a, b, t) => {

					// Get the base value so that the Perlin noise adds on top of it.
					Color lerp = Color.Lerp(a, b, t);

					float magnitude = Magnitude;
					if (IsDampered) {
						magnitude *= Damper.Evaluate(t);
					}

					// PerlinNoise returns values from 0 to 1. For that reason we subtract 0.5f
					return lerp + new Color(
						(Mathf.PerlinNoise((Time.time + timeOffset.r) * Speed, 0f) - 0.5f) * magnitude,
						(Mathf.PerlinNoise((Time.time + timeOffset.g) * Speed, 0f) - 0.5f) * magnitude,
						(Mathf.PerlinNoise((Time.time + timeOffset.b) * Speed, 0f) - 0.5f) * magnitude
					);

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
