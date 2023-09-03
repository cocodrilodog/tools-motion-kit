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

		public override bool IsInitialized => m_Timer != null;
		public override bool ShouldResetPlayback => base.ShouldResetPlayback || m_Timer == null;

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

		public override void Play() {
			base.Play();
			Timer.Play();
		}

		public override void Stop() => Timer.Stop();

		public override void Pause() => Timer.Pause();

		public override void Resume() => Timer.Resume();

		public override string DefaultName => $"Timer";

		#endregion


		#region protected Methods

		protected override void ResetPlayback() {

			m_Timer = MotionKit.GetTimer(Owner, ReuseID)
				.SetDuration(DurationInput)
				.SetTimeMode(TimeMode);

			// Callbacks: This approach will only work if the listeners are added via editor
			// We are setting the callbacks to null when there are none to clean it in case this is a reused playback
			m_Timer.SetOnStart(() => {
				TryResetPlayback(false);    // After a recursive reset on play, reset only this object (not recursively) when the playback starts
				UnlockResetPlayback(false); // After a possible recursive lock when setting initial values, this auto unlocks this object (not recursively)
											// when the lock is not needed anymore
				if (OnStart.GetPersistentEventCount() > 0) OnStart.Invoke();
			});
			m_Timer.SetOnUpdate(OnUpdate.GetPersistentEventCount() > 0 ? OnUpdate.Invoke : null);
			m_Timer.SetOnInterrupt(OnInterrupt.GetPersistentEventCount() > 0 ? OnInterrupt.Invoke : null);
			m_Timer.SetOnComplete(OnComplete.GetPersistentEventCount() > 0 ? OnComplete.Invoke : null);

		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private Timer m_Timer;

		#endregion


	}

}