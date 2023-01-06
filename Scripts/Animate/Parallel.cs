namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// A parallel structure that is used to play and control multiple <see cref="ITimedProgressable"/> objects at 
	/// the same time. It can be used to group <see cref="MotionBase{ValueT, MotionT}"/>, <see cref="Timer"/>,
	/// <see cref="Sequence"/> and <see cref="Parallel"/> objects.
	/// </summary>
	public class Parallel : IPlayback, ITimedProgressable {


		#region Small Types

		/// <summary>
		/// Object that stores parallel items info
		/// </summary>
		public class ParallelItemInfo {

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

			public ParallelItemInfo() { }

			public ParallelItemInfo(int index, ITimedProgressable item) {
				Index = index;
				Item = item;
			}

		}

		/// <summary>
		/// The easing function that will be used for the parallel.
		/// </summary>
		public delegate float Easing(float a, float b, float t);

		#endregion


		#region Public Properties

		/// <summary>
		/// The items of this parallel.
		/// </summary>
		public ITimedProgressable[] ParallelItems => m_ParallelItems;

		/// <summary>
		/// The current time of this parallel.
		/// </summary>
		/// 
		/// <remarks> 
		/// It is calculated inside of the coroutine while the parallel is playing
		/// </remarks>
		/// 
		/// <value>The time.</value>
		public float CurrentTime { get { return m_CurrentTime; } }

		/// <summary>
		/// Gets the duration of this parallel.
		/// </summary>
		/// 
		/// <remarks>
		/// It is set at <see cref="Play(float)"/> or alternatively at 
		/// <see cref="SetDuration(float)"/>. By default the <see cref="Duration"/> 
		/// will be the same as <see cref="ParallelDuration"/>, but it can be set to
		/// another value, which will scale the individual duration of the parallel
		/// items to fit the assigned <see cref="Duration"/>.
		/// </remarks>
		/// 
		/// <value>The duration.</value>
		public float Duration { get { return m_Duration; } }

		/// <summary>
		/// The sum of the parallel items duration.
		/// </summary>
		public float ParallelDuration { get { return m_ParallelDuration; } }

		/// <summary>
		/// Gets and sets the progress from 0 to 1 on this parallel.
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
		/// Is the parallel playing?
		/// </summary>
		/// <value><c>true</c> if is playing; otherwise, <c>false</c>.</value>
		public bool IsPlaying { get { return m_IsPlaying; } }

		/// <summary>
		/// Is the parallel paused?
		/// </summary>
		/// <value><c>true</c> if is paused; otherwise, <c>false</c>.</value>
		public bool IsPaused { get { return m_IsPaused; } }

		#endregion


		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Parallel"/> class.
		/// </summary>
		/// <param name="monoBehaviour">
		/// A <see cref="MonoBehaviour"/> that will start the coroutines.
		/// </param>
		public Parallel(MonoBehaviour monoBehaviour, params ITimedProgressable[] parallelItems) {
			m_MonoBehaviour = monoBehaviour;
			SetAnimatableElement(parallelItems);
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Plays the parallel which will last the current <see cref="Duration"/>.
		/// </summary>
		/// 
		/// <remarks>
		/// <see cref="Duration"/> is equal to <see cref="ParallelDuration"/> by default, but
		/// can be changed through <see cref="SetDuration(float)"/> which will scale the individual 
		/// duration of the parallel items to fit the assigned <see cref="Duration"/>
		/// </remarks>
		/// 
		/// <returns>The parallel object.</returns>
		public Parallel Play() {
			return Play(Duration);
		}

		/// <summary>
		/// Plays the parallel that will last the provided <c>duration</c> which in turn becomes the
		/// value for <see cref="Duration"/>.
		/// </summary>
		/// 
		/// <returns>The parallel object.</returns>
		/// <param name="duration">Duration.</param>
		public Parallel Play(float duration) {
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
		/// Pauses the parallel.
		/// </summary>
		/// <returns>The parallel object.</returns>
		public Parallel Pause() {
			m_IsPaused = true;
			return this;
		}

		/// <summary>
		/// Resumes this parallel.
		/// </summary>
		/// <returns>The parallel object.</returns>
		public Parallel Resume() {
			// Progress may have changed from outside so we update m_CurrentTime here.
			m_CurrentTime = m_Progress * m_Duration;
			m_IsPaused = false;
			return this;
		}

		/// <summary>
		/// Stops the parallel.
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
		/// Resets the parallel to its default state.
		/// </summary>
		public void Dispose() {

			m_IsDisposed = true;

			StopCoroutine();

			// Set to null the callbacks
			Clean(CleanFlag.All);

			// parallel objects
			m_ProgressingItemInfo = null;
			ResetItems();

			// Reset all to avoid references that would prevent garbage collection
			foreach (ParallelItemInfo parallelItemInfo in m_ParallelItemsInfo) {
				parallelItemInfo.Item.Dispose();
			}

		}

		/// <summary>
		/// Sets the <see cref="Duration"/> of the parallel.
		/// </summary>
		/// 
		/// <remarks>
		/// It is can be set at <see cref="Play(float)"/> or through this method.
		/// By default the <see cref="Duration"/> will be the same as <see cref="ParallelDuration"/>,
		/// but it can be set to another value, which will scale the individual duration
		/// of the parallel items to fit the assigned <see cref="Duration"/>.
		/// </remarks>
		/// 
		/// <returns>The parallel object.</returns>
		/// <param name="duration">The duration.</param>
		public Parallel SetDuration(float duration) {
			m_Duration = duration;
			return this;
		}

		/// <summary>
		/// Sets the <see cref="TimeMode"/> of the parallel.
		/// </summary>
		/// <param name="timeMode"></param>
		/// <returns></returns>
		public Parallel SetTimeMode(TimeMode timeMode) {
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
		public Parallel SetEasing(Easing easing) {
			m_Easing = easing;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the parallel starts on <c>Play</c>.
		/// </summary>
		/// <returns>The parallel object.</returns>
		/// <param name="onStart">The action to be invoked on start.</param>
		public Parallel SetOnStart(Action onStart) {
			m_OnStartParallel = null;
			m_OnStart = onStart;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the parallel starts on <c>Play</c>. 
		/// The callback receives the parallel object as a parameter.
		/// </summary>
		/// <returns>The parallel object.</returns>
		/// <param name="onStart">The action to be invoked on start.</param>
		public Parallel SetOnStart(Action<Parallel> onStart) {
			m_OnStart = null;
			m_OnStartParallel = onStart;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called every frame while the parallel is playing 
		/// and not paused.
		/// </summary>
		/// <returns>The parallel object.</returns>
		/// <param name="onUpdate">The action to be invoked on update.</param>
		public Parallel SetOnUpdate(Action onUpdate) {
			m_OnUpdateParallel = null;
			m_OnUpdate = onUpdate;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called every frame while the parallel is playing 
		/// and not paused. The callback receives the parallel object as parameter.
		/// </summary>
		/// <returns>The parallel object.</returns>
		/// <param name="onUpdate">The action to be invoked on update.</param>
		public Parallel SetOnUpdate(Action<Parallel> onUpdate) {
			m_OnUpdate = null;
			m_OnUpdateParallel = onUpdate;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the parallel is interrupted by being played
		/// or stopped while <see cref="IsPlaying"/><c> == true</c>.
		/// </summary>
		/// <param name="onInterrupt">The action to be invoked on interrupt.</param>
		/// <returns>The motion object.</returns>
		public Parallel SetOnInterrupt(Action onInterrupt) {
			m_OnInterruptParallel = null;
			m_OnInterrupt = onInterrupt;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the parallel is interrupted by being played
		/// or stopped while <see cref="IsPlaying"/><c> == true</c>. The callback receives the 
		/// parallel object as a parameter.
		/// </summary>
		/// <param name="onInterrupt">The action to be invoked on interrupt.</param>
		/// <returns>The motion object.</returns>
		public Parallel SetOnInterrupt(Action<Parallel> onInterrupt) {
			m_OnInterrupt = null;
			m_OnInterruptParallel = onInterrupt;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the parallel completes.
		/// </summary>
		/// <returns>The parallel object.</returns>
		/// <param name="onComplete">The action to be invoked on complete.</param>
		public Parallel SetOnComplete(Action onComplete) {
			m_OnCompleteParallel = null;
			m_OnComplete = onComplete;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the parallel completes. The callback receives
		/// the parallel object as a parameter.
		/// </summary>
		/// <returns>The parallel object.</returns>
		/// <param name="onComplete">The action to be invoked on complete.</param>
		public Parallel SetOnComplete(Action<Parallel> onComplete) {
			m_OnComplete = null;
			m_OnCompleteParallel = onComplete;
			return this;
		}

		/// <summary>
		/// Updates <see cref="ParallelDuration"/> and assigns its value to <see cref="Duration"/>
		/// so that the default duration of the parallel results from its items. Additionally
		/// calculates the position in time of its items. 
		/// </summary>
		///
		/// <remarks>
		/// It should be called if the duration of any of its parallel items changes.
		/// </remarks>
		public void EvaluateParallel() {
			m_ParallelItemsInfo = new ParallelItemInfo[m_ParallelItems.Length];
			m_ParallelDuration = 0;
			for (int i = 0; i < m_ParallelItems.Length; i++) {
				m_ParallelItemsInfo[i] = new ParallelItemInfo(i, m_ParallelItems[i]);
				m_ParallelItemsInfo[i].Position = m_ParallelDuration;
				m_ParallelDuration += m_ParallelItemsInfo[i].Item.Duration;
			}
			m_Duration = m_ParallelDuration;
		}

		/// <summary>
		/// Invokes the <c>OnStart</c> callback.
		/// </summary>
		public void InvokeOnStart() {

			// OnStart of the first item
			if (m_ParallelItemsInfo.Length > 0) {
				if (!m_ParallelItemsInfo[0].Started) {
					m_ParallelItemsInfo[0].Item.InvokeOnStart();
					m_ParallelItemsInfo[0].Started = true;
				}
			}

			// OnStart on the parallel itself
			m_OnStart?.Invoke();
			m_OnStartParallel?.Invoke(this);

		}

		/// <summary>
		/// Invokes the <c>OnUpdate</c> callback.
		/// </summary>
		public void InvokeOnUpdate() {
			// Start/Complete the parallel items if time is past their start/end and they haven't 
			// been started/completed
			foreach (ParallelItemInfo itemInfo in m_ParallelItemsInfo) {

				float timeScale = m_Duration / m_ParallelDuration;
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
			// Update the parallel itself
			m_OnUpdate?.Invoke();
			m_OnUpdateParallel?.Invoke(this);
		}

		/// <summary>
		/// Invokes the <c>OnInterrupt</c> callback.
		/// </summary>
		public void InvokeOnInterrupt() {
			// Interrupt the progressing item as long as it haven't been completed.
			if (m_ProgressingItemInfo != null && !m_ProgressingItemInfo.Completed) {
				m_ProgressingItemInfo.Item.InvokeOnInterrupt();
			}
			// Interrupt the parallel itself
			m_OnInterrupt?.Invoke();
			m_OnInterruptParallel?.Invoke(this);
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
			// Complete the parallel itself
			m_OnComplete?.Invoke();
			m_OnCompleteParallel?.Invoke(this);
		}

		/// <summary>
		/// Sets <c>Easing</c>, <c>OnStart</c>, <c>OnUpdate</c>, <c>OnInterrupt</c> and/or <c>OnComplete</c> to null.
		/// </summary>
		/// <param name="cleanFlags">The clean flags.</param>
		public Parallel Clean(CleanFlag cleanFlags) {
			if ((cleanFlags & CleanFlag.Easing) == CleanFlag.Easing) { SetEasing(null); }
			if ((cleanFlags & CleanFlag.OnStart) == CleanFlag.OnStart) { SetOnStart((Action)null); }
			if ((cleanFlags & CleanFlag.OnUpdate) == CleanFlag.OnUpdate) { SetOnUpdate((Action)null); }
			if ((cleanFlags & CleanFlag.OnInterrupt) == CleanFlag.OnInterrupt) { SetOnInterrupt((Action)null); }
			if ((cleanFlags & CleanFlag.OnComplete) == CleanFlag.OnComplete) { SetOnComplete((Action)null); }
			if ((cleanFlags & CleanFlag.All) == CleanFlag.All) {
				Clean(CleanFlag.Easing | CleanFlag.OnStart | CleanFlag.OnUpdate | CleanFlag.OnInterrupt | CleanFlag.OnComplete);
			}
			return this;
		}

		/// <summary>
		/// Changes parallel items.
		/// </summary>
		/// <param name="animatableElement"></param>
		public void SetAnimatableElement(object animatableElement) {

			if (!(animatableElement is ITimedProgressable[])) {
				throw new ArgumentException(
					$"The animatableElement: {animatableElement.GetType()} is not a {m_ParallelItems.GetType()}"
				);
			}

			ITimedProgressable[] parallelItems = (ITimedProgressable[])animatableElement;
			for (int i = 0; i < parallelItems.Length; i++) {
				if (Mathf.Approximately(parallelItems[i].Duration, 0)) {
					throw new ArgumentException("Parallel items can not have duration equal to 0");
				}
			}

			m_ParallelItems = parallelItems;
			EvaluateParallel();

		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private MonoBehaviour m_MonoBehaviour;

		[NonSerialized]
		private ITimedProgressable[] m_ParallelItems;

		[NonSerialized]
		private ParallelItemInfo[] m_ParallelItemsInfo;

		[NonSerialized]
		private float m_ParallelDuration;

		/// <summary>
		/// The currently progressing parallel item as calculated by
		/// <see cref="SetProgressingItemInfo()"/>.
		/// </summary>
		[NonSerialized]
		private ParallelItemInfo m_ProgressingItemInfo;

		[NonSerialized]
		private Coroutine m_Coroutine;

		[NonSerialized]
		private TimeMode m_TimeMode;

		[NonSerialized]
		private Easing m_Easing;

		[NonSerialized]
		private Action m_OnStart;

		[NonSerialized]
		private Action<Parallel> m_OnStartParallel;

		[NonSerialized]
		private Action m_OnUpdate;

		[NonSerialized]
		private Action<Parallel> m_OnUpdateParallel;

		[NonSerialized]
		private Action m_OnInterrupt;

		[NonSerialized]
		private Action<Parallel> m_OnInterruptParallel;

		[NonSerialized]
		private Action m_OnComplete;

		[NonSerialized]
		private Action<Parallel> m_OnCompleteParallel;

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

			// Wait one frame for the properties to be ready, in case the parallel is
			// created and started in the same line.
			yield return null;

			InvokeOnStart();

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
			foreach (ParallelItemInfo itemInfo in m_ParallelItemsInfo) {
				itemInfo.Completed = false;
				itemInfo.Started = false;
				if (itemInfo.Item is Parallel) { // TODO: Review if this need to interact with Sequences too
					((Parallel)itemInfo.Item).ResetItems();
				}
			}
		}

		/// <summary>
		/// Sets the item of the parallel that coincides with <see cref="EasedProgress"/>
		/// </summary>
		private void SetProgressingItemInfo() {
			if (EasedProgress < 1) {
				// Progress translated to time units
				float timeOnParallel = EasedProgress * m_ParallelDuration;
				// Iterate through items
				for (int i = 0; i < m_ParallelItemsInfo.Length; i++) {
					if (Mathf.Approximately(m_ParallelItemsInfo[i].Item.Duration, 0)) {
						throw new ArgumentException("Parallel items can not have duration equal to 0");
					}
					if (timeOnParallel >= m_ParallelItemsInfo[i].Position &&
						timeOnParallel < m_ParallelItemsInfo[i].Position + m_ParallelItemsInfo[i].Item.Duration) {
						m_ProgressingItemInfo = m_ParallelItemsInfo[i];
						break;
					}
				}
			} else {
				// Handle progress == 1 (0 is already handled)
				m_ProgressingItemInfo = m_ParallelItemsInfo[m_ParallelItemsInfo.Length - 1];
			}
		}

		private void ApplyProgress() {

			CheckDisposed();

			float timeOnParallel = EasedProgress * m_ParallelDuration;

			// Wait until it has started, otherwise it may progress before
			// starting, which would lead to unexpected behaviour.
			if (m_ProgressingItemInfo.Started) {
				m_ProgressingItemInfo.Item.Progress =
					(timeOnParallel - m_ParallelItemsInfo[m_ProgressingItemInfo.Index].Position) /
					m_ProgressingItemInfo.Item.Duration;
			}

			// If the parallel is paused and the Progress is set to a point in time before, this updates
			// the following parallel items so that they are not marked as completed.
			for (int i = m_ProgressingItemInfo.Index; i < m_ParallelItemsInfo.Length; i++) {
				m_ParallelItemsInfo[i].Completed = false;
			}

		}

		private void CheckDisposed() {
			if (m_IsDisposed) {
				throw new InvalidOperationException(
					"This parallel has been disposed, it can not be used anymore."
				);
			}
		}

		#endregion


	}

}