namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class TimerAsset : AnimateAsset {


		#region Public Properties

		public override float Progress => Timer.Progress;

		public override float CurrentTime => Timer.CurrentTime;

		public override float Duration => Timer.Duration;

		public override bool IsPlaying => Timer.IsPlaying;

		public override bool IsPaused => Timer.IsPaused;

		#endregion


		#region Public Methods

		public override void Initialize() {

			m_Timer = Animate.GetTimer(this, ReuseID)
				.SetDuration(DurationInput)
				.SetTimeMode(TimeMode);

			// This approach will only work if the listeners are added via editor
			if (OnStart.GetPersistentEventCount() > 0) m_Timer.SetOnStart(OnStart.Invoke);
			if (OnUpdate.GetPersistentEventCount() > 0) m_Timer.SetOnUpdate(OnUpdate.Invoke);
			if (OnInterrupt.GetPersistentEventCount() > 0) m_Timer.SetOnInterrupt(OnInterrupt.Invoke);
			if (OnComplete.GetPersistentEventCount() > 0) m_Timer.SetOnComplete(OnComplete.Invoke);

		}

		public override void Pause() => Timer.Pause();

		public override void Play() => Timer.Play();

		public override void Resume() => Timer.Resume();

		public override void Stop() => Timer.Stop();

		#endregion


		#region Private Fields

		private Timer m_Timer;

		#endregion


		#region Private Properties

		private Timer Timer {
			get {
				if (m_Timer == null) {
					Initialize();
				}
				return m_Timer;
			}
		}

		#endregion


	}

}