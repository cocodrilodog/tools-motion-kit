namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Asset for <see cref="Sequence"/> objects.
	/// </summary>
	public class SequenceAsset : AnimateAsset, IMonoScriptableOwner {


		#region #region Public Properties

		public Sequence Sequence {
			get {
				if (m_Sequence == null) {
					Initialize();
				}
				return m_Sequence;
			}
		}

		public List<AnimateAssetField> SequenceItemsFields => m_SequenceItems;

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

		public void OnMonoScriptableOwnerCreated() {
			MonoScriptableUtility.RecreateMonoScriptableObjects(SequenceItemsFields.ToArray(), this);
		}

		public void OnMonoScriptableOwnerModified() {
			MonoScriptableUtility.RecreateRepeatedMonoScriptableArrayOrListItems(SequenceItemsFields.ToArray(), this);
		}

		public void OnMonoScriptableOwnerContextMenu(string propertyPath) {
			MonoScriptableUtility.RecreateMonoScriptableObjects(SequenceItemsFields.ToArray(), this);
		}

		#endregion


		#region Private Fields - Serialized

		[SerializeField]
		private List<AnimateAssetField> m_SequenceItems;

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private Sequence m_Sequence;

		#endregion


	}

}