namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Component for <see cref="Parallel"/> objects.
	/// </summary>
	[AddComponentMenu("")]
	public class ParallelComponent : AnimateComponent, IMonoCompositeParent {


		#region #region Public Properties

		public Parallel Parallel {
			get {
				if (m_Parallel == null) {
					Initialize();
				}
				return m_Parallel;
			}
		}

		public List<AnimateComponentField> ParallelItemsFields => m_ParallelItems;

		public override ITimedProgressable TimedProgressable => Parallel;

		public override float Progress {
			get => Parallel.Progress;
			set => Parallel.Progress = value;
		}

		public override float CurrentTime => Parallel.CurrentTime;

		public override float Duration => Parallel.Duration;

		public override bool IsPlaying => Parallel.IsPlaying;

		public override bool IsPaused => Parallel.IsPaused;

		#endregion


		#region Public Methods

		public override void Initialize() {

			List<ITimedProgressable> parallelItemsList = new List<ITimedProgressable>();
			foreach(var parallelItemField in ParallelItemsFields) {
				parallelItemsList.Add(parallelItemField.Object.TimedProgressable);
			}

			m_Parallel = Animate.GetParallel(this, ReuseID, parallelItemsList.ToArray())
				.SetEasing(Easing.FloatEasing)
				.SetTimeMode(TimeMode);

			if (DurationInput > 0) {
				m_Parallel.SetDuration(DurationInput);
			}

			// This approach will only work if the listeners are added via editor
			if (OnStart.GetPersistentEventCount() > 0) m_Parallel.SetOnStart(OnStart.Invoke);
			if (OnUpdate.GetPersistentEventCount() > 0) m_Parallel.SetOnUpdate(OnUpdate.Invoke);
			if (OnInterrupt.GetPersistentEventCount() > 0) m_Parallel.SetOnInterrupt(OnInterrupt.Invoke);
			if (OnComplete.GetPersistentEventCount() > 0) m_Parallel.SetOnComplete(OnComplete.Invoke);

		}

		public override void Play() => Parallel.Play();

		public override void Stop() => Parallel.Stop();

		public override void Pause() => Parallel.Pause();

		public override void Resume() => Parallel.Resume();

		public override void Dispose() {
			base.Dispose();
			foreach (var sequenceItemsField in ParallelItemsFields) {
				sequenceItemsField.Object?.Dispose();
			}
		}

		public MonoCompositeFieldBase[] GetChildren() => ParallelItemsFields.ToArray();

		public void ConfirmChildren() {
			foreach (var field in GetChildren()) {
				if (field.ObjectBase != null) {
					field.ObjectBase.SetParent(this);
				}
			}
		}

		#endregion


		#region Private Fields - Serialized

		[SerializeField]
		private List<AnimateComponentField> m_ParallelItems = new List<AnimateComponentField>();

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private Parallel m_Parallel;

		#endregion


	}

}