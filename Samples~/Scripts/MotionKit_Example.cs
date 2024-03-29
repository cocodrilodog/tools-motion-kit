﻿namespace CocodriloDog.MotionKit.Examples {

	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using Random = UnityEngine.Random;

	[AddComponentMenu("")]
	public class MotionKit_Example : MonoBehaviour {


		#region Public Fields

		[Tooltip("A MotionKit curve.")]
		[SerializeField]
		public MotionKitCurve MotionKitCurve;

		#endregion


		#region MonoBehaviour Methods

		private void Start() {

			// Example of an animation that can be controlled with playback commands:
			StartPlaybackMotion();

			// Example of a compound animation that cycles:
			RepeatCycle();

			// Example of chained motion objects:
			//StartMotionMultiChain();
			StartMotionSelfChain();

		}

		private void Update() {
			// Update the UI
			if (PlaybackMotion.IsPlaying) {
				if (PlaybackMotion.IsPaused) {
					PlayPauseLabel.text = "Resume";
				} else {
					PlaybackSlider.value = PlaybackMotion.Progress;
					PlayPauseLabel.text = "Pause";
				}
			} else {
				PlayPauseLabel.text = "Play";
			}
		}

		private void OnDestroy() {
			// Clear the motions that were created by this script.
			MotionKit.ClearPlaybacks(this);
		}

		#endregion


		#region UI Event Handlers

		public void ResetButton_OnClick() {
			PlaybackMotion.Dispose();
		}

		public void StopButton_OnClick() {
			PlaybackMotion.Stop();
		}

		public void PlayPauseButton_OnClick() {
			if (PlaybackMotion.IsPlaying) {
				if (PlaybackMotion.IsPaused) {
					PlaybackMotion.Resume();
				} else {
					PlaybackMotion.Pause();
				}
			} else {
				// Set easing again because the motion may have been reset before
				PlaybackMotion.SetEasing(MotionKitEasing.ElasticOut)
					.Play(new Vector3(-5, -2, 0), new Vector3(-5, 2, 0), 5);
			}
		}

		public void Playback_OnSliderValueChanged(float value) {
			PlaybackMotion.Progress = value;
		}

		#endregion


		#region Internal Fields - Serialized

		[SerializeField]
		private Transform m_PlaybackObject;

		[SerializeField]
		private ColorAdapter m_ColorObject;

		[SerializeField]
		private Transform m_PositionObject;

		[SerializeField]
		private Transform m_ChainObject;

		[SerializeField]
		private Text m_PlayPauseLabel;

		[SerializeField]
		private Slider m_PlaybackSlider;

		#endregion


		#region Internal Fields - Non Serialized

		[NonSerialized]
		private Motion3D m_PlaybackMotion;

		[NonSerialized]
		private int m_Index;

		[NonSerialized]
		private Vector3[] m_Positions = {
			new Vector3(-2, -2, 0),
			new Vector3(-2, 2, 0),
			new Vector3(2, 2, 0),
			new Vector3(2, -2, 0)
		};

		#endregion


		#region Internal Properties

		private Transform PlaybackObject { get { return m_PlaybackObject; } }

		private ColorAdapter ColorObject { get { return m_ColorObject; } }

		private Transform PositionObject { get { return m_PositionObject; } }

		private Transform ChainObject { get { return m_ChainObject; } }

		private Text PlayPauseLabel { get { return m_PlayPauseLabel; } }

		private Slider PlaybackSlider { get { return m_PlaybackSlider; } }

		private Motion3D PlaybackMotion { get { return m_PlaybackMotion; } }

		private Vector3[] Positions { get { return m_Positions; } }

		#endregion


		#region Internal Methods

		private void StartPlaybackMotion() {
			m_PlaybackMotion = MotionKit.GetMotion(
				this, "Playback", p => PlaybackObject.localPosition = p
			).SetEasing(MotionKitEasing.ElasticOut)
			.Play(PlaybackObject.localPosition, new Vector3(-5, 2, 0), 5);
		}

		private void RepeatCycle() {
			AnimateCycle(Positions[m_Index], RepeatCycle);
			m_Index = m_Index++ == 3 ? 0 : m_Index;
		}

		private void AnimateCycle(Vector3 position, Action onComplete) {
			// Movement of the outer cube
			MotionKit.GetMotion(
				this, "Position", p => PositionObject.localPosition = p
			).SetEasing(MotionKitEasing.BackOut)
			.Play(PositionObject.localPosition, position, 1f)
			.SetOnComplete(() => {

				// A timer at the end of the movement
				MotionKit.GetTimer(this, "Timer").Play(1).SetOnComplete(onComplete);

				// A color blink in the sphere of the middle while the timer.
				// The blink graph is crafted in the MotionKitCurve
				MotionKit.GetMotion(
					this, "Color", c => ColorObject.Color = c
				).SetEasing(MotionKitCurve.ColorEasing)
				.Play(ColorObject.Color, Random.ColorHSV(), 1);

			});
		}

		private void StartMotionMultiChain() {

			List<Motion3D> motions = new List<Motion3D>();

			for (int i = 0; i < 10; i++) {
				Motion3D motion = MotionKit.GetMotion(
					this,
					// Name them differently so we are sure that the motions are different objects
					string.Format("Chain{0}", i),
					p => ChainObject.localPosition = p
				);
				motions.Add(motion);
				if (motions.Count > 1) {
					motions[i - 1].SetOnComplete(() => {
						motion.Play(
							// This will read the position only until this action is invoked :)
							ChainObject.localPosition,
							new Vector3(5, 0, 0) + Random.onUnitSphere * 3,
							1
						);
					});
				}
			}

			motions[0].Play(
				ChainObject.localPosition,
				new Vector3(5, 0, 0) + Random.onUnitSphere * 3,
				1
			);

		}

		private void StartMotionSelfChain() {

			Motion3D motion = MotionKit.GetMotion(
				this, "Chain", p => ChainObject.localPosition = p
			).SetEasing(MotionKitEasing.QuadInOut);

			motion.SetOnComplete(() => {
				motion.Play(
					ChainObject.localPosition,
					new Vector3(5, 0, 0) + Random.onUnitSphere * 3,
					1
				);
			}).Play(
				ChainObject.localPosition,
				new Vector3(5, 0, 0) + Random.onUnitSphere * 3,
				1
			);

		}

		#endregion


	}

}