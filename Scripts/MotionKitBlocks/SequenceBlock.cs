namespace CocodriloDog.Animation {

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
	public class SequenceBlock : CompoundBlock, IMotionKitParent {


		#region #region Public Properties

		public Sequence Sequence {
			get {
				if (m_Sequence == null) {
					Initialize();
				}
				return m_Sequence;
			}
		}

		public override ReadOnlyCollection<PlaybackBlock> Items {
			get {
				_ = Sequence; // <- Force init
				if(m_Items == null) {
					m_Items = new ReadOnlyCollection<PlaybackBlock>(m_SequenceItems);
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
			if (Application.isPlaying) {
				ResetPlayback();
			}
		}

		public override void ResetPlayback() {

			Debug.Log($"ResetPlayback: {Name}");

			List<ITimedProgressable> sequenceItemsList = new List<ITimedProgressable>();
			foreach (var sequenceItems in m_SequenceItems) {
				sequenceItemsList.Add(sequenceItems.TimedProgressable);
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

			// This approach will only work if the listeners are added via editor
			if (OnStart.GetPersistentEventCount() > 0) m_Sequence.SetOnStart(OnStart.Invoke);
			if (OnUpdate.GetPersistentEventCount() > 0) m_Sequence.SetOnUpdate(OnUpdate.Invoke);
			if (OnInterrupt.GetPersistentEventCount() > 0) m_Sequence.SetOnInterrupt(OnInterrupt.Invoke);
			if (OnComplete.GetPersistentEventCount() > 0) m_Sequence.SetOnComplete(OnComplete.Invoke);

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

		public PlaybackBlock GetChildBlock(string name) => Items.FirstOrDefault(b => b != null && b.Name == name);

		public PlaybackBlock GetChildBlockAtPath(string blockPath) => MotionKitBlocksUtility.GetChildBlockAtPath(this, blockPath);

		public PlaybackBlock[] GetChildrenBlocks() => Items.ToArray();

		public override void SetItem(int index, PlaybackBlock block) => m_SequenceItems[index] = block;

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
		private List<PlaybackBlock> m_SequenceItems = new List<PlaybackBlock>();

		[SerializeReference]
		private List<MotionKitBatchOperation> m_BatchOperations = new List<MotionKitBatchOperation>();

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private ReadOnlyCollection<PlaybackBlock> m_Items;

		[NonSerialized]
		private Sequence m_Sequence;

		#endregion


	}

}