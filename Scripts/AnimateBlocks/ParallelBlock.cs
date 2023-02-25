namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// Block for <see cref="Parallel"/> objects.
	/// </summary>
	[Serializable]
	public class ParallelBlock : CompoundBlock, IAnimateParent {


		#region #region Public Properties

		public Parallel Parallel {
			get {
				if (m_Parallel == null) {
					Initialize();
				}
				return m_Parallel;
			}
		}

		public List<AnimateBlock> ParallelItems {
			get {
				_ = Parallel; // <- Force init
				return m_ParallelItems;
			}
		}

		public override ITimedProgressable TimedProgressable => Parallel;

		public override float Progress {
			get => Parallel.Progress;
			set => Parallel.Progress = value;
		}

		public override float CurrentTime => Parallel.CurrentTime;

		public override float Duration => Parallel.Duration;

		public override bool IsPlaying => Parallel.IsPlaying;

		public override bool IsPaused => Parallel.IsPaused;

		/// <summary>
		/// <see cref="DurationInput"/> if it is above 0, otherwise the longest duration among
		/// the <see cref="ParallelItems"/>.
		/// </summary>
		public override float DurationToBeUsed {
			get {
				var duration = 0f;
				if (DurationInput > 0) {
					duration = DurationInput;
				} else {
					foreach (var item in ParallelItems) {
						duration = Mathf.Max(item != null ? item.DurationToBeUsed : 0, duration);
					}
				}
				return duration;
			}
		}

		#endregion


		#region Public Methods

		public override void Initialize() {

			if (Application.isPlaying) {

				List<ITimedProgressable> parallelItemsList = new List<ITimedProgressable>();
				foreach (var parallelItem in m_ParallelItems) {
					parallelItemsList.Add(parallelItem.TimedProgressable);
				}

				m_Parallel = Animate.GetParallel(Owner, ReuseID, parallelItemsList.ToArray())
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

		}

		public override void Play() => Parallel.Play();

		public override void Stop() => Parallel.Stop();

		public override void Pause() => Parallel.Pause();

		public override void Resume() => Parallel.Resume();

		public override void Dispose() {
			base.Dispose();
			foreach (var parallelItem in ParallelItems) {
				parallelItem?.Dispose();
			}
		}

		public AnimateBlock GetChildBlock(string name) => ParallelItems.FirstOrDefault(b => b != null && b.Name == name);

		public AnimateBlock GetChildBlockAtPath(string blockPath) => AnimateBlocksUtility.GetChildBlockAtPath(this, blockPath);

		public AnimateBlock[] GetChildrenBlocks() => ParallelItems.ToArray();

		public override string DefaultName => "Parallel";

		#endregion


		#region Private Fields - Serialized

		[SerializeReference]
		private List<AnimateBlock> m_ParallelItems = new List<AnimateBlock>();

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private Parallel m_Parallel;

		#endregion


	}

}