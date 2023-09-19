namespace CocodriloDog.MotionKit {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Shake easing compatible with <see cref="MotionKit"/>.
	/// </summary>
	[Serializable]
	public class Shake : ParameterizedEasing {


		#region Public Fields

		/// <summary>
		/// The magnitude of the shake.
		/// </summary>
		public AnimatableValue Magnitude => m_Magnitude;

		/// <summary>
		/// How fast will the shake look?
		/// </summary>
		public float TMultiplier => m_TMultiplier;

		/// <summary>
		/// Will this shake be dampered over time?
		/// </summary>
		public bool IsDampered => m_IsDampered;

		/// <summary>
		/// The Damper curve.
		/// </summary>
		public AnimationCurve Damper => m_Damper;

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

					float magnitude = Magnitude.Float;
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

					Vector3 magnitude = Magnitude.Vector3;
					if (IsDampered) {
						magnitude *= Damper.Evaluate(t);
					}

					// PerlinNoise returns values from 0 to 1. For that reason we subtract 0.5f
					return lerp + new Vector3(
						(Mathf.PerlinNoise((t + timeOffset.x) * TMultiplier, 0f) - 0.5f) * magnitude.x,
						(Mathf.PerlinNoise((t + timeOffset.y) * TMultiplier, 0f) - 0.5f) * magnitude.y,
						(Mathf.PerlinNoise((t + timeOffset.z) * TMultiplier, 0f) - 0.5f) * magnitude.z
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

					Color magnitude = Magnitude.Color;
					if (IsDampered) {
						magnitude *= Damper.Evaluate(t);
					}

					// PerlinNoise returns values from 0 to 1. For that reason we subtract 0.5f
					return lerp + new Color(
						(Mathf.PerlinNoise((t + timeOffset.r) * TMultiplier, 0f) - 0.5f) * magnitude.r,
						(Mathf.PerlinNoise((t + timeOffset.g) * TMultiplier, 0f) - 0.5f) * magnitude.g,
						(Mathf.PerlinNoise((t + timeOffset.b) * TMultiplier, 0f) - 0.5f) * magnitude.b,
						(Mathf.PerlinNoise((t + timeOffset.b) * TMultiplier, 0f) - 0.5f) * magnitude.a
					);

				};

			}
		}

		#endregion


		#region Public Constructors

		public Shake() { }

		public Shake(
			float magnitudeFloat, Vector3 magnitudeVector3, Color magnitudeColor,
			float tMultiplier = 10, bool isDampered = true
		) {
			m_Magnitude = new AnimatableValue(magnitudeFloat, magnitudeVector3, magnitudeColor);
			m_TMultiplier = tMultiplier;
			m_IsDampered = isDampered;
		}

		public Shake(
			float magnitudeFloat, float tMultiplier = 7, bool isDampered = true
		) : this(magnitudeFloat, default, default, tMultiplier, isDampered) { }

		public Shake(
			Vector3 magnitudeVector3, float tMultiplier = 7, bool isDampered = true
		) : this(default, magnitudeVector3, default, tMultiplier, isDampered) { }

		public Shake(
			Color magnitudeColor, float tMultiplier = 7, bool isDampered = true
		) : this(default, default, magnitudeColor, tMultiplier, isDampered) { }

		#endregion


		#region Public Methods

		public override ParameterizedEasing Copy() {
			return new Shake {
				m_Magnitude = Magnitude.Copy(),
				m_TMultiplier = m_TMultiplier,
				m_IsDampered = m_IsDampered,
				m_Damper = new AnimationCurve(m_Damper.keys)
			};
		} 

		#endregion


		#region Private Fields - Serialized

		[SerializeField]
		private AnimatableValue m_Magnitude;

		[SerializeField]
		public float m_TMultiplier = 7;

		[SerializeField]
		public bool m_IsDampered = true;

		[SerializeField]
		private AnimationCurve m_Damper = new AnimationCurve(
			new Keyframe(0f, 1f), new Keyframe(0.9f, .33f, -2f, -2f), new Keyframe(1f, 0f, -5.65f, -5.65f)
		);

		#endregion


	}

}
