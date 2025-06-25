namespace CocodriloDog.MotionKit {

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
	public class ParallelBlock : CompoundBlock, ICompositeParent<MotionKitBlock> {


		#region #region Public Properties

		public override bool IsInitialized => m_Parallel != null;

		public override bool ShouldResetPlayback => base.ShouldResetPlayback || m_Parallel == null;

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
			base.Initialize();
			if (Application.isPlaying) {
				ResetPlayback();
			}
		}

		public override void Play() {
			base.Play();
			Parallel.Play();
		}

		public override void Stop() => Parallel.Stop();

		public override void Pause() => Parallel.Pause();

		public override void Resume() => Parallel.Resume();

		public override void RegisterAsReferenceable(UnityEngine.Object root) {
			base.RegisterAsReferenceable(root);
			foreach (var parallelItem in Items) {
				parallelItem?.RegisterAsReferenceable(root);
			}
			m_BatchOperations.ForEach(bo => bo.RegisterAsReferenceable(root));
		}

		public override void UnregisterReferenceable(UnityEngine.Object root) {
			base.UnregisterReferenceable(root);
			foreach (var parallelItem in Items) {
				parallelItem?.UnregisterReferenceable(root);
			}
			m_BatchOperations.ForEach(bo => bo.UnregisterReferenceable(root));
		}

		public override void Dispose() {
			base.Dispose();
			foreach (var parallelItem in Items) {
				parallelItem?.Dispose();
			}
		}

		public MotionKitBlock GetChild(string name) => Items.FirstOrDefault(b => b != null && b.Name == name);

		public T GetChild<T>(string name) where T : MotionKitBlock => GetChild(name) as T;

		public MotionKitBlock GetChildAtPath(string path) => CompositeObjectUtility.GetChildAtPath(this, path);

		public T GetChildAtPath<T>(string path) where T : MotionKitBlock => GetChildAtPath(path) as T;

		public MotionKitBlock[] GetChildren() => Items.ToArray();

		public override void SetItem(int index, MotionKitBlock block) => m_ParallelItems[index] = block;

		#endregion


		#region Protected Properties

		protected override List<MotionKitBatchOperation> BatchOperations => m_BatchOperations;

		#endregion


		#region Protected Methods

		protected override void ResetPlayback() {

			base.ResetPlayback();

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

			// Callbacks: Adding editor and runtime callbacks
			m_Parallel.SetOnStart(() => {
				TryResetPlayback(false);    // After a recursive reset on play, reset only this object (not recursively) when the playback starts
				UnlockResetPlayback(false); // After a possible recursive lock when setting initial values, this auto unlocks this object (not recursively)
											// when the lock is not needed anymore
				InvokeOnStart();
			});

			// We are setting the callbacks to null when there are none to clean it in case this is a reused playback. In the case of
			// OnStart, it is required always, but it is updated. So if InvokeOnStart does nothing, we are still cleaning it.
			m_Parallel.SetOnUpdate(OnUpdate.GetPersistentEventCount() > 0 || OnUpdate_Runtime != null ? InvokeOnUpdate : null);
			m_Parallel.SetOnInterrupt(OnInterrupt.GetPersistentEventCount() > 0 || OnInterrupt_Runtime != null ? InvokeOnInterrupt : null);
			m_Parallel.SetOnComplete(OnComplete.GetPersistentEventCount() > 0 || OnComplete_Runtime != null ? InvokeOnComplete : null);

		}

		#endregion


		#region Private Fields - Serialized

		/// <summary>
		/// The property was renamed to <see cref="Items"/>, but this field was left as is because
		/// at the time of the change, there was a lot of work already done that I didn't wnat to lose. 
		/// Additionally it reads more clear for the user in the inspector.
		/// </summary>
		[Tooltip("The blocks that are part of this parallel.")]
		[SerializeReference]
		private List<MotionKitBlock> m_ParallelItems = new List<MotionKitBlock>();

		[Tooltip("A set of batch operations that can be performed in each block of the parallel in edit mode.")]
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