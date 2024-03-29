﻿namespace CocodriloDog.MotionKit {

	using System;
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// The object that defines an animation of a property from value A to value B.
	/// </summary>
	public abstract class MotionBase<ValueT, MotionT> : IPlayback, ITimedProgressable
		where MotionT : MotionBase<ValueT, MotionT> {


		#region Small Types

		/// <summary>
		/// The setter of the property to animate.
		/// </summary>
		/// 
		/// <remarks>
		/// Used as parameter at the contructor of a Motion object.
		/// </remarks>
		public delegate void Setter(ValueT value);

		/// <summary>
		/// The easing function that will be used fo the animation.
		/// </summary>
		public delegate ValueT Easing(ValueT a, ValueT b, float t);

		#endregion


		#region Public Properties

		/// <summary>
		/// Gets the initial value of this motion.
		/// </summary>
		/// 
		/// <remarks>
		/// It is set at <see cref="Play(ValueT, ValueT, float)"/> or alternatively
		/// at <see cref="SetInitialValue(ValueT)"/>
		/// </remarks>
		/// 
		/// <value>The initial value.</value>
		public ValueT InitialValue { get { return m_InitialValue; } }

		/// <summary>
		/// Gets the final value of this motion.
		/// </summary>
		/// 
		/// <remarks>
		/// It is set at <see cref="Play(ValueT, ValueT, float)"/> or alternatively
		/// at <see cref="SetFinalValue(ValueT)"/>
		/// </remarks>
		/// 
		/// <value>The final value.</value>
		public ValueT FinalValue { get { return m_FinalValue; } }

		/// <summary>
		/// The current time of this motion.
		/// </summary>
		/// 
		/// <remarks> 
		/// It is calculated inside of the coroutine while the animation is playing
		/// </remarks>
		/// 
		/// <value>The time.</value>
		public float CurrentTime { get { return m_CurrentTime; } }

		/// <summary>
		/// Gets the duration of this motion.
		/// </summary>
		/// 
		/// <remarks>
		/// It is set at <see cref="Play(ValueT, ValueT, float)"/>, 
		/// <see cref="Play(ValueT, ValueT, float)"/> or alternatively
		/// at <see cref="SetDuration(float)"/>
		/// </remarks>
		/// 
		/// <value>The duration.</value>
		public float Duration { get { return m_Duration; } }

		/// <summary>
		/// Gets and sets the progress from 0 to 1 on this motion.
		/// </summary>
		/// 
		/// <remarks>
		/// it will use <see cref=" InitialValue"/> and <see cref="FinalValue"/>
		/// as the values to interpolate while paused or even while not playing
		/// at all.
		///	It updates the <see cref="CurrentTime"/> consistently.
		/// </remarks>
		/// 
		/// <value>The progress.</value>
		public float Progress {
			get { return m_Progress; }
			set {
				SetProgress(value, false);
			}
		}

		/// <summary>
		/// Is the motion playing?
		/// </summary>
		/// <value><c>true</c> if is playing; otherwise, <c>false</c>.</value>
		public bool IsPlaying { get { return m_IsPlaying; } }

		/// <summary>
		/// Is the motion paused?
		/// </summary>
		/// <value><c>true</c> if is paused; otherwise, <c>false</c>.</value>
		public bool IsPaused { get { return m_IsPaused; } }

		#endregion


		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MotionBase{ValueT, MotionT}"/> 
		/// class.
		/// </summary>
		/// 
		/// <param name="monoBehaviour">
		/// A <see cref="MonoBehaviour"/> that will start the coroutines.
		/// </param>
		/// 
		/// <param name="setter">
		/// The settter of the property to animate.
		/// </param>
		public MotionBase(MonoBehaviour monoBehaviour, Setter setter) {
			m_MonoBehaviour = monoBehaviour;
			m_Setter = setter;
		}

		#endregion


		#region Public Methods


		/// <summary>
		/// Plays the motion that animates the intended property from its current value or
		/// the <see cref="InitialValue"/> to the <see cref="FinalValue"/> during the given
		/// <see cref="Duration"/>
		/// </summary>
		/// 
		/// <remarks>
		/// <see cref="InitialValue"/>, <see cref=" FinalValue"/> and <see cref="Duration"/>
		/// should have been set before via <see cref="SetInitialValue(ValueT)"/>, 
		/// <see cref="SetFinalValue(ValueT)"/> and <see cref="SetDuration(float)"/> 
		/// </remarks>
		/// 
		/// <returns>The motion object.</returns>
		public MotionT Play() {
			return Play(InitialValue, FinalValue, Duration);
		}

		/// <summary>
		/// Plays the motion that animates the intended property from the <c>initialValue</c>
		/// to the specified <c>finalValue</c> during the given <c>duration</c>. 
		/// </summary>
		/// 
		/// <returns>The motion object.</returns>
		/// <param name="initialValue">Initial value.</param>
		/// <param name="finalValue">Final value.</param>
		/// <param name="duration">Duration.</param>
		public MotionT Play(ValueT initialValue, ValueT finalValue, float duration) {
			if (IsPlaying) {
				InvokeOnInterrupt();
			}
			StopCoroutine();
			m_InitialValue = initialValue;
			m_FinalValue = finalValue;
			m_Duration = duration;
			m_IsPaused = false;
			m_Coroutine = m_MonoBehaviour.StartCoroutine(_Play());
			m_IsPlaying = true;
			return (MotionT)this;
		}

		/// <summary>
		/// Pauses the motion.
		/// </summary>
		/// <returns>The motion object.</returns>
		public MotionT Pause() {
			m_IsPaused = true;
			return (MotionT)this;
		}

		/// <summary>
		/// Resumes this motion.
		/// </summary>
		/// <returns>The motion object.</returns>
		public MotionT Resume() {
			// Progress may have changed from outside so we update m_CurrentTime here.
			m_CurrentTime = m_Progress * m_Duration;
			m_IsPaused = false;
			return (MotionT)this;
		}

		/// <summary>
		/// Stops the motion.
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
			m_Progress = progress;
			m_CurrentTime = m_Progress * m_Duration;
			UpdateState(invokeCallbacks);
		}

		/// <summary>
		/// Resets the motion to its default state.
		/// </summary>
		/// <returns>The motion object.</returns>
		public void Dispose() {
			m_IsDisposed = true;
			m_Setter = null;
			StopCoroutine();
			Clean(CleanFlag.All);
		}

		/// <summary>
		/// Sets the initial value of the motion.
		/// </summary>
		/// <returns>The motion object.</returns>
		/// <param name="initialValue">The initial value.</param>
		public MotionT SetInitialValue(ValueT initialValue) {
			m_InitialValue = initialValue;
			return (MotionT)this;
		}

		/// <summary>
		/// Sets the final value of the motion.
		/// </summary>
		/// <returns>The motion object.</returns>
		/// <param name="finalValue">The final value.</param>
		public MotionT SetFinalValue(ValueT finalValue) {
			m_FinalValue = finalValue;
			return (MotionT)this;
		}

		/// <summary>
		/// Sets the duration of the motion.
		/// </summary>
		/// <returns>The motion object.</returns>
		/// <param name="duration">The duration.</param>
		public MotionT SetDuration(float duration) {
			m_Duration = duration;
			return (MotionT)this;
		}

		/// <summary>
		/// Shortcut to set <c>initialValue</c>, <c>finalValue</c> and <c>duration</c>
		/// in one call.
		/// </summary>
		/// <param name="initialValue">The initial value of the motion.</param>
		/// <param name="finalValue">The final value of the motion.</param>
		/// <param name="duration">The duration of the motion.</param>
		/// <returns></returns>
		public MotionT SetValuesAndDuration(ValueT initialValue, ValueT finalValue, float duration) {
			m_InitialValue = initialValue;
			m_FinalValue = finalValue;
			m_Duration = duration;
			return (MotionT)this;
		}

		/// <summary>
		/// Sets the <see cref="TimeMode"/> of the motion.
		/// </summary>
		/// <param name="timeMode"></param>
		/// <returns></returns>
		public MotionT SetTimeMode(TimeMode timeMode) {
			m_TimeMode = timeMode;
			return (MotionT)this;
		}

		/// <summary>
		/// Sets the easing of the motion.
		/// </summary>
		/// 
		/// <remarks>
		/// This must be a function that interpolates two values <c>a</c> and <c>b</c>
		/// with a progress number from 0 to 1 <c>t</c>
		/// </remarks>
		/// 
		/// <returns>The motion object.</returns>
		/// <param name="easing">Easing.</param>
		public MotionT SetEasing(Easing easing) {
			m_Easing = easing;
			return (MotionT)this;
		}

		/// <summary>
		/// Sets a callback that will be called when the motion starts on <c>Play</c>.
		/// </summary>
		/// <returns>The motion object.</returns>
		/// <param name="onStart">The action to be invoked on start.</param>
		public MotionT SetOnStart(Action onStart) {
			m_OnStartMotion = null;
			m_OnStart = onStart;
			return (MotionT)this;
		}

		/// <summary>
		/// Sets a callback that will be called when the motion starts on <c>Play</c>. 
		/// The callback receives the motion object as a parameter.
		/// </summary>
		/// <returns>The motion object.</returns>
		/// <param name="onStart">The action to be invoked on start.</param>
		public MotionT SetOnStart(Action<MotionT> onStart) {
			m_OnStart = null;
			m_OnStartMotion = onStart;
			return (MotionT)this;
		}

		/// <summary>
		/// Sets a callback that will be called every frame while the motion is playing 
		/// and not paused.
		/// </summary>
		/// <returns>The motion object.</returns>
		/// <param name="onUpdate">The action to be invoked on update.</param>
		public MotionT SetOnUpdate(Action onUpdate) {
			m_OnUpdateMotion = null;
			m_OnUpdate = onUpdate;
			return (MotionT)this;
		}

		/// <summary>
		/// Sets a callback that will be called every frame while the motion is playing 
		/// and not paused. The callback receives the motion object as a parameter.
		/// </summary>
		/// <returns>The motion object.</returns>
		/// <param name="onUpdate">The action to be invoked on update.</param>
		public MotionT SetOnUpdate(Action<MotionT> onUpdate) {
			m_OnUpdate = null;
			m_OnUpdateMotion = onUpdate;
			return (MotionT)this;
		}

		/// <summary>
		/// Sets a callback that will be called when the motion is interrupted by being played
		/// or stopped while <see cref="IsPlaying"/><c> == true</c>.
		/// </summary>
		/// <param name="onInterrupt">The action to be invoked on interrupt.</param>
		/// <returns>The motion object.</returns>
		public MotionT SetOnInterrupt(Action onInterrupt) {
			m_OnInterruptMotion = null;
			m_OnInterrupt = onInterrupt;
			return (MotionT)this;
		}

		/// <summary>
		/// Sets a callback that will be called when the motion is interrupted by being played
		/// or stopped while <see cref="IsPlaying"/><c> == true</c>. The callback receives the 
		/// motion object as a parameter.
		/// </summary>
		/// <param name="onInterrupt">The action to be invoked on interrupt.</param>
		/// <returns>The motion object.</returns>
		public MotionT SetOnInterrupt(Action<MotionT> onInterrupt) {
			m_OnInterrupt = null;
			m_OnInterruptMotion = onInterrupt;
			return (MotionT)this;
		}

		/// <summary>
		/// Sets a callback that will be called when the motion completes its animation.
		/// </summary>
		/// <returns>The motion object.</returns>
		/// <param name="onComplete">The action to be invoked on complete.</param>
		public MotionT SetOnComplete(Action onComplete) {
			m_OnCompleteMotion = null;
			m_OnComplete = onComplete;
			return (MotionT)this;
		}

		/// <summary>
		/// Sets a callback that will be called when the motion completes its animation.
		/// The callback receives the motion object as a parameter.
		/// </summary>
		/// <returns>The motion object.</returns>
		/// <param name="onComplete">The action to be invoked on complete.</param>
		public MotionT SetOnComplete(Action<MotionT> onComplete) {
			m_OnComplete = null;
			m_OnCompleteMotion = onComplete;
			return (MotionT)this;
		}

		/// <summary>
		/// Resets the state of this Motion.
		/// </summary>
		public void ResetState() {
			m_CurrentTime = 0;
			m_Progress = 0;
			m_Started = false;
			m_Completed = false;
		}

		/// <summary>
		/// Invokes the <c>OnInterrupt</c> callback.
		/// </summary>
		public void InvokeOnInterrupt() {
			if (m_InvokeCallbacks) {
				m_OnInterrupt?.Invoke();
				m_OnInterruptMotion?.Invoke(this as MotionT);
			}
		}

		/// <summary>
		/// Sets <c>Easing</c>, <c>OnStart</c>, <c>OnUpdate</c>, <c>OnInterrupt</c> and/or <c>OnComplete</c> to null.
		/// </summary>
		/// <param name="cleanFlags">The clean flags.</param>
		public MotionT Clean(CleanFlag cleanFlags) {
			if ((cleanFlags & CleanFlag.Easing) == CleanFlag.Easing) { SetEasing(null); }
			if ((cleanFlags & CleanFlag.OnStart) == CleanFlag.OnStart) { SetOnStart((Action)null); }
			if ((cleanFlags & CleanFlag.OnUpdate) == CleanFlag.OnUpdate) { SetOnUpdate((Action)null); }
			if ((cleanFlags & CleanFlag.OnInterrupt) == CleanFlag.OnInterrupt) { SetOnInterrupt((Action)null); }
			if ((cleanFlags & CleanFlag.OnComplete) == CleanFlag.OnComplete) { SetOnComplete((Action)null); }
			if ((cleanFlags & CleanFlag.All) == CleanFlag.All) { 
				Clean(CleanFlag.Easing | CleanFlag.OnStart | CleanFlag.OnUpdate | CleanFlag.OnInterrupt | CleanFlag.OnComplete); 
			}
			return (MotionT)this;
		}

		/// <summary>
		/// Changes the setter.
		/// </summary>
		/// <param name="animatableElement">The setter.</param>
		public void SetAnimatableElement(object animatableElement) {
			if(!(animatableElement is Setter)) {
				throw new ArgumentException(
					$"The animatableElement: {animatableElement.GetType()} is not a {m_Setter.GetType()}"
				);
			}
			m_Setter = (Setter)animatableElement;
		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private MonoBehaviour m_MonoBehaviour;

		[NonSerialized]
		private Setter m_Setter;

		[NonSerialized]
		private Coroutine m_Coroutine;

		[NonSerialized]
		private TimeMode m_TimeMode;

		[NonSerialized]
		private Easing m_Easing;

		[NonSerialized]
		private Easing m_DefaultEasing;

		[NonSerialized]
		private Action m_OnStart;

		[NonSerialized]
		private Action<MotionT> m_OnStartMotion;

		[NonSerialized]
		private Action m_OnUpdate;

		[NonSerialized]
		private Action<MotionT> m_OnUpdateMotion;

		[NonSerialized]
		private Action m_OnInterrupt;

		[NonSerialized]
		private Action<MotionT> m_OnInterruptMotion;

		[NonSerialized]
		private Action m_OnComplete;

		[NonSerialized]
		private Action<MotionT> m_OnCompleteMotion;

		[NonSerialized]
		private ValueT m_InitialValue;

		[NonSerialized]
		private ValueT m_FinalValue;

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
		private bool m_Completed;

		[NonSerialized]
		private bool m_InvokeCallbacks;

		[NonSerialized]
		private bool m_IsDisposed;

		#endregion


		#region Private Properties

		private Easing DefaultEasing {
			get { return m_DefaultEasing = m_DefaultEasing ?? GetDefaultEasing(); }
		}

		private float DeltaTime => MotionKitUtility.GetDeltaTime(m_TimeMode);

		private bool Started {
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

		private bool Completed {
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


		#region Private Methods

		/// <summary>
		/// Implement this in subclasses to provide a default easing function.
		/// </summary>
		/// <returns>The default easing.</returns>
		protected abstract Easing GetDefaultEasing();

		private IEnumerator _Play() {

			ResetState();

			// Wait one frame for the properties to be ready, in case the motion is
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

		private void ApplyProgress() {
			CheckDisposed();
			if (m_Easing != null) {
				m_Setter(m_Easing(m_InitialValue, m_FinalValue, m_Progress));
			} else {
				m_Setter(DefaultEasing(m_InitialValue, m_FinalValue, m_Progress));
			}
		}

		private void UpdateState(bool invokeCallbacks) {
			m_InvokeCallbacks = invokeCallbacks;
			if (Progress >= 0) {
				Started = true;
			}
			if (Started && !Completed) {
				// Putting ApplyProgress() here solves the issue of the target property being changed
				// before OnStart. Now OnStart is invoked before the motion starts modifying the property
				ApplyProgress();
				InvokeOnUpdate();
			}
			if (Progress >= 1) {

				// Set the coroutine to null before calling m_OnComplete() because m_OnComplete()
				// may start another animation with the same motion object and we don't 
				// want to set the coroutine to null just after starting the new animation.
				m_Coroutine = null;
				m_IsPlaying = false;
				m_CurrentTime = 0;

				// Don't reset the state here because we need this to remain complete as long
				// as it is part of a sequence.
				Completed = true;

			} else {
				// This is useful if the animation is played in reverse
				Completed = false;
			}
			m_InvokeCallbacks = true;
		}

		/// <summary>
		/// Invokes the <c>OnStart</c> callback.
		/// </summary>
		private void InvokeOnStart() {
			if (m_InvokeCallbacks) {
				m_OnStart?.Invoke();
				m_OnStartMotion?.Invoke(this as MotionT);
			}
		}

		/// <summary>
		/// Invokes the <c>OnUpdate</c> callback.
		/// </summary>
		private void InvokeOnUpdate() {
			if (m_InvokeCallbacks) {
				m_OnUpdate?.Invoke();
				m_OnUpdateMotion?.Invoke(this as MotionT);
			}
		}

		/// <summary>
		/// Invokes the <c>OnComplete</c> callback.
		/// </summary>
		private void InvokeOnComplete() {
			if (m_InvokeCallbacks) {
				m_OnComplete?.Invoke();
				m_OnCompleteMotion?.Invoke(this as MotionT);
			}
		}

		private void CheckDisposed() {
			if (m_IsDisposed) {
				throw new InvalidOperationException(
					"This motion has been disposed, it can not be used anymore."
				);
			}
		}

		#endregion


	}
}