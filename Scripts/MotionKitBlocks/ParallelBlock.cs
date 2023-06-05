namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// Block for <see cref="Parallel"/> objects.
	/// </summary>
	[Serializable]
	public class ParallelBlock : CompoundBlock, IMotionKitParent {


		#region #region Public Properties

		public Parallel Parallel {
			get {
				if (m_Parallel == null) {
					Initialize();
				}
				return m_Parallel;
			}
		}

		public override ReadOnlyCollection<MotionKitBlock> Items {
			get {
				_ = Parallel; // <- Force init
				if (m_Items == null) {
					m_Items = new ReadOnlyCollection<MotionKitBlock>(m_ParallelItems);
				}
				return m_Items;
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

		public override string DefaultName => "Parallel";

		/// <summary>
		/// <see cref="DurationInput"/> if it is above 0, otherwise the longest duration among
		/// the <see cref="Items"/>.
		/// </summary>
		public override float DurationToBeUsed {
			get {
				var duration = 0f;
				if (DurationInput > 0) {
					duration = DurationInput;
				} else {
					foreach (var item in Items) {
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
				ResetPlayback();
			}
		}

		public override void ResetPlayback() {

			Debug.Log($"ResetPlayback: {Name}");

			List<ITimedProgressable> parallelItemsList = new List<ITimedProgressable>();
			foreach (var parallelItem in m_ParallelItems) {
				parallelItemsList.Add(parallelItem.TimedProgressable);
			}

			m_Parallel = MotionKit.GetParallel(Owner, ReuseID, parallelItemsList.ToArray())
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

		public override void Play() {
			base.Play();
			Parallel.Play();
		}

		public override void Stop() => Parallel.Stop();

		public override void Pause() => Parallel.Pause();

		public override void Resume() => Parallel.Resume();

		public override void Dispose() {
			base.Dispose();
			foreach (var parallelItem in Items) {
				parallelItem?.Dispose();
			}
		}

		public MotionKitBlock GetChildBlock(string name) => Items.FirstOrDefault(b => b != null && b.Name == name);

		public MotionKitBlock GetChildBlockAtPath(string blockPath) => MotionKitBlocksUtility.GetChildBlockAtPath(this, blockPath);

		public MotionKitBlock[] GetChildrenBlocks() => Items.ToArray();

		public override void SetItem(int index, MotionKitBlock block) => m_ParallelItems[index] = block;

		#endregion


		#region Protected Properties

		protected override List<MotionKitBatchOperation> BatchOperations => m_BatchOperations;

		#endregion


		#region Private Fields - Serialized

		/// <summary>
		/// The property was renamed to <see cref="Items"/>, but this field was left as is because
		/// at the time of the change, there was a lot of work already done that I didn't wnat to lose. 
		/// Additionally it reads more clear for the user in the inspector.
		/// </summary>
		[SerializeReference]
		private List<MotionKitBlock> m_ParallelItems = new List<MotionKitBlock>();

		[SerializeReference]
		private List<MotionKitBatchOperation> m_BatchOperations = new List<MotionKitBatchOperation>();

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private ReadOnlyCollection<MotionKitBlock> m_Items;

		[NonSerialized]
		private Parallel m_Parallel;

		#endregion


	}

}