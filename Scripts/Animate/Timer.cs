﻿namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using UnityEngine;

	public class Timer : IPlayback, ITimedProgressable {


		#region Public Properties

		/// <summary>
		/// The current time of this timer.
		/// </summary>
		/// 
		/// <remarks> 
		/// It is calculated inside of the coroutine while the timer is playing
		/// </remarks>
		/// 
		/// <value>The time.</value>
		public float CurrentTime { get { return m_CurrentTime; } }

		/// <summary>
		/// Gets the duration of this timer.
		/// </summary>
		/// 
		/// <remarks>
		/// It is set at <see cref="Play(float)"/> or alternatively
		/// at <see cref="SetDuration(float)"/>
		/// </remarks>
		/// 
		/// <value>The duration.</value>
		public float Duration { get { return m_Duration; } }

		/// <summary>
		/// Gets and sets the progress from 0 to 1 on this timer.
		/// </summary>
		/// <value>The progress.</value>
		public float Progress {	
			get { return m_Progress; }
			set { m_Progress = value; }
		}

		/// <summary>
		/// Is the timer playing?
		/// </summary>
		/// <value><c>true</c> if is playing; otherwise, <c>false</c>.</value>
		public bool IsPlaying { get { return m_IsPlaying; } }

		/// <summary>
		/// Is the timer paused?
		/// </summary>
		/// <value><c>true</c> if is paused; otherwise, <c>false</c>.</value>
		public bool IsPaused { get { return m_IsPaused; } }

		#endregion



		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Timer"/> class.
		/// </summary>
		/// <param name="monoBehaviour">
		/// A <see cref="MonoBehaviour"/> that will start the coroutines.
		/// </param>
		public Timer(MonoBehaviour monoBehaviour) {
			m_MonoBehaviour = monoBehaviour;
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Plays the timer that will last the <see cref="Duration"/>.
		/// </summary>
		/// 
		/// <remarks>
		/// <see cref="Duration"/> should have been set before via 
		/// <see cref="SetDuration(float)"/>.
		/// </remarks>
		/// 
		/// <returns>The timer object.</returns>
		public Timer Play() {
			return Play(Duration);
		}

		/// <summary>
		/// Plays the timer that will last the given <c>duration</c>. 
		/// </summary>
		/// <returns>The timer object.</returns>
		/// <param name="duration">Duration.</param>
		public Timer Play(float duration) {
			StopCoroutine();
			m_Duration = duration;
			m_IsPaused = false;
			m_Coroutine = m_MonoBehaviour.StartCoroutine(_Play());
			m_IsPlaying = true;
			return this;
		}

		/// <summary>
		/// Pauses the timer.
		/// </summary>
		/// <returns>The timer object.</returns>
		public Timer Pause() {
			m_IsPaused = true;
			return this;
		}

		/// <summary>
		/// Resumes this timer.
		/// </summary>
		/// <returns>The timer object.</returns>
		public Timer Resume() {
			// Progress may have changed from outside so we update m_CurrentTime here.
			m_CurrentTime = m_Progress * m_Duration;
			m_IsPaused = false;
			return this;
		}

		/// <summary>
		/// Stops the timer.
		/// </summary>
		public void Stop() {
			StopCoroutine();
			m_IsPlaying = false;
		}

		/// <summary>
		/// Resets the timer to its default state.
		/// </summary>
		/// <returns>The timer object.</returns>
		public void Reset() {

			StopCoroutine();

			// Value set on Play or individually
			m_Duration = 0;
			m_TimeMode = default;

			// Values set when starting the coroutine
			m_CurrentTime = 0;
			m_Progress = 0;

			// Values set on playback actions
			m_IsPlaying = false;
			m_IsPaused = false;

			// Set to null the callbacks
			m_OnUpdate = null;
			m_OnUpdateProgress = null;
			m_OnComplete = null;

		}

		/// <summary>
		/// Sets the duration of the timer.
		/// </summary>
		/// <returns>The timer object.</returns>
		/// <param name="duration">The duration.</param>
		public Timer SetDuration(float duration) {
			m_Duration = duration;
			return this;
		}

		/// <summary>
		/// Sets the <see cref="TimeMode"/> of the timer.
		/// </summary>
		/// <param name="timeMode"></param>
		/// <returns></returns>
		public Timer SetTimeMode(TimeMode timeMode) {
			m_TimeMode = timeMode;
			return (Timer)this;
		}

		/// <summary>
		/// Sets a callback that will be called every frame while the timer is playing 
		/// and not paused.
		/// </summary>
		/// <returns>The timer object.</returns>
		/// <param name="onUpdate">The action to be invoked on update.</param>
		public Timer SetOnUpdate(Action onUpdate) {
			m_OnUpdateProgress = null;
			m_OnUpdate = onUpdate;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called every frame while the timer is playing 
		/// and not paused. <paramref name="onUpdate"/> receives the <c>progress</c> as
		/// parameter.
		/// </summary>
		/// <returns>The timer object.</returns>
		/// <param name="onUpdate">The action to be invoked on update.</param>
		public Timer SetOnUpdate(Action<float> onUpdate) {
			m_OnUpdate = null;
			m_OnUpdateProgress = onUpdate;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the timer completes.
		/// </summary>
		/// <returns>The timer object.</returns>
		/// <param name="onComplete">The action to be invoked on complete.</param>
		public Timer SetOnComplete(Action onComplete) {
			m_OnComplete = onComplete;
			return this;
		}

		/// <summary>
		/// Invokes the <c>OnUpdate</c> callback.
		/// </summary>
		public void OnUpdate() {
			m_OnUpdate?.Invoke();
			m_OnUpdateProgress?.Invoke(Progress);
		}

		/// <summary>
		/// Invokes the <c>OnComplete</c> callback.
		/// </summary>
		public void OnComplete() {
			m_OnComplete?.Invoke();
		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private MonoBehaviour m_MonoBehaviour;

		[NonSerialized]
		private Coroutine m_Coroutine;

		[NonSerialized]
		private TimeMode m_TimeMode;

		[NonSerialized]
		private Action m_OnUpdate;

		[NonSerialized]
		private Action<float> m_OnUpdateProgress;

		[NonSerialized]
		private Action m_OnComplete;

		[NonSerialized]
		private float m_CurrentTime;

		[NonSerialized]
		private float m_Duration;

		[NonSerialized]
		private float m_Progress;

		[NonSerialized]
		private bool m_IsPlaying;

		[NonSerialized]
		private bool m_IsPaused;

		#endregion


		#region Private Properties

		private float DeltaTime {
			get {
				switch (m_TimeMode) {
					case TimeMode.Normal: return Time.deltaTime;
					case TimeMode.Unscaled: return Time.unscaledDeltaTime;
					case TimeMode.Smooth: return Time.smoothDeltaTime;
					case TimeMode.Fixed: return Time.fixedDeltaTime;
					case TimeMode.FixedUnscaled: return Time.fixedUnscaledTime;
					default: return Time.deltaTime;
				}
			}
		}

		#endregion


		#region Internal Methods

		private IEnumerator _Play() {

			m_CurrentTime = 0;
			m_Progress = 0;

			// Wait one frame for the properties to be ready, in case the timer is
			// created and started in the same line.
			yield return null;

			while (true) {
				if (!IsPaused) {

					// Add the time at the beginning because one frame has already happened
					m_CurrentTime += DeltaTime;

					// This avoids progress to be greater than 1
					if (m_CurrentTime > m_Duration) {
						m_CurrentTime = m_Duration;
						Progress = 1;
						break;
					}

					Progress = m_CurrentTime / m_Duration;

					OnUpdate();

				}
				yield return null;
			}

			OnUpdate();

			// Set the coroutine to null before calling m_OnComplete() because m_OnComplete()
			// may start another animation with the same timer object and we don't 
			// want to set the coroutine to null just after starting the new animation.
			m_Coroutine = null;
			m_IsPlaying = false;
			m_CurrentTime = 0;

			OnComplete();

		}

		private void StopCoroutine() {
			if (m_Coroutine != null) {
				m_MonoBehaviour.StopCoroutine(m_Coroutine);
				m_Coroutine = null;
			}
		}

		#endregion


	}

}