namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// A sequence of <see cref="MotionBase{ValueT, MotionT}"/>, <see cref="Timer"/>
	/// and <see cref="Sequence"/> objects.
	/// </summary>
	public class Sequence : IPlayback, ITimedProgressable {


		#region Small Types

		/// <summary>
		/// Object that stores sequence items info
		/// </summary>
		public class SequenceItemInfo {

			/// <summary>
			/// The index of the item.
			/// </summary>
			public int Index;

			/// <summary>
			/// The item.
			/// </summary>
			public ITimedProgressable Item;

			/// <summary>
			/// The position in time of the item.
			/// </summary>
			public float Position;

			/// <summary>
			/// Has this item started?
			/// </summary>
			public bool Started;

			/// <summary>
			/// Has this item completed?
			/// </summary>
			public bool Completed;

			public SequenceItemInfo() { }

			public SequenceItemInfo(int index, ITimedProgressable item) {
				Index = index;
				Item = item;
			}

		}

		/// <summary>
		/// The easing function that will be used for the sequence.
		/// </summary>
		/// 
		/// <remarks>
		/// Used as parameter at the contructor of a Motion object.
		/// </remarks>
		public delegate float Easing(float a, float b, float t);

		#endregion


		#region Public Properties

		/// <summary>
		/// The current time of this sequence.
		/// </summary>
		/// 
		/// <remarks> 
		/// It is calculated inside of the coroutine while the sequence is playing
		/// </remarks>
		/// 
		/// <value>The time.</value>
		public float CurrentTime { get { return m_CurrentTime; } }

		/// <summary>
		/// Gets the duration of this sequence.
		/// </summary>
		/// 
		/// <remarks>
		/// It is set at <see cref="Play(float)"/> or alternatively at 
		/// <see cref="SetDuration(float)"/>. By default the <see cref="Duration"/> 
		/// will be the same as <see cref="SequenceDuration"/>, but it can be set to
		/// another value, which will scale the individual duration of the sequence
		/// items to fit the assigned <see cref="Duration"/>.
		/// </remarks>
		/// 
		/// <value>The duration.</value>
		public float Duration { get { return m_Duration; } }

		/// <summary>
		/// The sum of the sequence items duration.
		/// </summary>
		public float SequenceDuration { get { return m_SequenceDuration; } }

		/// <summary>
		/// Gets and sets the progress from 0 to 1 on this sequence.
		/// </summary>
		/// <remarks>
		///	It updates the <see cref="CurrentTime"/> consistently.
		/// </remarks>
		/// <value>The progress.</value>
		public float Progress {
			get { return m_Progress; }
			set {
				m_Progress = Mathf.Clamp01(value);
				m_CurrentTime = m_Progress * m_Duration;
				SetProgressingItemInfo();
				ApplyProgress();
			}
		}

		/// <summary>
		/// Is the sequence playing?
		/// </summary>
		/// <value><c>true</c> if is playing; otherwise, <c>false</c>.</value>
		public bool IsPlaying { get { return m_IsPlaying; } }

		/// <summary>
		/// Is the sequence paused?
		/// </summary>
		/// <value><c>true</c> if is paused; otherwise, <c>false</c>.</value>
		public bool IsPaused { get { return m_IsPaused; } }

		#endregion


		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Sequence"/> class.
		/// </summary>
		/// <param name="monoBehaviour">
		/// A <see cref="MonoBehaviour"/> that will start the coroutines.
		/// </param>
		public Sequence(MonoBehaviour monoBehaviour, params ITimedProgressable[] sequenceItems) {
			m_MonoBehaviour = monoBehaviour;
			SetAnimatableElement(sequenceItems);
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Plays the sequence that will last the <see cref="Duration"/>.
		/// </summary>
		/// 
		/// <remarks>
		/// <see cref="Duration"/> should have been set before via 
		/// <see cref="SetDuration(float)"/>.
		/// </remarks>
		/// 
		/// <returns>The sequence object.</returns>
		public Sequence Play() {
			return Play(Duration);
		}

		/// <summary>
		/// Plays the sequence that will last the given <c>duration</c>. 
		/// </summary>
		///
		/// <remarks>
		/// <see cref="Duration"/> can be set alternatively at <see cref="SetDuration(float)"/>.
		/// By default the <see cref="Duration"/> will be the same as <see cref="SequenceDuration"/>,
		/// but it can be set to another value, which will scale the individual duration of the
		/// sequence items to fit the assigned <see cref="Duration"/>.
		/// </remarks>
		/// 
		/// <returns>The sequence object.</returns>
		/// <param name="duration">Duration.</param>
		public Sequence Play(float duration) {
			if (IsPlaying) {
				InvokeOnInterrupt();
			}
			StopCoroutine();
			m_Duration = duration;
			m_IsPaused = false;
			m_Coroutine = m_MonoBehaviour.StartCoroutine(_Play());
			m_IsPlaying = true;
			InvokeOnStart();
			return this;
		}

		/// <summary>
		/// Pauses the sequence.
		/// </summary>
		/// <returns>The sequence object.</returns>
		public Sequence Pause() {
			m_IsPaused = true;
			return this;
		}

		/// <summary>
		/// Resumes this sequence.
		/// </summary>
		/// <returns>The sequence object.</returns>
		public Sequence Resume() {
			// Progress may have changed from outside so we update m_CurrentTime here.
			m_CurrentTime = m_Progress * m_Duration;
			m_IsPaused = false;
			return this;
		}

		/// <summary>
		/// Stops the sequence.
		/// </summary>
		public void Stop() {
			if (IsPlaying) {
				InvokeOnInterrupt();
			}
			StopCoroutine();
			m_IsPlaying = false;
			m_ProgressingItemInfo = null;
			ResetItems();
		}

		/// <summary>
		/// Resets the sequence to its default state.
		/// </summary>
		public void Dispose() {

			m_IsDisposed = true;

			StopCoroutine();

			// Set to null the callbacks
			Clean(CleanFlag.Easing | CleanFlag.OnUpdate | CleanFlag.OnInterrupt | CleanFlag.OnComplete);

			// Sequence objects
			m_ProgressingItemInfo = null;
			ResetItems();

			// Reset all to avoid references that would prevent garbage collection
			foreach(SequenceItemInfo sequenceItemInfo in m_SequenceItemsInfo) {
				sequenceItemInfo.Item.Dispose();
			}

		}

		/// <summary>
		/// Sets the <see cref="Duration"/> of the sequence.
		/// </summary>
		/// 
		/// <remarks>
		/// It is can be set at <see cref="Play(float)"/> or through this method.
		/// By default the <see cref="Duration"/> will be the same as <see cref="SequenceDuration"/>,
		/// but it can be set to another value, which will scale the individual duration
		/// of the sequence items to fit the assigned <see cref="Duration"/>.
		/// </remarks>
		/// 
		/// <returns>The sequence object.</returns>
		/// <param name="duration">The duration.</param>
		public Sequence SetDuration(float duration) {
			m_Duration = duration;
			return this;
		}

		/// <summary>
		/// Sets the <see cref="TimeMode"/> of the sequence.
		/// </summary>
		/// <param name="timeMode"></param>
		/// <returns></returns>
		public Sequence SetTimeMode(TimeMode timeMode) {
			m_TimeMode = timeMode;
			return this;
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
		public Sequence SetEasing(Easing easing) {
			m_Easing = easing;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the sequence starts on <c>Play</c>.
		/// </summary>
		/// <returns>The sequence object.</returns>
		/// <param name="onStart">The action to be invoked on start.</param>
		public Sequence SetOnStart(Action onStart) {
			m_OnStartSequence = null;
			m_OnStart = onStart;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the sequence starts on <c>Play</c>. 
		/// The callback receives the sequence object as a parameter.
		/// </summary>
		/// <returns>The sequence object.</returns>
		/// <param name="onStart">The action to be invoked on start.</param>
		public Sequence SetOnStart(Action<Sequence> onStart) {
			m_OnStart = null;
			m_OnStartSequence = onStart;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called every frame while the sequence is playing 
		/// and not paused.
		/// </summary>
		/// <returns>The sequence object.</returns>
		/// <param name="onUpdate">The action to be invoked on update.</param>
		public Sequence SetOnUpdate(Action onUpdate) {
			m_OnUpdateSequence = null;
			m_OnUpdate = onUpdate;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called every frame while the sequence is playing 
		/// and not paused. The callback receives the sequence object as parameter.
		/// </summary>
		/// <returns>The sequence object.</returns>
		/// <param name="onUpdate">The action to be invoked on update.</param>
		public Sequence SetOnUpdate(Action<Sequence> onUpdate) {
			m_OnUpdate = null;
			m_OnUpdateSequence = onUpdate;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the sequence is interrupted by being played
		/// or stopped while <see cref="IsPlaying"/><c> == true</c>.
		/// </summary>
		/// <param name="onInterrupt">The action to be invoked on interrupt.</param>
		/// <returns>The motion object.</returns>
		public Sequence SetOnInterrupt(Action onInterrupt) {
			m_OnInterruptSequence = null;
			m_OnInterrupt = onInterrupt;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the sequence is interrupted by being played
		/// or stopped while <see cref="IsPlaying"/><c> == true</c>. The callback receives the 
		/// sequence object as a parameter.
		/// </summary>
		/// <param name="onInterrupt">The action to be invoked on interrupt.</param>
		/// <returns>The motion object.</returns>
		public Sequence SetOnInterrupt(Action<Sequence> onInterrupt) {
			m_OnInterrupt = null;
			m_OnInterruptSequence = onInterrupt;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the sequence completes.
		/// </summary>
		/// <returns>The sequence object.</returns>
		/// <param name="onComplete">The action to be invoked on complete.</param>
		public Sequence SetOnComplete(Action onComplete) {
			m_OnCompleteSequence = null;
			m_OnComplete = onComplete;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the sequence completes. The callback receives
		/// the sequence object as a parameter.
		/// </summary>
		/// <returns>The sequence object.</returns>
		/// <param name="onComplete">The action to be invoked on complete.</param>
		public Sequence SetOnComplete(Action<Sequence> onComplete) {
			m_OnComplete = null;
			m_OnCompleteSequence = onComplete;
			return this;
		}

		/// <summary>
		/// Updates <see cref="SequenceDuration"/> and assigns its value to <see cref="Duration"/>
		/// so that the default duration of the sequence results from its items. Additionally
		/// calculates the position in time of its items. 
		/// </summary>
		///
		/// <remarks>
		/// It should be called if the duration of any of its sequence items changes.
		/// </remarks>
		public void EvaluateSequence() {
			m_SequenceItemsInfo = new SequenceItemInfo[m_SequenceItems.Length];
			m_SequenceDuration = 0;
			for (int i = 0; i < m_SequenceItems.Length; i++) {
				m_SequenceItemsInfo[i] = new SequenceItemInfo(i, m_SequenceItems[i]);
				m_SequenceItemsInfo[i].Position = m_SequenceDuration;
				m_SequenceDuration += m_SequenceItemsInfo[i].Item.Duration;
			}
			m_Duration = m_SequenceDuration;
		}

		/// <summary>
		/// Invokes the <c>OnStart</c> callback.
		/// </summary>
		public void InvokeOnStart() {

			// OnStart of the first item
			if (m_SequenceItemsInfo.Length > 0) {
				if (!m_SequenceItemsInfo[0].Started) {
					m_SequenceItemsInfo[0].Item.InvokeOnStart();
					m_SequenceItemsInfo[0].Started = true;
				}
			}

			// OnStart on the sequence itself
			m_OnStart?.Invoke();
			m_OnStartSequence?.Invoke(this);

		}

		/// <summary>
		/// Invokes the <c>OnUpdate</c> callback.
		/// </summary>
		public void InvokeOnUpdate() {
			// Start/Complete the sequence items if time is past their start/end and they haven't 
			// been started/completed
			foreach (SequenceItemInfo itemInfo in m_SequenceItemsInfo) {

				float timeScale = m_Duration / m_SequenceDuration;
				float easedCurrentTime = EasedProgress * m_Duration;

				if (!itemInfo.Started) {
					float itemStart = itemInfo.Position * timeScale;
					if (easedCurrentTime >= itemStart) {
						itemInfo.Item.InvokeOnStart();
						itemInfo.Started = true;
					}
				}

				if (!itemInfo.Completed) {
					float itemEnd = (itemInfo.Position + itemInfo.Item.Duration) * timeScale;
					if (easedCurrentTime >= itemEnd) {
						itemInfo.Item.Progress = 1;
						itemInfo.Item.InvokeOnUpdate();
						itemInfo.Item.InvokeOnComplete();
						itemInfo.Completed = true;
					}
				}

			}
			// Update the progressing item as long as it haven't been completed
			if (m_ProgressingItemInfo != null && !m_ProgressingItemInfo.Completed) {
				m_ProgressingItemInfo.Item.InvokeOnUpdate();
			}
			// Update the sequence itself
			m_OnUpdate?.Invoke();
			m_OnUpdateSequence?.Invoke(this);
		}

		/// <summary>
		/// Invokes the <c>OnInterrupt</c> callback.
		/// </summary>
		public void InvokeOnInterrupt() {
			// Interrupt the progressing item as long as it haven't been completed.
			if (m_ProgressingItemInfo != null && !m_ProgressingItemInfo.Completed) {
				m_ProgressingItemInfo.Item.InvokeOnInterrupt();
			}
			// Interrupt the sequence itself
			m_OnInterrupt?.Invoke();
			m_OnInterruptSequence?.Invoke(this);
		}

		/// <summary>
		/// Invokes the <c>OnComplete</c> callback.
		/// </summary>
		public void InvokeOnComplete() {
			// Complete the progressing item as long as it haven't been completed.
			// At this point, the m_ProgressingItemInfo must be the last item.
			if (m_ProgressingItemInfo != null && !m_ProgressingItemInfo.Completed) {
				m_ProgressingItemInfo.Item.InvokeOnComplete();
			}
			// Complete the sequence itself
			m_OnComplete?.Invoke();
			m_OnCompleteSequence?.Invoke(this);
		}

		/// <summary>
		/// Sets <c>Easing</c>, <c>OnUpdate</c> and/or <c>OnComplete</c> to null;
		/// </summary>
		/// <param name="cleanFlag">The clean flags.</param>
		public void Clean(CleanFlag cleanFlag) {
			if ((cleanFlag & CleanFlag.Easing) == CleanFlag.Easing) { SetEasing(null); }
			if ((cleanFlag & CleanFlag.OnStart) == CleanFlag.OnStart) { SetOnStart((Action)null); }
			if ((cleanFlag & CleanFlag.OnUpdate) == CleanFlag.OnUpdate) { SetOnUpdate((Action)null); }
			if ((cleanFlag & CleanFlag.OnInterrupt) == CleanFlag.OnInterrupt) { SetOnInterrupt((Action)null); }
			if ((cleanFlag & CleanFlag.OnComplete) == CleanFlag.OnComplete) { SetOnComplete((Action)null); }
		}

		/// <summary>
		/// Changes sequence items.
		/// </summary>
		/// <param name="animatableElement"></param>
		public void SetAnimatableElement(object animatableElement) {

			if (!(animatableElement is ITimedProgressable[])) {
				throw new ArgumentException(
					$"The animatableElement: {animatableElement.GetType()} is not a {m_SequenceItems.GetType()}"
				);
			}

			ITimedProgressable[] sequenceItems = (ITimedProgressable[])animatableElement;
			for (int i = 0; i < sequenceItems.Length; i++) {
				if (Mathf.Approximately(sequenceItems[i].Duration, 0)) {
					throw new ArgumentException("Sequence items can not have duration equal to 0");
				}
			}

			m_SequenceItems = sequenceItems;
			EvaluateSequence();

		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private MonoBehaviour m_MonoBehaviour;

		[NonSerialized]
		private ITimedProgressable[] m_SequenceItems;

		[NonSerialized]
		private SequenceItemInfo[] m_SequenceItemsInfo;

		[NonSerialized]
		private float m_SequenceDuration;

		/// <summary>
		/// The currently progressing sequence item as calculated by
		/// <see cref="SetProgressingItemInfo()"/>.
		/// </summary>
		[NonSerialized]
		private SequenceItemInfo m_ProgressingItemInfo;

		[NonSerialized]
		private Coroutine m_Coroutine;

		[NonSerialized]
		private TimeMode m_TimeMode;

		[NonSerialized]
		private Easing m_Easing;

		[NonSerialized]
		private Action m_OnStart;

		[NonSerialized]
		private Action<Sequence> m_OnStartSequence;

		[NonSerialized]
		private Action m_OnUpdate;

		[NonSerialized]
		private Action<Sequence> m_OnUpdateSequence;

		[NonSerialized]
		private Action m_OnInterrupt;

		[NonSerialized]
		private Action<Sequence> m_OnInterruptSequence;

		[NonSerialized]
		private Action m_OnComplete;

		[NonSerialized]
		private Action<Sequence> m_OnCompleteSequence;

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
		private bool m_IsDisposed;

		#endregion


		#region Private Properties

		private float DeltaTime {
			get {
				switch (m_TimeMode) {
					case TimeMode.Normal: return Time.deltaTime;
					case TimeMode.Unscaled: return Time.unscaledDeltaTime;
					case TimeMode.Smooth: return Time.smoothDeltaTime;
					case TimeMode.Fixed: return Time.fixedDeltaTime;
					case TimeMode.FixedUnscaled: return Time.fixedUnscaledDeltaTime;
					default: return Time.deltaTime;
				}
			}
		}

		/// <summary>
		/// Internal version of <see cref="Progress"/> that doesn't update <see cref="m_CurrentTime"/>
		/// because it was updated in the coroutine.
		/// </summary>
		private float _Progress {
			get { return m_Progress; }
			set {
				m_Progress = Mathf.Clamp01(value);
				SetProgressingItemInfo();
				ApplyProgress();
			}
		}

		/// <summary>
		/// The <see cref="Progress"/>, but processed by <see cref="m_Easing"/>.
		/// </summary>
		private float EasedProgress {
			get {
				if (m_Easing != null) {
					return m_Easing(0, 1, m_Progress);
				} else {
					return m_Progress;
				}
			}
		}

		#endregion


		#region Private Methods

		private IEnumerator _Play() {

			m_CurrentTime = 0;
			m_Progress = 0;

			ResetItems();

			// Wait one frame for the properties to be ready, in case the sequence is
			// created and started in the same line.
			yield return null;

			while (true) {
				if (!IsPaused) {

					// Add the time at the beginning because one frame has already happened
					m_CurrentTime += DeltaTime;

					// This avoids progress to be greater than 1
					if (m_CurrentTime > m_Duration) {
						m_CurrentTime = m_Duration;
						_Progress = 1;
						break;
					}

					// Set the progress
					_Progress = m_CurrentTime / m_Duration;

					InvokeOnUpdate();

				}
				yield return null;
			}

			InvokeOnUpdate();

			// Set the coroutine to null before calling m_OnComplete() because m_OnComplete()
			// may start another animation with the same motion object and we don't 
			// want to set the coroutine to null just after starting the new animation.
			m_Coroutine = null;
			m_IsPlaying = false;
			m_CurrentTime = 0;

			InvokeOnComplete();

		}

		private void StopCoroutine() {
			if (m_Coroutine != null) {
				if (m_MonoBehaviour != null) {
					m_MonoBehaviour.StopCoroutine(m_Coroutine);
				}
				m_Coroutine = null;
			}
		}

		private void ResetItems() {
			foreach (SequenceItemInfo itemInfo in m_SequenceItemsInfo) {
				itemInfo.Completed = false;
				itemInfo.Started = false;
				if (itemInfo.Item is Sequence) {
					((Sequence)itemInfo.Item).ResetItems();
				}
			}
		}

		/// <summary>
		/// Sets the item of the sequence that coincides with <see cref="EasedProgress"/>
		/// </summary>
		private void SetProgressingItemInfo() {
			if (EasedProgress < 1) {
				// Progress translated to time units
				float timeOnSequence = EasedProgress * m_SequenceDuration;
				// Iterate through items
				for (int i = 0; i < m_SequenceItemsInfo.Length; i++) {
					if (Mathf.Approximately(m_SequenceItemsInfo[i].Item.Duration, 0)) {
						throw new ArgumentException("Sequence items can not have duration equal to 0");
					}
					if (timeOnSequence >= m_SequenceItemsInfo[i].Position &&
						timeOnSequence < m_SequenceItemsInfo[i].Position + m_SequenceItemsInfo[i].Item.Duration) {
						m_ProgressingItemInfo = m_SequenceItemsInfo[i];
						break;
					}
				}
			} else {
				// Handle progress == 1 (0 is already handled)
				m_ProgressingItemInfo = m_SequenceItemsInfo[m_SequenceItemsInfo.Length - 1];
			}
		}

		private void ApplyProgress() {

			CheckDisposed();

			float timeOnSequence = EasedProgress * m_SequenceDuration;

			// Wait until it has started, otherwise it may progress before
			// starting, which would lead to unexpected behaviour.
			if (m_ProgressingItemInfo.Started) {
				m_ProgressingItemInfo.Item.Progress =
					(timeOnSequence - m_SequenceItemsInfo[m_ProgressingItemInfo.Index].Position) /
					m_ProgressingItemInfo.Item.Duration;
			}

			// If the sequence is paused and the Progress is set to a point in time before, this updates
			// the following sequence items so that they are not marked as completed.
			for (int i = m_ProgressingItemInfo.Index; i < m_SequenceItemsInfo.Length; i++) {
				m_SequenceItemsInfo[i].Completed = false;
			}

		}

		private void CheckDisposed() {
			if (m_IsDisposed) {
				throw new InvalidOperationException(
					"This sequence has been disposed, it can not be used anymore."
				);
			}
		}

		#endregion


	}

}