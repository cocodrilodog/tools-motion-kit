namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	/// <summary>
	/// Interface to be implemented by <see cref="MotionKitBlock"/>. It exposes the members 
	/// that are used for playback and initialization of the <see cref="MotionKit"/> objects.
	/// </summary>
	/// 
	/// <remarks>
	/// It was created so that a <see cref="MotionBlock{ValueT, MotionT}"/> is easily
	/// identifiable with <see cref="IMotionBaseBlock"/> instead of using the concrete types
	/// derived from the template.
	/// </remarks>
	public interface IMotionKitBlock {
		bool IsInitialized { get; }
		ITimedProgressable TimedProgressable { get; }
		float Progress { get; set; }
		float CurrentTime { get; }
		float Duration { get; }
		bool IsPlaying { get; }
		bool IsPaused { get; }
		void Initialize();
		void TryResetPlayback(bool recursive);
		void LockResetPlayback(bool recursive);
		void UnlockResetPlayback(bool recursive);
		void ForceResetPlayback();
		void Play();
		void Stop();
		void Pause();
		void Resume();
		void Dispose();
	}

	/// <summary>
	/// Base class for all MotionKit blocks. It provides a generic interface that applies
	/// for all MotionKit objects.
	/// </summary>
	/// 
	/// <remarks>
	/// MotionKit blocks are <see cref="CompositeObject"/>s that contain information about 
	/// MotionKit motion, timer, sequence or parallel and can create those objects at runtime. 
	/// They are intended to be used as the inspector-friendly part of the MotionKit engine.
	/// </remarks>
	[Serializable]
	public abstract class MotionKitBlock : CompositeObject, IMotionKitBlock {


		#region Public Properties

		/// <summary>
		/// Has this block been already initialized?
		/// </summary>
		public abstract bool IsInitialized { get; }

		/// <summary>
		/// Playback needs to be reset upon <see cref="Play"/> or <c>OnStart</c>.
		/// </summary>
		/// 
		/// <remarks>
		/// 
		/// <see cref="MotionKitBlock"/>s that have the same <see cref="Owner"/> and <see cref="ReuseID"/> 
		/// will share the same playback object. So when any of them is reset, it will pass their corresponding 
		/// <c>OnStart</c> callback to the shared playback object. For that reason, they need to be reset on 
		/// <see cref="Play"/>, otherwise, the <c>OnStart</c> of one <see cref="MotionKitBlock"/> may be 
		/// triggered when another one is played.
		/// 
		/// <para>
		/// For example, if a <see cref="MotionKitBlock"/> object A is reset, then another one B is reset upon 
		/// initialization (both with the same <see cref="Owner"/> and <see cref="ReuseID"/>), their shared playback 
		/// object will have the <c>OnStart</c> of B, so when the <see cref="MotionKitBlock"/> A is played, it will 
		/// trigger B's <c>OnStart</c>, unless <see cref="ResetPlayback"/> is called on A upon <see cref="Play"/>
		/// </para> 
		/// 
		/// </remarks>
		public virtual bool ShouldResetPlayback => m_Owner != null && !string.IsNullOrEmpty(m_ReuseID);

		/// <summary>
		/// While <c>true</c>, the motion kit block won't be reset.
		/// </summary>
		public bool IsResetPlaybackLocked => m_IsResetPlaybackLocked;

		/// <summary>
		/// Gets the MotionKit object as a <see cref="ITimedProgressable"/> so that it can be used in <see cref="Sequence"/>s.
		/// </summary>
		public abstract ITimedProgressable TimedProgressable { get; }

		/// <summary>
		/// Gets the <c>Progress</c> of the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		public abstract float Progress { get; set; }

		/// <summary>
		/// Gets the <c>CurrentTime</c> of the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		public abstract float CurrentTime { get; }

		/// <summary>
		/// Gets the <c>Duration</c> of the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		public abstract float Duration { get; }

		/// <summary>
		/// Gets the <c>IsPlaying</c> property of the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		public abstract bool IsPlaying { get; }

		/// <summary>
		/// Gets the <c>IsPaused</c> property of the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		public abstract bool IsPaused { get; }

		/// <summary>
		/// The duration of the MotionKit object as specified in the inspector.
		/// </summary>
		/// <remarks>
		/// This was named differently in order to differentiate it from <see cref="Duration"/>.
		/// </remarks>
		public float DurationInput {
			get => SharedSettings != null ? SharedSettings.Duration : m_Duration;
			set => m_Duration = value;
		}

		/// <summary>
		/// The duration that will be used for the MotionKit object.
		/// </summary>
		/// <remarks>
		/// By default, it will be <see cref="DurationInput"/>, but <see cref="SequenceBlock"/> and
		/// <see cref="ParallelBlock"/> implement a different logic.
		/// </remarks>
		public virtual float DurationToBeUsed => DurationInput;

		/// <summary>
		/// Shows the <see cref="DisplayName"/> with the <see cref="DurationToBeUsed"/> in seconds.
		/// </summary>
		public override string DisplayName => $"{base.DisplayName} ({DurationToBeUsed}s)";

		/// <summary>
		/// The time mode to be used by the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		public TimeMode TimeMode {
			get => SharedSettings != null ? SharedSettings.TimeMode : m_TimeMode;
			set => m_TimeMode = value;
		}

		/// <summary>
		/// The easing to be used by the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		public MotionKitEasingField Easing {
			get => SharedSettings != null ? SharedSettings.Easing : m_Easing;
			set => m_Easing = value;
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// The contained MotionKit object such as <see cref="MotionBase{ValueT, MotionT}"/>, 
		/// <see cref="Timer"/> or <see cref="Sequence"/> should be created here.
		/// </summary>
		public abstract void Initialize();

		/// <summary>
		/// This is called in <see cref="Play"/> and on the playbackObject's <c>onStart</c>
		/// </summary>
		public virtual void TryResetPlayback(bool recursive) {
			Debug.Log($"{Name}: TryResetPlayback: IsResetPlaybackLocked: {IsResetPlaybackLocked}");
			if(ShouldResetPlayback && !IsResetPlaybackLocked) {
				if (!IsInitialized) {
					Initialize(); // This will reset anyway
				} else {
					ResetPlayback();
				}
			}
		}

		public void ForceResetPlayback() => ResetPlayback();

		/// <summary>
		/// Prevents the block from being reset until <see cref="UnlockResetPlayback"/> is called.
		/// </summary>
		public virtual void LockResetPlayback(bool recursive) {
			if (!IsInitialized) {
				Initialize();
			}
			Debug.Log($"{Name}: LockResetPlayback");
			m_IsResetPlaybackLocked = true;
		}

		/// <summary>
		/// Allows the block to be reset from now on.
		/// </summary>
		public virtual void UnlockResetPlayback(bool recursive) {
			if (Name == "TitleMotion") {
				Debug.Log($"{Name}: UnlockResetPlayback");
			}
			m_IsResetPlaybackLocked = false;
		}

		/// <summary>
		/// Plays the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		public virtual void Play() {
			// Try reset recursive here so that all children blocks are guaranteed to have their 
			// corresponding OnStart, in case they are sharing their motion with other blocks via Owner
			// and ReuseID. Then when each children reach OnStart, they will trigger the proper method.
			//
			// Read the ShouldResetPlayback remarks for more info!
			TryResetPlayback(true);
		}

		/// <summary>
		/// Plays the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		public abstract void Stop();

		/// <summary>
		/// Pauses the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		public abstract void Pause();

		/// <summary>
		/// Resumes the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		public abstract void Resume();

		/// <summary>
		/// Disposes any MotionKit object that was created with this asset as owner.
		/// </summary>
		public virtual void Dispose() {
			if (IsInitialized) {
				MotionKit.ClearPlayback(Owner, ReuseID);
			}
		}

		#endregion


		#region Protected Properties

		/// <summary>
		/// The owner to be used by the MotionKit object.
		/// </summary>
		/// 
		/// <remarks>
		/// If no <see cref="Owner"/>  is provided in the inspector, it will default to the <see cref="MotionKit"/>
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
					return MotionKit.Instance;
				}
			}
		}

		/// <summary>
		/// The unique ID to be used by the MotionKit object.
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
		/// The <c>OnStart</c> callbacks to be invoked by the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		protected UnityEvent OnStart => m_OnStart;

		/// <summary>
		/// The <c>OnUpdate</c> callbacks to be invoked by the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		protected UnityEvent OnUpdate => m_OnUpdate;

		/// <summary>
		/// The <c>OnInterrupt</c> callbacks to be invoked by the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		protected UnityEvent OnInterrupt => m_OnInterrupt;

		/// <summary>
		/// The <c>OnComplete</c> callbacks to be invoked by the MotionKit object managed by this <see cref="MotionKitBlock"/>.
		/// </summary>
		protected UnityEvent OnComplete => m_OnComplete;

		#endregion


		#region Protected Methods

		/// <summary>
		/// Creates or updates the playback object by invoking the playback factory method at 
		/// <see cref="MotionKit"/>.
		/// </summary>
		/// <remarks>
		/// This is called in the <see cref="Play"/> method when <see cref="ShouldResetPlayback"/> is <c>true</c>
		/// and <see cref="IsResetPlaybackLocked"/> is false.
		/// </remarks>
		protected abstract void ResetPlayback();

		#endregion


		#region Private Fields - Serialized

		[Tooltip(
			"(Optional) Assign an object here if you want to overrride the default owner which is the MotionKit singleton. " +
			"See the documentation for more info on Owner - ReuseID."
		)]
		[SerializeField]
		private UnityEngine.Object m_Owner;

		[Tooltip(
			"(Optional) Assign a reuse ID here if you want to overrride the default ReuseID which is automatically generated " +
			"at runtime and unique. See the documentation for more info on Owner - ReuseID."
		)]
		[SerializeField]
		private string m_ReuseID;

		[SerializeField]
		private float m_Duration;

		[SerializeField]
		private TimeMode m_TimeMode;

		[SerializeField]
		private MotionKitEasingField m_Easing;

		[SerializeField]
		private MotionKitSettings m_SharedSettings;

		[SerializeField]
		private UnityEvent m_OnStart;

		[SerializeField]
		private UnityEvent m_OnUpdate;

		[SerializeField]
		private UnityEvent m_OnInterrupt;

		[SerializeField]
		private UnityEvent m_OnComplete;

		[SerializeField]
		private int m_CallbackSelection;

		[SerializeField]
		private bool m_EditOwnerAndReuseID;

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private string m_ReuseID_Auto;

		[NonSerialized]
		private bool m_IsResetPlaybackLocked;

		#endregion


		#region Private Properties

		private MotionKitSettings SharedSettings => m_SharedSettings;

		#endregion


	}

}