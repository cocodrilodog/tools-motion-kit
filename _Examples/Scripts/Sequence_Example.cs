namespace CocodriloDog.Animation.Examples {

	using CocodriloDog.Rendering;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	[AddComponentMenu("")]
	public class Sequence_Example : MonoBehaviour {


		#region Unity Methods

		private void Start() {
			m_Sequence = MotionKit.GetSequence(

				this, "Sequence",

				MotionKit.GetTimer()
					.SetDuration(0.5f)
					.SetOnStart(() => Debug.LogFormat("Timer #1 Start"))
					.SetOnUpdate(() => Debug.LogFormat("Timer #1 Update"))
					.SetOnInterrupt(() => Debug.LogFormat("Timer #1 Interrupt"))
					.SetOnComplete(() => Debug.LogFormat("Timer #1 Complete")),

				MotionKit.GetMotion(p => PositionCube.transform.position = p)
					.SetInitialValue(new Vector3(2, -2, 0)).SetFinalValue(new Vector3(-2, -2, 0))
					.SetDuration(2).SetEasing(MotionKitEasing.ElasticOut)
					.SetOnStart(() => Debug.LogFormat("Animation #1 Start"))
					.SetOnUpdate(() => Debug.LogFormat("Animation #1 Update"))
					.SetOnInterrupt(() => Debug.LogFormat("Animation #1 Interrupt"))
					.SetOnComplete(() => Debug.LogFormat("Animation #1 Complete: {0}", PositionCube.transform.position)),

				MotionKit.GetMotion(c => ColorSphere.Color = c)
					.SetInitialValue(Color.red).SetFinalValue(Color.black)
					.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
					.SetOnStart(() => Debug.LogFormat("Animation #2 Start"))
					.SetOnUpdate(() => Debug.LogFormat("Animation #2 Update"))
					.SetOnInterrupt(() => Debug.LogFormat("Animation #2 Interrupt"))
					.SetOnComplete(() => Debug.LogFormat("Animation #2 Complete: {0}", ColorSphere.Color)),

				MotionKit.GetTimer()
					.SetDuration(2)
					.SetOnStart(() => Debug.LogFormat("Timer #2 Start"))
					.SetOnUpdate(() => Debug.LogFormat("Timer #2 Update"))
					.SetOnInterrupt(() => Debug.LogFormat("Timer #2 Interrupt"))
					.SetOnComplete(() => Debug.LogFormat("Timer #2 Complete")),

				MotionKit.GetMotion(p => PositionCube.transform.position = p)
					.SetInitialValue(new Vector3(-2, -2, 0)).SetFinalValue(new Vector3(-2, 2, 0))
					.SetDuration(2).SetEasing(MotionKitEasing.ElasticOut)
					.SetOnStart(() => Debug.LogFormat("Animation #3 Start"))
					.SetOnUpdate(() => Debug.LogFormat("Animation #3 Update"))
					.SetOnInterrupt(() => Debug.LogFormat("Animation #3 Interrupt"))
					.SetOnComplete(() => Debug.LogFormat("Animation #3 Complete: {0}", PositionCube.transform.position)),

				MotionKit.GetSequence(

					MotionKit.GetMotion(c => ColorSphere.Color = c)
						.SetInitialValue(Color.green).SetFinalValue(Color.black)
						.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
						.SetOnStart(() => Debug.LogFormat("Animation #4 Start"))
						.SetOnUpdate(() => Debug.LogFormat("Animation #4 Update"))
						.SetOnInterrupt(() => Debug.LogFormat("Animation #4 Interrupt"))
						.SetOnComplete(() => Debug.LogFormat("Animation #4 Complete: {0}", ColorSphere.Color)),

					MotionKit.GetMotion(p => PositionCube.transform.position = p)
						.SetInitialValue(new Vector3(-2, 2, 0)).SetFinalValue(new Vector3(2, 2, 0))
						.SetDuration(2).SetEasing(MotionKitEasing.ElasticOut)
						.SetOnStart(() => Debug.LogFormat("Animation #5 Start"))
						.SetOnUpdate(() => Debug.LogFormat("Animation #5 Update"))
						.SetOnInterrupt(() => Debug.LogFormat("Animation #5 Interrupt"))
						.SetOnComplete(() => Debug.LogFormat("Animation #5 Complete: {0}", PositionCube.transform.position))

				).SetOnStart(() => Debug.LogFormat("Animation #4-5 Start!!!"))
				.SetOnUpdate(() => Debug.LogFormat("Animation #4-5 Update!!!"))
				.SetOnInterrupt(() => Debug.LogFormat("Animation #4-5 Interrupt!!!"))
				.SetOnComplete(() => Debug.LogFormat("Animation #4-5 Complete!!!")),

				MotionKit.GetMotion(c => ColorSphere.Color = c)
					.SetInitialValue(Color.blue).SetFinalValue(Color.black)
					.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
					.SetOnStart(() => Debug.LogFormat("Animation #6 Start"))
					.SetOnUpdate(() => Debug.LogFormat("Animation #6 Update"))
					.SetOnInterrupt(() => Debug.LogFormat("Animation #6 Interrupt"))
					.SetOnComplete(() => Debug.LogFormat("Animation #6 Complete: {0}", ColorSphere.Color)),

				MotionKit.GetMotion(p => PositionCube.transform.position = p)
					.SetInitialValue(new Vector3(2, 2, 0)).SetFinalValue(new Vector3(2, -2, 0))
					.SetDuration(2).SetEasing(MotionKitEasing.ElasticOut)
					.SetOnStart(() => Debug.LogFormat("Animation #7 Start"))
					.SetOnUpdate(() => Debug.LogFormat("Animation #7 Update"))
					.SetOnInterrupt(() => Debug.LogFormat("Animation #7 Interrupt"))
					.SetOnComplete(() => Debug.LogFormat("Animation #7 Complete: {0}", PositionCube.transform.position)),

				MotionKit.GetMotion(c => ColorSphere.Color = c)
					.SetInitialValue(Color.white).SetFinalValue(Color.black)
					.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
					.SetOnStart(() => Debug.LogFormat("Animation #8 Start"))
					.SetOnUpdate(() => Debug.LogFormat("Animation #8 Update"))
					.SetOnInterrupt(() => Debug.LogFormat("Animation #8 Interrupt"))
					.SetOnComplete(() => Debug.LogFormat("Animation #8 Complete: {0}", ColorSphere.Color)),

				MotionKit.GetTimer()
					.SetDuration(0.5f)
					.SetOnStart(() => Debug.LogFormat("Timer #3 Start"))
					.SetOnUpdate(() => Debug.LogFormat("Timer #3 Update"))
					.SetOnInterrupt(() => Debug.LogFormat("Timer #3 Interrupt"))
					.SetOnComplete(() => Debug.LogFormat("Timer #3 Complete")),

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
			.SetOnStart(() => Debug.Log($"SEQUENCE START: {Time.time}"))
			.SetOnUpdate(()=> Debug.LogFormat($"SEQUENCE UPDATE: {Time.time}"))
			.SetOnInterrupt(() => Debug.LogFormat($"SEQUENCE INTERRUPT: {Time.time}"))
			.SetOnComplete(() => Debug.LogFormat($"SEQUENCE COMPLETE: {Time.time}"))
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
			m_Sequence.Dispose();
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
		private Sequence m_Sequence;

		#endregion


		#region Private Properties

		private Transform PositionCube { get { return m_PositionCube; } }

		private ColorModifier ColorSphere { get { return m_ColorSphere; } }

		private Transform Sphere1 => m_Sphere1;

		private Transform Sphere2 => m_Sphere2;

		private Text PlayPauseLabel { get { return m_PlayPauseLabel; } }

		private Slider PlaybackSlider { get { return m_PlaybackSlider; } }

		#endregion


	}

}