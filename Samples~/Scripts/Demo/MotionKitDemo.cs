namespace CocodriloDog.MotionKit.Examples {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[AddComponentMenu("")]
	public class MotionKitDemo : MonoBehaviour {


		#region Unity Methods

		private void Start() {
			StartRotation();
			StartGradient();
			StartMove();
		}

		private void OnDestroy() {
			MotionKit.ClearPlaybacks(this);
		}

		#endregion

		#region Private Fields - Serialized

		[SerializeField]
		private List<GameObject> m_Cubes;

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private List<Transform> m_CubeTransforms;

		[NonSerialized]
		private List<GradientAdapter> m_CubeGradients;

		[NonSerialized]
		private Parallel m_RotationParallel;

		[NonSerialized]
		private Parallel m_GradientParallel;

		[NonSerialized]
		private Sequence m_MoveSequence;

		#endregion


		#region Private Properties

		private List<Transform> CubeTransforms {
			get {
				if (m_CubeTransforms == null) {
					m_CubeTransforms = new List<Transform>();
					foreach (var cube in m_Cubes) {
						m_CubeTransforms.Add(cube.GetComponent<Transform>());
					}
				}
				return m_CubeTransforms;
			}
		}

		private List<GradientAdapter> CubeGradients {
			get {
				if (m_CubeGradients == null) {
					m_CubeGradients = new List<GradientAdapter>();
					foreach (var cube in m_Cubes) {
						m_CubeGradients.Add(cube.GetComponent<GradientAdapter>());
					}
				}
				return m_CubeGradients;
			}
		}

		#endregion


		#region Private Methods

		private void StartRotation() {

			// Rotation
			var rotations = new ITimedProgressable[CubeTransforms.Count];

			for (int i = 0; i < CubeTransforms.Count; i++) {
				// Create a new index variable j because the lambda expression will excecute later an will
				// get the latest value of i which at that moment is m_Cubes.Count.
				var j = i;
				rotations[i] = MotionKit.GetMotion(r => CubeTransforms[j].localEulerAngles = r)
					.SetValuesAndDuration(new Vector3(i * 15, 0, 0), new Vector3(360 + i * 15, 0, 0), 10);
			}

			m_RotationParallel = MotionKit.GetParallel(this, "RotationParallel", rotations)
				.Play().SetOnComplete(() => m_RotationParallel.Play());

		}

		private void StartGradient() {

			// Gradient
			var gradients = new ITimedProgressable[CubeGradients.Count];

			for (int i = 0; i < CubeGradients.Count; i++) {
				var j = i;
				var k = (float)i;
				gradients[i] = MotionKit.GetMotion(v => CubeGradients[j].Value = v)
					.SetValuesAndDuration(k / CubeGradients.Count, 1 + k / CubeGradients.Count, 10);
			}

			m_GradientParallel = MotionKit.GetParallel(this, "GradientParallel", gradients)
				.Play().SetOnComplete(() => m_GradientParallel.Play());
		}

		private void StartMove() {

			// Move
			var moves = new ITimedProgressable[CubeTransforms.Count];

			for (int i = 0; i < CubeTransforms.Count; i++) {
				var j = i;
				moves[i] = MotionKit.GetSequence(
					MotionKit.GetTimer().SetDuration(0.1f + 0.1f * i),
					MotionKit.GetMotion(p => CubeTransforms[j].localPosition = p)
						.SetValuesAndDuration(CubeTransforms[j].localPosition, CubeTransforms[j].localPosition, 0.5f)
						.SetEasing(new Pulse(new Vector3(0, 1, 0)).Vector3Easing)
				);
			}

			var moveParallel = MotionKit.GetParallel(moves);

			m_MoveSequence = MotionKit.GetSequence(
				this, "MoveSequence",
				MotionKit.GetTimer().SetDuration(2),
				moveParallel
			).Play().SetOnComplete(() => m_MoveSequence.Play());

		}

		#endregion


	}

}