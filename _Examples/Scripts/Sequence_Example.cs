namespace CocodriloDog.Animation.Examples {

	using CocodriloDog.Rendering;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class Sequence_Example : MonoBehaviour {


		#region Unity Methods

		private void Start() {
			m_Sequence = Animate.GetSequence(

				this, "Sequence",

				Animate.GetMotion(p => PositionCube.transform.position = p)
					.SetInitialValue(new Vector3(2, -2, 0)).SetFinalValue(new Vector3(-2, -2, 0))
					.SetDuration(2).SetEasing(AnimateEasing.ElasticOut)
					.SetOnUpdate(() => Debug.LogFormat("Animation #1 Update"))
					.SetOnComplete(() => Debug.LogFormat("Animation #1 Complete: {0}", PositionCube.transform.position)),

				Animate.GetMotion(c => ColorSphere.Color = c)
					.SetInitialValue(Color.red).SetFinalValue(Color.black)
					.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
					.SetOnUpdate(() => Debug.LogFormat("Animation #2 Update"))
					.SetOnComplete(() => Debug.LogFormat("Animation #2 Complete: {0}", ColorSphere.Color)),

				Animate.GetTimer()
					.SetDuration(2)
					.SetOnUpdate(() => Debug.LogFormat("Timer Update"))
					.SetOnComplete(() => Debug.LogFormat("Timer Complete")),

				Animate.GetMotion(p => PositionCube.transform.position = p)
					.SetInitialValue(new Vector3(-2, -2, 0)).SetFinalValue(new Vector3(-2, 2, 0))
					.SetDuration(2).SetEasing(AnimateEasing.ElasticOut)
					.SetOnUpdate(() => Debug.LogFormat("Animation #3 Update"))
					.SetOnComplete(() => Debug.LogFormat("Animation #3 Complete: {0}", PositionCube.transform.position)),

				Animate.GetSequence(

					Animate.GetMotion(c => ColorSphere.Color = c)
						.SetInitialValue(Color.green).SetFinalValue(Color.black)
						.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
						.SetOnUpdate(() => Debug.LogFormat("Animation #4 Update"))
						.SetOnComplete(() => Debug.LogFormat("Animation #4 Complete: {0}", ColorSphere.Color)),

					Animate.GetMotion(p => PositionCube.transform.position = p)
						.SetInitialValue(new Vector3(-2, 2, 0)).SetFinalValue(new Vector3(2, 2, 0))
						.SetDuration(2).SetEasing(AnimateEasing.ElasticOut)
						.SetOnUpdate(() => Debug.LogFormat("Animation #5 Update"))
						.SetOnComplete(() => Debug.LogFormat("Animation #5 Complete: {0}", PositionCube.transform.position))

				).SetOnUpdate(() => Debug.LogFormat("Animation #4-5 Update!!!"))
				.SetOnComplete(() => Debug.LogFormat("Animation #4-5 Complete!!!")),

				Animate.GetMotion(c => ColorSphere.Color = c)
					.SetInitialValue(Color.blue).SetFinalValue(Color.black)
					.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
					.SetOnUpdate(() => Debug.LogFormat("Animation #6 Update"))
					.SetOnComplete(() => Debug.LogFormat("Animation #6 Complete: {0}", ColorSphere.Color)),

				Animate.GetMotion(p => PositionCube.transform.position = p)
					.SetInitialValue(new Vector3(2, 2, 0)).SetFinalValue(new Vector3(2, -2, 0))
					.SetDuration(2).SetEasing(AnimateEasing.ElasticOut)
					.SetOnUpdate(() => Debug.LogFormat("Animation #7 Update"))
					.SetOnComplete(() => Debug.LogFormat("Animation #7 Complete: {0}", PositionCube.transform.position)),

				Animate.GetMotion(c => ColorSphere.Color = c)
					.SetInitialValue(Color.white).SetFinalValue(Color.black)
					.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
					.SetOnUpdate(() => Debug.LogFormat("Animation #8 Update"))
					.SetOnComplete(() => Debug.LogFormat("Animation #8 Complete: {0}", ColorSphere.Color))

			).SetEasing(AnimateEasing.QuadInOut)
			.SetOnUpdate(()=> Debug.LogFormat("Sequence update"))
			.SetOnComplete(() => Debug.LogFormat("Sequence complete"))
			.Play();
		}

		private void Update() {
			// Update the UI
			if (m_Sequence.IsPlaying) {
				if (m_Sequence.IsPaused) {
					PlayPauseLabel.text = "Resume";
				} else {
					PlaybackSlider.value = m_Sequence.Progress;
					PlayPauseLabel.text = "Pause";
				}
			} else {
				PlayPauseLabel.text = "Play";
			}
		}

		#endregion


		#region UI Event Handlers

		public void ResetButton_OnClick() {
			m_Sequence.Reset();
			m_Sequence.Progress = 0;

			// Restore the sequence parameters
			//m_Sequence.SetDuration(m_Sequence.SequenceDuration);
			m_Sequence.SetDuration(0);
			m_Sequence.SetOnUpdate(() => Debug.LogFormat("Sequence update"))
				.SetOnComplete(() => Debug.LogFormat("Sequence complete"));

			PlaybackSlider.value = 0;
		}

		public void StopButton_OnClick() {
			m_Sequence.Stop();
		}

		public void PlayPauseButton_OnClick() {
			if (m_Sequence.IsPlaying) {
				if (m_Sequence.IsPaused) {
					m_Sequence.Resume();
				} else {
					m_Sequence.Pause();
				}
			} else {
				m_Sequence.Play();
			}
		}

		public void Playback_OnSliderValueChanged(float value) {
			if (!m_Sequence.IsPlaying || (m_Sequence.IsPlaying && m_Sequence.IsPaused)) {
				m_Sequence.Progress = value;
			}
		}

		#endregion


		#region Private Fields - Serialized

		[SerializeField]
		private Transform m_PositionCube;

		[SerializeField]
		private ColorModifier m_ColorSphere;

		[SerializeField]
		private Text m_PlayPauseLabel;

		[SerializeField]
		private Slider m_PlaybackSlider;

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private Sequence m_Sequence;

		#endregion


		#region Private Properties

		private Transform PositionCube { get { return m_PositionCube; } }

		private ColorModifier ColorSphere { get { return m_ColorSphere; } }

		private Text PlayPauseLabel { get { return m_PlayPauseLabel; } }

		private Slider PlaybackSlider { get { return m_PlaybackSlider; } }

		#endregion


	}

}