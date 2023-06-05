namespace CocodriloDog.Animation.Examples {

	using CocodriloDog.Rendering;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	[AddComponentMenu("")]
	public class Parallel_Example : MonoBehaviour {


		#region Unity Methods

		private void Start() {

			m_Parallel = MotionKit.GetParallel(

				this, "Parallel",

				MotionKit.GetMotion(p => Cube1.transform.position = p)
					.SetValuesAndDuration(new Vector3(-2, -2, 0), new Vector3(-2, 2, 0), 2)
					.SetEasing(MotionKitEasing.ElasticOut)
					.SetOnStart(() => Debug.LogFormat("Cube1 start"))
					.SetOnUpdate(() => Debug.LogFormat("Cube1 update"))
					.SetOnInterrupt(() => Debug.LogFormat("Cube1 interrupt"))
					.SetOnComplete(() => Debug.LogFormat("Cube1 complete")),

				MotionKit.GetSequence(

					MotionKit.GetTimer().SetDuration(0.5f)
						.SetOnStart(() => Debug.LogFormat("Timer_Cube2 start"))
						.SetOnUpdate(() => Debug.LogFormat("Timer_Cube2 update"))
						.SetOnInterrupt(() => Debug.LogFormat("Timer_Cube2 interrupt"))
						.SetOnComplete(() => Debug.LogFormat("Timer_Cube2 complete")),

					MotionKit.GetMotion(p => Cube2.transform.position = p)
						.SetValuesAndDuration(new Vector3(0, -2, 0), new Vector3(0, 2, 0), 2)
						.SetEasing(MotionKitEasing.ElasticOut)
						.SetOnStart(() => Debug.LogFormat("Cube2 start"))
						.SetOnUpdate(() => Debug.LogFormat("Cube2 update"))
						.SetOnInterrupt(() => Debug.LogFormat("Cube2 interrupt"))
						.SetOnComplete(() => Debug.LogFormat("Cube2 complete"))

				).SetOnStart(() => Debug.LogFormat("Sequence_Cube2 start"))
				.SetOnUpdate(() => Debug.LogFormat("Sequence_Cube2 update"))
				.SetOnInterrupt(() => Debug.LogFormat("Sequence_Cube2 interrupt"))
				.SetOnComplete(() => Debug.LogFormat("Sequence_Cube2 complete")),

				MotionKit.GetSequence(
					
					MotionKit.GetTimer().SetDuration(1)
						.SetOnStart(() => Debug.LogFormat("Timer_Cube3 start"))
						.SetOnUpdate(() => Debug.LogFormat("Timer_Cube3 update"))
						.SetOnInterrupt(() => Debug.LogFormat("Timer_Cube3 interrupt"))
						.SetOnComplete(() => Debug.LogFormat("Timer_Cube3 complete")),

					MotionKit.GetMotion(p => Cube3.transform.position = p)
						.SetValuesAndDuration(new Vector3(2, -2, 0), new Vector3(2, 2, 0), 2)
						.SetEasing(MotionKitEasing.ElasticOut)
						.SetOnStart(() => Debug.LogFormat("Cube3 start"))
						.SetOnUpdate(() => Debug.LogFormat("Cube3 update"))
						.SetOnInterrupt(() => Debug.LogFormat("Cube3 interrupt"))
						.SetOnComplete(() => Debug.LogFormat("Cube3 complete"))

				).SetOnStart(() => Debug.LogFormat("Sequence_Cube3 start"))
				.SetOnUpdate(() => Debug.LogFormat("Sequence_Cube3 update"))
				.SetOnInterrupt(() => Debug.LogFormat("Sequence_Cube3 interrupt"))
				.SetOnComplete(() => Debug.LogFormat("Sequence_Cube3 complete")),

				MotionKit.GetParallel(

					MotionKit.GetMotion(p => Sphere1.transform.position = p)
						.SetValuesAndDuration(new Vector3(-2, -4, 0), new Vector3(2, -4, 0), 2)
						.SetEasing(MotionKitEasing.BounceOut)
						.SetOnStart(() => Debug.LogFormat("Sphere1 start"))
						.SetOnUpdate(() => Debug.LogFormat("Sphere1 update"))
						.SetOnInterrupt(() => Debug.LogFormat("Sphere1 interrupt"))
						.SetOnComplete(() => Debug.LogFormat("Sphere1 complete")),

					MotionKit.GetMotion(p => Sphere2.transform.position = p)
						.SetValuesAndDuration(new Vector3(2, -4, 0), new Vector3(-2, -4, 0), 2)
						.SetEasing(MotionKitEasing.BounceOut)
						.SetOnStart(() => Debug.LogFormat("Sphere2 start"))
						.SetOnUpdate(() => Debug.LogFormat("Sphere2 update"))
						.SetOnInterrupt(() => Debug.LogFormat("Sphere2 interrupt"))
						.SetOnComplete(() => Debug.LogFormat("Sphere2 complete"))

				).SetOnStart(() => Debug.LogFormat("Parallel_Spheres start"))
				.SetOnUpdate(() => Debug.LogFormat("Parallel_Spheres update"))
				.SetOnInterrupt(() => Debug.LogFormat("Parallel_Spheres interrupt"))
				.SetOnComplete(() => Debug.LogFormat("Parallel_Spheres complete"))

			).SetEasing(MotionKitEasing.QuadInOut)
			.SetOnStart(() => Debug.Log("PARALLEL start"))
			.SetOnUpdate(() => Debug.LogFormat("PARALLEL update"))
			.SetOnInterrupt(() => Debug.LogFormat("PARALLEL interrupt"))
			.SetOnComplete(() => Debug.LogFormat("PARALLEL complete"))
			.Play();

		}

		private void Update() {
			// Update the UI
			if (m_Parallel.IsPlaying) {
				if (m_Parallel.IsPaused) {
					PlayPauseLabel.text = "Resume";
				} else {
					PlaybackSlider.value = m_Parallel.Progress;
					PlayPauseLabel.text = "Pause";
				}
			} else {
				PlayPauseLabel.text = "Play";
			}
		}

		#endregion


		#region UI Event Handlers

		public void ResetButton_OnClick() {
			m_Parallel.Dispose();
		}

		public void StopButton_OnClick() {
			m_Parallel.Stop();
		}

		public void PlayPauseButton_OnClick() {
			if (m_Parallel.IsPlaying) {
				if (m_Parallel.IsPaused) {
					m_Parallel.Resume();
				} else {
					m_Parallel.Pause();
				}
			} else {
				m_Parallel.Play();
			}
		}

		public void Playback_OnSliderValueChanged(float value) {
			if (!m_Parallel.IsPlaying || (m_Parallel.IsPlaying && m_Parallel.IsPaused)) {
				m_Parallel.Progress = value;
			}
		}

		#endregion


		#region Private Fields - Serialized

		[SerializeField]
		private Transform m_Cube1;

		[SerializeField]
		private Transform m_Cube2;

		[SerializeField]
		private Transform m_Cube3;

		[SerializeField]
		private Transform m_Sphere1;

		[SerializeField]
		private Transform m_Sphere2;

		[SerializeField]
		private Text m_PlayPauseLabel;

		[SerializeField]
		private Slider m_PlaybackSlider;

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private Parallel m_Parallel;

		#endregion


		#region Private Properties

		private Transform Cube1 => m_Cube1;

		private Transform Cube2 => m_Cube2;

		private Transform Cube3 => m_Cube3;

		private Transform Sphere1 => m_Sphere1;

		private Transform Sphere2 => m_Sphere2;

		private Text PlayPauseLabel => m_PlayPauseLabel;

		private Slider PlaybackSlider => m_PlaybackSlider;

		#endregion


	}

}