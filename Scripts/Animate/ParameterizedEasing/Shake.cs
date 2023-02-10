namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Shake easing compatible with <see cref="Animate"/>.
	/// </summary>
	[Serializable]
	public class Shake : ParameterizedEasing {


		#region Public Fields

		/// <summary>
		/// How fast will the shake look?
		/// </summary>
		[SerializeField]
		public float TMultiplier = 10;

		/// <summary>
		/// The magnitude of the shake.
		/// </summary>
		[SerializeField]
		public float Magnitude = 2;

		/// <summary>
		/// Will this shake be dampered over time?
		/// </summary>
		[SerializeField]
		public bool IsDampered = true;

		#endregion


		#region Public Properties

		/// <summary>
		/// Gets an easing function for <c>float</c> modified by the shake parameters.
		/// </summary>
		public override MotionFloat.Easing FloatEasing {
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
					return lerp + (Mathf.PerlinNoise((t + timeOffset) * TMultiplier, 0f) - 0.5f) * magnitude;

				};

			}
		}

		/// <summary>
		/// Gets an easing function for <c>Vector3</c> modified by the shake parameters.
		/// </summary>
		public override Motion3D.Easing Vector3Easing {
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
						(Mathf.PerlinNoise((t + timeOffset.x) * TMultiplier, 0f) - 0.5f) * magnitude,
						(Mathf.PerlinNoise((t + timeOffset.y) * TMultiplier, 0f) - 0.5f) * magnitude,
						(Mathf.PerlinNoise((t + timeOffset.z) * TMultiplier, 0f) - 0.5f) * magnitude
					);

				};

			}
		}

		/// <summary>
		/// Gets an easing function for <c>Color</c> modified by the shake parameters.
		/// </summary>
		public override MotionColor.Easing ColorEasing {
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
						(Mathf.PerlinNoise((t + timeOffset.r) * TMultiplier, 0f) - 0.5f) * magnitude,
						(Mathf.PerlinNoise((t + timeOffset.g) * TMultiplier, 0f) - 0.5f) * magnitude,
						(Mathf.PerlinNoise((t + timeOffset.b) * TMultiplier, 0f) - 0.5f) * magnitude
					);

				};

			}
		}

		#endregion


		#region Public Constructors

		public Shake() { }

		public Shake(float tMultiplier = 10, float magnitude = 2, bool isDampered = true) {
			TMultiplier = tMultiplier;
			Magnitude = magnitude;
			IsDampered = isDampered;
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
