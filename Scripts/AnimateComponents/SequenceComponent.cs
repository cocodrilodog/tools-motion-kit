namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Component for <see cref="Sequence"/> objects.
	/// </summary>
	[AddComponentMenu("")]
	public class SequenceComponent : AnimateBaseComponent, IMonoCompositeParent {


		#region #region Public Properties

		public Sequence Sequence {
			get {
				if (m_Sequence == null) {
					Initialize();
				}
				return m_Sequence;
			}
		}

		public List<AnimateComponentField> SequenceItemsFields => m_SequenceItems;

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
			foreach(var sequenceItemsField in SequenceItemsFields) {
				sequenceItemsList.Add(sequenceItemsField.Object.TimedProgressable);
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
			foreach (var sequenceItemsField in SequenceItemsFields) {
				sequenceItemsField.Object?.Dispose();
			}
		}

		public MonoCompositeFieldBase[] GetChildren() => SequenceItemsFields.ToArray();

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
		private List<AnimateComponentField> m_SequenceItems = new List<AnimateComponentField>();

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private Sequence m_Sequence;

		#endregion


	}

}