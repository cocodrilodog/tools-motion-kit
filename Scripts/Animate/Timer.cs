namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// A timer that works exactly as a <see cref="MotionBase{ValueT, MotionT}"/>
	/// but without affecting any property.
	/// </summary>
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
		/// <remarks>
		///	It updates the <see cref="CurrentTime"/> consistently.
		/// </remarks>
		/// <value>The progress.</value>
		public float Progress {
			get { return m_Progress; }
			set {
				SetProgress(value, false);
			}
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

		public bool Started {
			get => m_Started;
			set {
				if (value != m_Started) {
					m_Started = value;
					if (m_Started) {
						InvokeOnStart();
					}
				}
			}
		}

		public bool Completed {
			get => m_Completed;
			set {
				if (value != m_Completed) {
					m_Completed = value;
					if (m_Completed) {
						InvokeOnComplete();
					}
				}
			}
		}

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
			if (IsPlaying) {
				InvokeOnInterrupt();
			}
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
			if (IsPlaying) {
				InvokeOnInterrupt();
			}
			StopCoroutine();
			m_IsPlaying = false;
			ResetState();
		}

		public void SetProgress(float progress, bool invokeCallbacks) {
			CheckDisposed();
			m_Progress = progress;
			m_CurrentTime = m_Progress * m_Duration;
			UpdateState(invokeCallbacks);
		}

		/// <summary>
		/// Resets the timer to its default state.
		/// </summary>
		/// <returns>The timer object.</returns>
		public void Dispose() {
			m_IsDisposed = true;
			StopCoroutine();
			Clean(CleanFlag.All);
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
		/// Sets a callback that will be called when the timer starts on <c>Play</c>.
		/// </summary>
		/// <returns>The timer object.</returns>
		/// <param name="onStart">The action to be invoked on start.</param>
		public Timer SetOnStart(Action onStart) {
			m_OnStartTimer = null;
			m_OnStart = onStart;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the timer starts on <c>Play</c>. 
		/// The callback receives the timer object as a parameter.
		/// </summary>
		/// <returns>The timer object.</returns>
		/// <param name="onStart">The action to be invoked on start.</param>
		public Timer SetOnStart(Action<Timer> onStart) {
			m_OnStart = null;
			m_OnStartTimer = onStart;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called every frame while the timer is playing 
		/// and not paused.
		/// </summary>
		/// <returns>The timer object.</returns>
		/// <param name="onUpdate">The action to be invoked on update.</param>
		public Timer SetOnUpdate(Action onUpdate) {
			m_OnUpdateTimer = null;
			m_OnUpdate = onUpdate;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called every frame while the timer is playing 
		/// and not paused. The callback receives the timer object as a parameter.
		/// </summary>
		/// <returns>The timer object.</returns>
		/// <param name="onUpdate">The action to be invoked on update.</param>
		public Timer SetOnUpdate(Action<Timer> onUpdate) {
			m_OnUpdate = null;
			m_OnUpdateTimer = onUpdate;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the timer is interrupted by being played
		/// or stopped while <see cref="IsPlaying"/><c> == true</c>.
		/// </summary>
		/// <param name="onInterrupt">The action to be invoked on interrupt.</param>
		/// <returns>The motion object.</returns>
		public Timer SetOnInterrupt(Action onInterrupt) {
			m_OnInterruptTimer = null;
			m_OnInterrupt = onInterrupt;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the timer is interrupted by being played
		/// or stopped while <see cref="IsPlaying"/><c> == true</c>. The callback receives the 
		/// timer object as a parameter.
		/// </summary>
		/// <param name="onInterrupt">The action to be invoked on interrupt.</param>
		/// <returns>The motion object.</returns>
		public Timer SetOnInterrupt(Action<Timer> onInterrupt) {
			m_OnInterrupt = null;
			m_OnInterruptTimer = onInterrupt;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the timer completes.
		/// </summary>
		/// <returns>The timer object.</returns>
		/// <param name="onComplete">The action to be invoked on complete.</param>
		public Timer SetOnComplete(Action onComplete) {
			m_OnCompleteTimer = null;
			m_OnComplete = onComplete;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the timer completes. The callback
		/// receives the timer object as a parameter.
		/// </summary>
		/// <returns>The motion object.</returns>
		/// <param name="onComplete">The action to be invoked on complete.</param>
		public Timer SetOnComplete(Action<Timer> onComplete) {
			m_OnComplete = null;
			m_OnCompleteTimer = onComplete;
			return this;
		}

		/// <summary>
		/// Resets the state of this timer.
		/// </summary>
		public void ResetState() {
			m_Started = false;
			m_UpdateTime = -1;
			m_Completed = false;
		}

		/// <summary>
		/// Invokes the <c>OnInterrupt</c> callback.
		/// </summary>
		public void InvokeOnInterrupt() {
			if (m_InvokeCallbacks) {
				m_OnInterrupt?.Invoke();
				m_OnInterruptTimer?.Invoke(this);
			}
		}

		/// <summary>
		/// Sets <c>OnStart</c>, <c>OnUpdate</c>, <c>OnInterrupt</c> and/or <c>OnComplete</c> to null.
		/// </summary>
		/// <param name="cleanFlag">The clean flags.</param>
		public Timer Clean(CleanFlag cleanFlags) {
			if ((cleanFlags & CleanFlag.OnStart) == CleanFlag.OnStart) { SetOnStart((Action)null); }
			if ((cleanFlags & CleanFlag.OnUpdate) == CleanFlag.OnUpdate) { SetOnUpdate((Action)null); }
			if ((cleanFlags & CleanFlag.OnInterrupt) == CleanFlag.OnInterrupt) { SetOnInterrupt((Action)null); }
			if ((cleanFlags & CleanFlag.OnComplete) == CleanFlag.OnComplete) { SetOnComplete((Action)null); }
			if ((cleanFlags & CleanFlag.All) == CleanFlag.All) {
				Clean(CleanFlag.OnStart | CleanFlag.OnUpdate | CleanFlag.OnInterrupt | CleanFlag.OnComplete);
			}
			return this;
		}

		/// <summary>
		/// Does nothing. For interface compliance only.
		/// </summary>
		/// <param name="animatableElement"></param>
		public void SetAnimatableElement(object animatableElement) { }

		#endregion


		#region Private Fields

		[NonSerialized]
		private MonoBehaviour m_MonoBehaviour;

		[NonSerialized]
		private Coroutine m_Coroutine;

		[NonSerialized]
		private TimeMode m_TimeMode;

		[NonSerialized]
		private Action m_OnStart;

		[NonSerialized]
		private Action<Timer> m_OnStartTimer;

		[NonSerialized]
		private Action m_OnUpdate;

		[NonSerialized]
		private Action<Timer> m_OnUpdateTimer;

		[NonSerialized]
		private Action m_OnInterrupt;

		[NonSerialized]
		private Action<Timer> m_OnInterruptTimer;

		[NonSerialized]
		private Action m_OnComplete;

		[NonSerialized]
		private Action<Timer> m_OnCompleteTimer;

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

		[NonSerialized]
		private bool m_Started;

		[NonSerialized]
		private float m_UpdateTime;

		[NonSerialized]
		private bool m_Completed;

		[NonSerialized]
		private bool m_InvokeCallbacks;

		[NonSerialized]
		private bool m_IsDisposed;

		#endregion


		#region Private Properties

		private float DeltaTime => AnimateUtility.GetDeltaTime(m_TimeMode);

		private float _Time => AnimateUtility.GetTime(m_TimeMode);

		#endregion


		#region Private Methods

		private IEnumerator _Play() {

			ResetState();

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
						SetProgress(1, true);
						break;
					}

					SetProgress(m_CurrentTime / m_Duration, true);

				}
				yield return null;
			}

			ResetState();

		}

		private void StopCoroutine() {
			if (m_Coroutine != null) {
				if (m_MonoBehaviour != null) {
					m_MonoBehaviour.StopCoroutine(m_Coroutine);
				}
				m_Coroutine = null;
			}
		}

		private void UpdateState(bool invokeCallbacks) {
			m_InvokeCallbacks = invokeCallbacks;
			if (Progress >= 0) {
				Started = true;
			}
			if (Started && !Completed) {
				Update();
			}
			if (Progress >= 1) {

				// Set the coroutine to null before calling m_OnComplete() because m_OnComplete()
				// may start another animation with the same timer object and we don't 
				// want to set the coroutine to null just after starting the new animation.
				m_Coroutine = null;
				m_IsPlaying = false;
				m_CurrentTime = 0;

				Completed = true;

			}
			m_InvokeCallbacks = true;
		}

		private void Update() {
			var time = _Time;
			if (!Mathf.Approximately(time, m_UpdateTime)) {
				m_UpdateTime = _Time;
				InvokeOnUpdate();
			}
		}

		/// <summary>
		/// Invokes the <c>OnStart</c> callback.
		/// </summary>
		private void InvokeOnStart() {
			if (m_InvokeCallbacks) {
				m_OnStart?.Invoke();
				m_OnStartTimer?.Invoke(this);
			}
		}

		/// <summary>
		/// Invokes the <c>OnUpdate</c> callback.
		/// </summary>
		private void InvokeOnUpdate() {
			if (m_InvokeCallbacks) {
				m_OnUpdate?.Invoke();
				m_OnUpdateTimer?.Invoke(this);
			}
		}

		/// <summary>
		/// Invokes the <c>OnComplete</c> callback.
		/// </summary>
		private void InvokeOnComplete() {
			if (m_InvokeCallbacks) {
				m_OnComplete?.Invoke();
				m_OnCompleteTimer?.Invoke(this);
			}
		}

		private void CheckDisposed() {
			if (m_IsDisposed) {
				throw new InvalidOperationException(
					"This timer has been disposed, it can not be used anymore."
				);
			}
		}

		#endregion


	}

}