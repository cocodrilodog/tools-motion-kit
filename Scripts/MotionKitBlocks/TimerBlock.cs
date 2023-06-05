namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Block for <see cref="Timer"/> objects.
	/// </summary>
	[Serializable]
	public class TimerBlock : MotionKitBlock {


		#region Public Properties

		public Timer Timer {
			get {
				if (m_Timer == null) {
					Initialize();
				}
				return m_Timer;
			}
		}

		public override ITimedProgressable TimedProgressable => Timer;

		public override float Progress {
			get => Timer.Progress;
			set => Timer.Progress = value;
		}

		public override float CurrentTime => Timer.CurrentTime;

		public override float Duration => Timer.Duration;

		public override bool IsPlaying => Timer.IsPlaying;

		public override bool IsPaused => Timer.IsPaused;

		#endregion


		#region Public Methods

		public override void Initialize() {
			if (Application.isPlaying) {
				ResetPlayback();
			}
		}

		public override void ResetPlayback() {

			Debug.Log($"ResetPlayback: {Name}");

			m_Timer = MotionKit.GetTimer(Owner, ReuseID)
				.SetDuration(DurationInput)
				.SetTimeMode(TimeMode);

			// This approach will only work if the listeners are added via editor
			if (OnStart.GetPersistentEventCount() > 0) m_Timer.SetOnStart(OnStart.Invoke);
			if (OnUpdate.GetPersistentEventCount() > 0) m_Timer.SetOnUpdate(OnUpdate.Invoke);
			if (OnInterrupt.GetPersistentEventCount() > 0) m_Timer.SetOnInterrupt(OnInterrupt.Invoke);
			if (OnComplete.GetPersistentEventCount() > 0) m_Timer.SetOnComplete(OnComplete.Invoke);

		}

		public override void Play() {
			base.Play();
			Timer.Play();
		}

		public override void Stop() => Timer.Stop();

		public override void Pause() => Timer.Pause();

		public override void Resume() => Timer.Resume();

		public override string DefaultName => $"Timer";

		#endregion


		#region Private Fields

		[NonSerialized]
		private Timer m_Timer;

		#endregion


	}

}