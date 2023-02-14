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
	public class SequenceBlock : AnimateBlock, IAnimateParent {


		#region #region Public Properties

		public Sequence Sequence {
			get {
				if (m_Sequence == null) {
					Initialize();
				}
				return m_Sequence;
			}
		}

		public List<AnimateBlock> SequenceItems => m_SequenceItems;

		public override ITimedProgressable TimedProgressable => Sequence;

		public override float Progress {
			get => Sequence.Progress;
			set => Sequence.Progress = value;
		}

		public override float CurrentTime => Sequence.CurrentTime;

		public override float Duration => Sequence.Duration;

		public override bool IsPlaying => Sequence.IsPlaying;

		public override bool IsPaused => Sequence.IsPaused;

		#endregion


		#region Public Methods

		public override void Initialize() {

			List<ITimedProgressable> sequenceItemsList = new List<ITimedProgressable>();
			foreach (var sequenceItems in SequenceItems) {
				sequenceItemsList.Add(sequenceItems.TimedProgressable);
			}

			m_Sequence = Animate.GetSequence(this, ReuseID, sequenceItemsList.ToArray())
				.SetEasing(Easing.FloatEasing)
				.SetTimeMode(TimeMode);

			if (DurationInput > 0) {
				m_Sequence.SetDuration(DurationInput);
			}

			// This approach will only work if the listeners are added via editor
			if (OnStart.GetPersistentEventCount() > 0) m_Sequence.SetOnStart(OnStart.Invoke);
			if (OnUpdate.GetPersistentEventCount() > 0) m_Sequence.SetOnUpdate(OnUpdate.Invoke);
			if (OnInterrupt.GetPersistentEventCount() > 0) m_Sequence.SetOnInterrupt(OnInterrupt.Invoke);
			if (OnComplete.GetPersistentEventCount() > 0) m_Sequence.SetOnComplete(OnComplete.Invoke);

		}

		public override void Play() => Sequence.Play();

		public override void Stop() => Sequence.Stop();

		public override void Pause() => Sequence.Pause();

		public override void Resume() => Sequence.Resume();

		public override void Dispose() {
			base.Dispose();
			foreach (var sequenceItem in SequenceItems) {
				sequenceItem?.Dispose();
			}
		}

		public AnimateBlock GetChildBlock(string name) => SequenceItems.FirstOrDefault(b => b != null && b.Name == name);

		public AnimateBlock[] GetChildrenBlocks() => SequenceItems.ToArray();

		public override string DefaultName => "Sequence";

		#endregion


		#region Private Fields - Serialized

		[SerializeReference]
		private List<AnimateBlock> m_SequenceItems = new List<AnimateBlock>();

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private Sequence m_Sequence;

		#endregion


	}

}