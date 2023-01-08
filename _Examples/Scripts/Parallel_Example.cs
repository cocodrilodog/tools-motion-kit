namespace CocodriloDog.Animation.Examples {

	using CocodriloDog.Rendering;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class Parallel_Example : MonoBehaviour {


		#region Unity Methods

		private void Start() {

			m_Parallel = Animate.GetParallel(

				this, "Parallel",

				Animate.GetMotion(p => Cube1.transform.position = p)
					.SetValuesAndDuration(new Vector3(-2, -2, 0), new Vector3(-2, 2, 0), 2)
					.SetEasing(AnimateEasing.ElasticOut)
					.SetOnStart(() => Debug.LogFormat("Cube1 start"))
					.SetOnUpdate(() => Debug.LogFormat("Cube1 update"))
					.SetOnInterrupt(() => Debug.LogFormat("Cube1 interrupt"))
					.SetOnComplete(() => Debug.LogFormat("Cube1 complete")),

				Animate.GetSequence(

					Animate.GetTimer().SetDuration(0.5f)
						.SetOnStart(() => Debug.LogFormat("Timer1 start"))
						.SetOnUpdate(() => Debug.LogFormat("Timer1 update"))
						.SetOnInterrupt(() => Debug.LogFormat("Timer1 interrupt"))
						.SetOnComplete(() => Debug.LogFormat("Timer1 complete")),

					Animate.GetMotion(p => Cube2.transform.position = p)
						.SetValuesAndDuration(new Vector3(0, -2, 0), new Vector3(0, 2, 0), 2)
						.SetEasing(AnimateEasing.ElasticOut)
						.SetOnStart(() => Debug.LogFormat("Cube2 start"))
						.SetOnUpdate(() => Debug.LogFormat("Cube2 update"))
						.SetOnInterrupt(() => Debug.LogFormat("Cube2 interrupt"))
						.SetOnComplete(() => Debug.LogFormat("Cube2 complete"))

				),

				Animate.GetSequence(
					
					Animate.GetTimer().SetDuration(1)
						.SetOnStart(() => Debug.LogFormat("Timer2 start"))
						.SetOnUpdate(() => Debug.LogFormat("Timer2 update"))
						.SetOnInterrupt(() => Debug.LogFormat("Timer2 interrupt"))
						.SetOnComplete(() => Debug.LogFormat("Timer2 complete")),

					Animate.GetMotion(p => Cube3.transform.position = p)
						.SetValuesAndDuration(new Vector3(2, -2, 0), new Vector3(2, 2, 0), 2)
						.SetEasing(AnimateEasing.ElasticOut)
						.SetOnStart(() => Debug.LogFormat("Cube3 start"))
						.SetOnUpdate(() => Debug.LogFormat("Cube3 update"))
						.SetOnInterrupt(() => Debug.LogFormat("Cube3 interrupt"))
						.SetOnComplete(() => Debug.LogFormat("Cube3 complete"))

				)

			).SetEasing(AnimateEasing.QuadInOut)
			.SetOnStart(() => Debug.Log("Parallel start"))
			.SetOnUpdate(() => Debug.LogFormat("Parallel update"))
			.SetOnInterrupt(() => Debug.LogFormat("Parallel interrupt"))
			.SetOnComplete(() => Debug.LogFormat("Parallel complete"))
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

		private Text PlayPauseLabel => m_PlayPauseLabel;

		private Slider PlaybackSlider => m_PlaybackSlider;

		#endregion


	}

}