namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using UnityEngine;

	public class Sequence : IPlayback, ITimedProgressable {


		#region Small Types

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
		/// <value>The progress.</value>
		public float Progress {
			get { return m_Progress; }
			set {
				m_Progress = value;
				UpdateSequenceItemAtProgress(m_Progress);
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
			m_SequenceItems = sequenceItems;
			UpdateSequence();
		}

		#endregion


		#region Public Methods

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
			StopCoroutine();
			m_Duration = duration;
			m_IsPaused = false;
			m_Coroutine = m_MonoBehaviour.StartCoroutine(_Play());
			m_IsPlaying = true;
			return this;
		}

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
			StopCoroutine();
			if (m_Duration > 0) {
				m_IsPaused = false;
				m_Coroutine = m_MonoBehaviour.StartCoroutine(_Play());
				m_IsPlaying = true;
			} else {
				// TODO: Check if this needs to be done in MotionBase and Timer.
				// TODO: Should the OnUpdate and OnComplete callbacks of the sequence items be called here?
				Progress = 1;
				OnUpdate();
				OnComplete();
			}
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
			StopCoroutine();
			m_IsPlaying = false;
			m_ProgressedSequenceItem = null;
			m_LastProgressedSequenceItem = null;
		}

		/// <summary>
		/// Resets the sequence to its default state.
		/// </summary>
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

			// Storage for progressed sequence item
			m_ProgressedSequenceItem = null;
			m_LastProgressedSequenceItem = null;

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
		/// Sets a callback that will be called every frame while the sequence is playing 
		/// and not paused.
		/// </summary>
		/// <returns>The sequence object.</returns>
		/// <param name="onUpdate">The action to be invoked on update.</param>
		public Sequence SetOnUpdate(Action onUpdate) {
			m_OnUpdateProgress = null;
			m_OnUpdate = onUpdate;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called every frame while the sequence is playing 
		/// and not paused. <paramref name="onUpdate"/> receives the <c>progress</c> as
		/// parameter.
		/// </summary>
		/// <returns>The sequence object.</returns>
		/// <param name="onUpdate">The action to be invoked on update.</param>
		public Sequence SetOnUpdate(Action<float> onUpdate) {
			m_OnUpdate = null;
			m_OnUpdateProgress = onUpdate;
			return this;
		}

		/// <summary>
		/// Sets a callback that will be called when the sequence completes.
		/// </summary>
		/// <returns>The sequence object.</returns>
		/// <param name="onComplete">The action to be invoked on complete.</param>
		public Sequence SetOnComplete(Action onComplete) {
			m_OnComplete = onComplete;
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
		public void UpdateSequence() {
			m_SequenceItemPositions = new float[m_SequenceItems.Length];
			for (int i = 0; i < m_SequenceItems.Length; i++) {
				m_SequenceItemPositions[i] = m_SequenceDuration;
				m_SequenceDuration += m_SequenceItems[i].Duration;
			}
			m_Duration = m_SequenceDuration;
		}

		/// <summary>
		/// Invokes the <c>OnUpdate</c> callback.
		/// </summary>
		public void OnUpdate() {
			m_ProgressedSequenceItem?.OnUpdate();
			m_OnUpdate?.Invoke();
			m_OnUpdateProgress?.Invoke(Progress);
		}

		/// <summary>
		/// Invokes the <c>OnComplete</c> callback.
		/// </summary>
		public void OnComplete() {
			// This calls OnComplete on the last item.
			if (m_LastProgressedSequenceItem != null) {
				m_ProgressedSequenceItem.Progress = 1;
				m_ProgressedSequenceItem.OnComplete();
			}
			m_OnComplete?.Invoke();
		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private MonoBehaviour m_MonoBehaviour;

		[NonSerialized]
		private ITimedProgressable[] m_SequenceItems;

		[NonSerialized]
		private float m_SequenceDuration;

		/// <summary>
		/// The position in time of each sequence item.
		/// </summary>
		[NonSerialized]
		private float[] m_SequenceItemPositions;

		/// <summary>
		/// The currently progressed sequence item as calculated by <see cref="Progress"/>.
		/// </summary>
		[NonSerialized]
		private ITimedProgressable m_ProgressedSequenceItem;

		/// <summary>
		/// The index of the <see cref="m_ProgressedSequenceItem"/>.
		/// </summary>
		[NonSerialized]
		private int m_ProgressedSequenceItemIndex;

		/// <summary>
		/// Storage for <see cref="m_ProgressedSequenceItem"/> to check for change on
		/// <see cref="_Play"/>.
		/// </summary>
		///
		/// <remarks>
		/// When <see cref="m_ProgressedSequenceItem"/> changes, is time to invoke its
		/// <c>OnComplete</c> callback.
		/// </remarks>
		[NonSerialized]
		private ITimedProgressable m_LastProgressedSequenceItem;

		[NonSerialized]
		private Coroutine m_Coroutine;

		[NonSerialized]
		private TimeMode m_TimeMode;

		[NonSerialized]
		private Easing m_Easing;

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

		/// <summary>
		/// An internal version of <see cref="Progress"/>
		/// </summary>
		///
		/// <remarks>
		///	This is used in <see cref="_Play"/> which calls <see cref="UpdateSequenceItemAtProgress(float)"/>
		///	prior to applying the progress.
		/// </remarks>
		private float _Progress {
			get { return m_Progress; }
			set {
				m_Progress = value;
				ApplyProgress();
			}
		}

		#endregion


		#region Private Methods

		private IEnumerator _Play() {

			m_CurrentTime = 0;
			m_Progress = 0;

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

					// Check for OnComplete of all the items except the last one
					
					// Get the sequence item to be progressed (targetProgress is always < 1 here)
					float targetProgress = m_CurrentTime / m_Duration;
					UpdateSequenceItemAtProgress(targetProgress);

					// TODO: If duration is very short, should OnUpdate and OnComplete be certified
					// to be called in all sequence items?
					
					// Will the progressed sequence item change?
					// If it will, complete the previous one before. 
					if (m_ProgressedSequenceItem != m_LastProgressedSequenceItem) {
						if(m_LastProgressedSequenceItem != null) {
							m_LastProgressedSequenceItem.Progress = 1;
							m_LastProgressedSequenceItem.OnComplete();
						}
						m_LastProgressedSequenceItem = m_ProgressedSequenceItem;
					}

					// Finally, apply the progress
					_Progress = targetProgress;

					OnUpdate();

				}
				yield return null;
			}

			OnUpdate();

			// Set the coroutine to null before calling m_OnComplete() because m_OnComplete()
			// may start another animation with the same motion object and we don't 
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

		/// <summary>
		/// Gets the item of the sequence that coincides with the provided <paramref name="progress"/>.
		/// </summary>
		/// <param name="progress">The progress.</param>
		private void UpdateSequenceItemAtProgress(float progress) {
			if (m_Progress < 1) {
				// Progress translated to time units
				float timeOnSequence = progress * m_SequenceDuration;
				// Iterate through items
				for (int i = 0; i < m_SequenceItems.Length; i++) {
					if (timeOnSequence >= m_SequenceItemPositions[i] &&
						timeOnSequence < m_SequenceItemPositions[i] + m_SequenceItems[i].Duration) {
						m_ProgressedSequenceItemIndex = i;
						m_ProgressedSequenceItem = m_SequenceItems[i];
						break;
					}
				}
			} else {
				// Handle progress == 1 (0 is already handled)
				m_ProgressedSequenceItemIndex = m_SequenceItems.Length - 1;
				m_ProgressedSequenceItem = m_SequenceItems[m_ProgressedSequenceItemIndex];
			}
		}

		private void ApplyProgress() {
			float timeOnSequence = m_Progress * m_SequenceDuration;
			// TODO: Use easing
			m_ProgressedSequenceItem.Progress =
				(timeOnSequence - m_SequenceItemPositions[m_ProgressedSequenceItemIndex]) /
				m_ProgressedSequenceItem.Duration;
		}

		#endregion


	}

}