namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	public abstract class AnimateAsset : MonoScriptableObject {


		#region Public Properties

		public abstract ITimedProgressable TimedProgressable { get; }

		public abstract float Progress { get; }

		public abstract float CurrentTime { get; }

		public abstract float Duration { get; }

		public abstract bool IsPlaying { get; }

		public abstract bool IsPaused { get; }

		#endregion


		#region Public Methods

		public abstract void Initialize();

		public abstract void Play();

		public abstract void Stop();

		public abstract void Pause();

		public abstract void Resume();

		public virtual void Dispose() {
			Animate.ClearPlaybacks(this);
		}

		#endregion


		#region Protected Properties

		protected string ReuseID => m_ReuseID = m_ReuseID ?? Guid.NewGuid().ToString();

		protected float DurationInput => m_Duration;

		protected TimeMode TimeMode => m_TimeMode;

		protected AnimateEasingField Easing => m_Easing;

		protected UnityEvent OnStart => m_OnStart;

		protected UnityEvent OnUpdate => m_OnUpdate;

		protected UnityEvent OnInterrupt => m_OnInterrupt;

		protected UnityEvent OnComplete => m_OnComplete;

		#endregion


		#region Private Fields - Serialized

		[SerializeField]
		private float m_Duration;

		[SerializeField]
		private TimeMode m_TimeMode;

		[SerializeField]
		private AnimateEasingField m_Easing;

		[SerializeField]
		private UnityEvent m_OnStart;

		[SerializeField]
		private UnityEvent m_OnUpdate;

		[SerializeField]
		private UnityEvent m_OnInterrupt;

		[SerializeField]
		private UnityEvent m_OnComplete;

#if UNITY_EDITOR
		[SerializeField]
		private int m_CallbackSelection;
#endif

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private string m_ReuseID;

		#endregion


	}

}