namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	/// <summary>
	/// Interface to be implemented by <see cref="AnimateBlock"/>. It exposes the members 
	/// that are used for playback and initialization of the compound <see cref="Animate"/> 
	/// objects: <see cref="Sequence"/> and <see cref="Parallel"/>.
	/// </summary>
	/// 
	/// <remarks>
	/// It was created so that a <see cref="MotionBlock{ValueT, MotionT}"/> is easily
	/// identifiable with <see cref="IMotionBlock"/> instead of using the concrete types
	/// derived from the template.
	/// </remarks>
	public interface IAnimateBlock {
		ITimedProgressable TimedProgressable { get; }
		float Progress { get; set; }
		//void SetProgress(float progress, bool invokeCallbacks);
		float CurrentTime { get; }
		float Duration { get; }
		bool IsPlaying { get; }
		bool IsPaused { get; }
		void Initialize();
		void Play();
		void Stop();
		void Pause();
		void Resume();
		void Dispose();
	}

	/// <summary>
	/// Base class for all Animate blocks. It provides a generic interface that applies
	/// for all Animate objects.
	/// </summary>
	/// 
	/// <remarks>
	/// Animate blocks are <see cref="CompositeObject"/>s that contain information about 
	/// Animate motion, timer, sequence or parallel and can create those objects at runtime. 
	/// They are intended to be used as the inspector-friendly part of the Animate engine.
	/// </remarks>
	[Serializable]
	public abstract class AnimateBlock : CompositeObject, IAnimateBlock {


		#region Public Properties

		/// <summary>
		/// Gets the Animate object as a <see cref="ITimedProgressable"/> so that it can be used in <see cref="Sequence"/>s.
		/// </summary>
		public abstract ITimedProgressable TimedProgressable { get; }

		/// <summary>
		/// Gets the <c>Progress</c> of the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		public abstract float Progress { get; set; }

		/// <summary>
		/// Gets the <c>CurrentTime</c> of the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		public abstract float CurrentTime { get; }

		/// <summary>
		/// Gets the <c>Duration</c> of the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		public abstract float Duration { get; }

		/// <summary>
		/// The duration of the Animate object as specified in the inspector.
		/// </summary>
		/// <remarks>
		/// This was named differently in order to differentiate it from <see cref="Duration"/>.
		/// </remarks>
		public float DurationInput => m_Duration;

		/// <summary>
		/// The duration that will be used for the animate object.
		/// </summary>
		/// <remarks>
		/// By default, it will be <see cref="DurationInput"/>, but <see cref="SequenceBlock"/> and
		/// <see cref="ParallelBlock"/> implement a different logic.
		/// </remarks>
		public virtual float DurationToBeUsed => DurationInput;

		/// <summary>
		/// Gets the <c>IsPlaying</c> property of the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		public abstract bool IsPlaying { get; }

		/// <summary>
		/// Gets the <c>IsPaused</c> property of the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		public abstract bool IsPaused { get; }

		public override string NamePostfix => $" ({DurationToBeUsed}s)";

		#endregion


		#region Public Methods

		/// <summary>
		/// The contained animate object such as <see cref="MotionBase{ValueT, MotionT}"/>, 
		/// <see cref="Timer"/> or <see cref="Sequence"/> should be created here.
		/// </summary>
		public abstract void Initialize();

		/// <summary>
		/// Plays the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		public abstract void Play();

		/// <summary>
		/// Plays the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		public abstract void Stop();

		/// <summary>
		/// Pauses the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		public abstract void Pause();

		/// <summary>
		/// Resumes the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		public abstract void Resume();

		/// <summary>
		/// Disposes any Animate object that was created with this asset as owner.
		/// </summary>
		public virtual void Dispose() {
			Animate.ClearPlayback(Animate.Instance, ReuseID);
		}

		#endregion


		#region Protected Properties

		/// <summary>
		/// The owner to be used by the animate object.
		/// </summary>
		/// 
		/// <remarks>
		/// If no <see cref="Owner"/>  is provided in the inspector, it will default to the <see cref="Animate"/> 
		/// singleton.
		/// 
		/// Specifying the same <see cref="Owner"/> in more than one animation in the inspector may be helpful 
		/// to make animations that set values on the same properties not to conflict with each other. The 
		/// animations should share the same <see cref="ReuseID"/> as well.
		/// 
		/// For example, two motions that set the position property of an object may have the same 
		/// <see cref="Owner"/> and <see cref="ReuseID"/> so that when one is interrupted by the other, 
		/// the new one stops the previous one. This avoids the two animations trying to animate the same 
		/// property with different values at the same time.
		/// </remarks>
		protected UnityEngine.Object Owner {
			get {
				if (m_Owner != null) {
					return m_Owner;
				} else {
					return Animate.Instance;
				}
			}
		}

		/// <summary>
		/// The unique ID to be used by the Animate object.
		/// </summary>
		/// 
		/// <remarks>
		/// If no ID is provided in the inspector, an automatic unique ID will be created.
		/// 
		/// Specifying the same <see cref="ReuseID"/> in more than one animation in the inspector may be helpful 
		/// to make animations that set values on the same properties not to conflict with each other. The 
		/// animations should share the same <see cref="Owner"/> as well.
		/// 
		/// For example, two motions that set the position property of an object may have the same 
		/// <see cref="Owner"/> and <see cref="ReuseID"/> so that when one is interrupted by the other, 
		/// the new one stops the previous one. This avoids the two animations trying to animate the same 
		/// property with different values at the same time.
		/// </remarks>
		protected string ReuseID {
			get {
				if (string.IsNullOrWhiteSpace(m_ReuseID)) {
					return m_ReuseID_Auto = m_ReuseID_Auto ?? Guid.NewGuid().ToString();
				} else {
					return m_ReuseID;
				}
			}
		}

		/// <summary>
		/// The time mode to be used by the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		protected TimeMode TimeMode => m_TimeMode;

		/// <summary>
		/// The easing to be used by the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		protected AnimateEasingField Easing => m_Easing;

		/// <summary>
		/// The <c>OnStart</c> callbacks to be invoked by the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		protected UnityEvent OnStart => m_OnStart;

		/// <summary>
		/// The <c>OnUpdate</c> callbacks to be invoked by the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		protected UnityEvent OnUpdate => m_OnUpdate;

		/// <summary>
		/// The <c>OnInterrupt</c> callbacks to be invoked by the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		protected UnityEvent OnInterrupt => m_OnInterrupt;

		/// <summary>
		/// The <c>OnComplete</c> callbacks to be invoked by the Animate object managed by this <see cref="AnimateBlock"/>.
		/// </summary>
		protected UnityEvent OnComplete => m_OnComplete;

		#endregion


		#region Private Fields - Serialized

		[SerializeField]
		private UnityEngine.Object m_Owner;

		[SerializeField]
		private string m_ReuseID;

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
		private string m_ReuseID_Auto;

		#endregion


	}

}