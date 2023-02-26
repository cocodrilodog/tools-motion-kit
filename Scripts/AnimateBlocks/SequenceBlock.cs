namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// Block for <see cref="Sequence"/> objects.
	/// </summary>
	[Serializable]
	public class SequenceBlock : CompoundBlock, IAnimateParent {


		#region #region Public Properties

		public Sequence Sequence {
			get {
				if (m_Sequence == null) {
					Initialize();
				}
				return m_Sequence;
			}
		}

		public override List<AnimateBlock> Items {
			get {
				_ = Sequence; // <- Force init
				return m_SequenceItems;
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

				List<ITimedProgressable> sequenceItemsList = new List<ITimedProgressable>();
				foreach (var sequenceItems in m_SequenceItems) {
					sequenceItemsList.Add(sequenceItems.TimedProgressable);
				}

				try {
					m_Sequence = Animate.GetSequence(Owner, ReuseID, sequenceItemsList.ToArray())
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

		}

		public override void Play() => Sequence.Play();

		public override void Stop() => Sequence.Stop();

		public override void Pause() => Sequence.Pause();

		public override void Resume() => Sequence.Resume();

		public override void Dispose() {
			base.Dispose();
			foreach (var sequenceItem in Items) {
				sequenceItem?.Dispose();
			}
		}

		public AnimateBlock GetChildBlock(string name) => Items.FirstOrDefault(b => b != null && b.Name == name);

		public AnimateBlock GetChildBlockAtPath(string blockPath) => AnimateBlocksUtility.GetChildBlockAtPath(this, blockPath);

		public AnimateBlock[] GetChildrenBlocks() => Items.ToArray();

		public override string DefaultName => "Sequence";

		#endregion


		#region Protected Properties

		protected override List<AnimateBlockOperation> BatchOperations => m_BatchOperations;

		#endregion


		#region Private Fields - Serialized

		/// <summary>
		/// The property was renamed to <see cref="Items"/>, but this field was left as is because
		/// at the time of the change, there was a lot of work already done that I didn't wnat to lose. 
		/// Additionally it reads more clear for the user in the inspector.
		/// </summary>
		[SerializeReference]
		private List<AnimateBlock> m_SequenceItems = new List<AnimateBlock>();

		[SerializeReference]
		private List<AnimateBlockOperation> m_BatchOperations = new List<AnimateBlockOperation>();

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private Sequence m_Sequence;

		#endregion


	}

}