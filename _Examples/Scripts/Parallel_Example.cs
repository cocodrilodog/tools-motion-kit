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
			//m_Parallel = Animate.GetParallel(

			//	this, "Parallel",

			//	Animate.GetMotion(p => PositionCube.transform.position = p)
			//		.SetInitialValue(new Vector3(2, -2, 0)).SetFinalValue(new Vector3(-2, -2, 0))
			//		.SetDuration(2).SetEasing(AnimateEasing.ElasticOut)
			//		.SetOnStart(() => Debug.LogFormat("Animation #1 Start"))
			//		.SetOnUpdate(() => Debug.LogFormat("Animation #1 Update"))
			//		.SetOnInterrupt(() => Debug.LogFormat("Animation #1 Interrupt"))
			//		.SetOnComplete(() => Debug.LogFormat("Animation #1 Complete: {0}", PositionCube.transform.position)),

			//	Animate.GetMotion(c => ColorSphere.Color = c)
			//		.SetInitialValue(Color.red).SetFinalValue(Color.black)
			//		.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
			//		.SetOnStart(() => Debug.LogFormat("Animation #2 Start"))
			//		.SetOnUpdate(() => Debug.LogFormat("Animation #2 Update"))
			//		.SetOnInterrupt(() => Debug.LogFormat("Animation #2 Interrupt"))
			//		.SetOnComplete(() => Debug.LogFormat("Animation #2 Complete: {0}", ColorSphere.Color)),

			//	Animate.GetTimer()
			//		.SetDuration(2)
			//		.SetOnStart(() => Debug.LogFormat("Timer Start"))
			//		.SetOnUpdate(() => Debug.LogFormat("Timer Update"))
			//		.SetOnInterrupt(() => Debug.LogFormat("Timer Interrupt"))
			//		.SetOnComplete(() => Debug.LogFormat("Timer Complete")),

			//	Animate.GetMotion(p => PositionCube.transform.position = p)
			//		.SetInitialValue(new Vector3(-2, -2, 0)).SetFinalValue(new Vector3(-2, 2, 0))
			//		.SetDuration(2).SetEasing(AnimateEasing.ElasticOut)
			//		.SetOnStart(() => Debug.LogFormat("Animation #3 Start"))
			//		.SetOnUpdate(() => Debug.LogFormat("Animation #3 Update"))
			//		.SetOnInterrupt(() => Debug.LogFormat("Animation #3 Interrupt"))
			//		.SetOnComplete(() => Debug.LogFormat("Animation #3 Complete: {0}", PositionCube.transform.position)),

			//	Animate.GetParallel(

			//		Animate.GetMotion(c => ColorSphere.Color = c)
			//			.SetInitialValue(Color.green).SetFinalValue(Color.black)
			//			.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
			//			.SetOnStart(() => Debug.LogFormat("Animation #4 Start"))
			//			.SetOnUpdate(() => Debug.LogFormat("Animation #4 Update"))
			//			.SetOnInterrupt(() => Debug.LogFormat("Animation #4 Interrupt"))
			//			.SetOnComplete(() => Debug.LogFormat("Animation #4 Complete: {0}", ColorSphere.Color)),

			//		Animate.GetMotion(p => PositionCube.transform.position = p)
			//			.SetInitialValue(new Vector3(-2, 2, 0)).SetFinalValue(new Vector3(2, 2, 0))
			//			.SetDuration(2).SetEasing(AnimateEasing.ElasticOut)
			//			.SetOnStart(() => Debug.LogFormat("Animation #5 Start"))
			//			.SetOnUpdate(() => Debug.LogFormat("Animation #5 Update"))
			//			.SetOnInterrupt(() => Debug.LogFormat("Animation #5 Interrupt"))
			//			.SetOnComplete(() => Debug.LogFormat("Animation #5 Complete: {0}", PositionCube.transform.position))

			//	).SetOnStart(() => Debug.LogFormat("Animation #4-5 Start!!!"))
			//	.SetOnUpdate(() => Debug.LogFormat("Animation #4-5 Update!!!"))
			//	.SetOnInterrupt(() => Debug.LogFormat("Animation #4-5 Interrupt!!!"))
			//	.SetOnComplete(() => Debug.LogFormat("Animation #4-5 Complete!!!")),

			//	Animate.GetMotion(c => ColorSphere.Color = c)
			//		.SetInitialValue(Color.blue).SetFinalValue(Color.black)
			//		.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
			//		.SetOnStart(() => Debug.LogFormat("Animation #6 Start"))
			//		.SetOnUpdate(() => Debug.LogFormat("Animation #6 Update"))
			//		.SetOnInterrupt(() => Debug.LogFormat("Animation #6 Interrupt"))
			//		.SetOnComplete(() => Debug.LogFormat("Animation #6 Complete: {0}", ColorSphere.Color)),

			//	Animate.GetMotion(p => PositionCube.transform.position = p)
			//		.SetInitialValue(new Vector3(2, 2, 0)).SetFinalValue(new Vector3(2, -2, 0))
			//		.SetDuration(2).SetEasing(AnimateEasing.ElasticOut)
			//		.SetOnStart(() => Debug.LogFormat("Animation #7 Start"))
			//		.SetOnUpdate(() => Debug.LogFormat("Animation #7 Update"))
			//		.SetOnInterrupt(() => Debug.LogFormat("Animation #7 Interrupt"))
			//		.SetOnComplete(() => Debug.LogFormat("Animation #7 Complete: {0}", PositionCube.transform.position)),

			//	Animate.GetMotion(c => ColorSphere.Color = c)
			//		.SetInitialValue(Color.white).SetFinalValue(Color.black)
			//		.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
			//		.SetOnStart(() => Debug.LogFormat("Animation #8 Start"))
			//		.SetOnUpdate(() => Debug.LogFormat("Animation #8 Update"))
			//		.SetOnInterrupt(() => Debug.LogFormat("Animation #8 Interrupt"))
			//		.SetOnComplete(() => Debug.LogFormat("Animation #8 Complete: {0}", ColorSphere.Color))

			//).SetEasing(AnimateEasing.QuadInOut)
			//.SetOnStart(() => Debug.Log("Parallel start"))
			//.SetOnUpdate(()=> Debug.LogFormat("Parallel update"))
			//.SetOnInterrupt(() => Debug.LogFormat("Parallel interrupt"))
			//.SetOnComplete(() => Debug.LogFormat("Parallel complete"))
			//.Play();

			m_Parallel = Animate.GetParallel(

				this, "Parallel",

				Animate.GetMotion(p => Cube1.transform.position = p)
					.SetValuesAndDuration(new Vector3(-2, -2, 0), new Vector3(-2, 2, 0), 2).SetEasing(AnimateEasing.ElasticOut),

				Animate.GetSequence(
					Animate.GetTimer().SetDuration(3),
					Animate.GetMotion(p => Cube2.transform.position = p)
						.SetValuesAndDuration(new Vector3(0, -2, 0), new Vector3(0, 2, 0), 2).SetEasing(AnimateEasing.ElasticOut)
				),

				Animate.GetSequence(
					Animate.GetTimer().SetDuration(3),
					Animate.GetMotion(p => Cube3.transform.position = p)
						.SetValuesAndDuration(new Vector3(2, -2, 0), new Vector3(2, 2, 0), 2).SetEasing(AnimateEasing.ElasticOut)
				)

			).Play();

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