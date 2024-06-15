namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// Block for <see cref="Sequence"/> objects.
	/// </summary>
	[Serializable]
	public class SequenceBlock : CompoundBlock, ICompositeParent<MotionKitBlock> {


		#region #region Public Properties

		public override bool IsInitialized => m_Sequence != null;

		public override bool ShouldResetPlayback => base.ShouldResetPlayback || m_Sequence == null;

		public Sequence Sequence {
			get {
				if (m_Sequence == null) {
					Initialize();
				}
				return m_Sequence;
			}
		}

		public override ReadOnlyCollection<MotionKitBlock> Items {
			get {
				_ = Sequence; // <- Force init
				if (m_Items == null) {
					m_Items = new ReadOnlyCollection<MotionKitBlock>(m_SequenceItems);
				}
				return m_Items;
			}
		}

		public override ITimedProgressable TimedProgressable => Sequence;

		public override float Progress {
			get => Sequence.Progress;
			set => Sequence.Progress = value;
		}

		public override float CurrentTime => Sequence.CurrentTime;

		public override float Duration => Sequence.Duration;

		public override bool IsPlaying => Sequence.IsPlaying;

		public override bool IsPaused => Sequence.IsPaused;

		public override string DefaultName => "Sequence";

		/// <summary>
		/// <see cref="DurationInput"/> if it is above 0, otherwise the sum of the duration of 
		/// the <see cref="Items"/>.
		/// </summary>
		public override float DurationToBeUsed {
			get {
				var duration = 0f;
				if(DurationInput > 0) {
					duration = DurationInput;
				} else {
					foreach(var item in Items) {
						duration += item != null ? item.DurationToBeUsed : 0;
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
			Sequence.Play();
		}

		public override void Stop() => Sequence.Stop();

		public override void Pause() => Sequence.Pause();

		public override void Resume() => Sequence.Resume();

		public override void Dispose() {
			base.Dispose();
			foreach (var sequenceItem in Items) {
				sequenceItem?.Dispose();
			}
		}

		public MotionKitBlock GetChild(string name) => Items.FirstOrDefault(b => b != null && b.Name == name);

		public T GetChild<T>(string name) where T : MotionKitBlock => GetChild(name) as T;

		public MotionKitBlock GetChildAtPath(string path) => CompositeObjectUtility.GetChildAtPath(this, path);

		public T GetChildAtPath<T>(string path) where T : MotionKitBlock => GetChildAtPath(path) as T;

		public MotionKitBlock[] GetChildren() => Items.ToArray();

		public override void SetItem(int index, MotionKitBlock block) => m_SequenceItems[index] = block;

		#endregion


		#region Protected Properties

		protected override List<MotionKitBatchOperation> BatchOperations => m_BatchOperations;

		#endregion


		#region protected Methods

		protected override void ResetPlayback() {

			base.ResetPlayback();

			List<ITimedProgressable> sequenceItemsList = new List<ITimedProgressable>();
			foreach (var sequenceItem in m_SequenceItems) {
				sequenceItemsList.Add(sequenceItem.TimedProgressable);
			}

			try {
				m_Sequence = MotionKit.GetSequence(Owner, ReuseID, sequenceItemsList.ToArray())
					.SetEasing(Easing.FloatEasing)
					.SetTimeMode(TimeMode);
			} catch (ArgumentException e) {
				// This helps to identify which is the one with the error
				throw new ArgumentException($"{Name}: {e}");
			}

			if (DurationInput > 0) {
				m_Sequence.SetDuration(DurationInput);
			}

			// Callbacks: Adding editor and runtime callbacks
			m_Sequence.SetOnStart(() => {
				TryResetPlayback(false);    // After a recursive reset on play, reset only this object (not recursively) when the playback starts
				UnlockResetPlayback(false); // After a possible recursive lock when setting initial values, this auto unlocks this object (not recursively)
											// when the lock is not needed anymore
				InvokeOnStart();
			});

			// We are setting the callbacks to null when there are none to clean it in case this is a reused playback. In the case of
			// OnStart, it is required always, but it is updated. So if InvokeOnStart does nothing, we are still cleaning it.
			m_Sequence.SetOnUpdate(OnUpdate.GetPersistentEventCount() > 0 || OnUpdate_Runtime != null ? InvokeOnUpdate : null);
			m_Sequence.SetOnInterrupt(OnInterrupt.GetPersistentEventCount() > 0 || OnInterrupt_Runtime != null ? InvokeOnInterrupt : null);
			m_Sequence.SetOnComplete(OnComplete.GetPersistentEventCount() > 0 || OnComplete_Runtime != null ? InvokeOnComplete : null);

		}

		#endregion


		#region Private Fields - Serialized

		/// <summary>
		/// The property was renamed to <see cref="Items"/>, but this field was left as is because
		/// at the time of the change, there was a lot of work already done that I didn't want to lose. 
		/// Additionally it reads more clear for the user in the inspector.
		/// </summary>
		[Tooltip("The blocks that are part of this sequence.")]
		[SerializeReference]
		private List<MotionKitBlock> m_SequenceItems = new List<MotionKitBlock>();

		[Tooltip("A set of batch operations that can be performed in each block of the sequence in edit mode.")]
		[SerializeReference]
		private List<MotionKitBatchOperation> m_BatchOperations = new List<MotionKitBatchOperation>();

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private ReadOnlyCollection<MotionKitBlock> m_Items;

		[NonSerialized]
		private Sequence m_Sequence;

		#endregion


	}

}