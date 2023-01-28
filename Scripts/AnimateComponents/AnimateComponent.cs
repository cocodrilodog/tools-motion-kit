namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	/// <summary>
	/// Base class for all Animate components. It provides a generic interface that applies
	/// for all Animate objects.
	/// </summary>
	/// 
	/// <remarks>
	/// Animate components are <c>MonoCompositeObjects</c> that contain information about 
	/// Animate motion, timer, sequence or parallel and can create those objects at runtime. 
	/// They are intended to be used as the inspector-friendly part of the Animate engine.
	/// </remarks>
	public abstract class AnimateComponent : MonoScriptableObject {


		#region Public Properties

		/// <summary>
		/// Gets the Animate object as a <see cref="ITimedProgressable"/> so that it can be used in <see cref="Sequence"/>s.
		/// </summary>
		public abstract ITimedProgressable TimedProgressable { get; }

		/// <summary>
		/// Gets the <c>Progress</c> of the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		public abstract float Progress { get; set; }

		/// <summary>
		/// Gets the <c>CurrentTime</c> of the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		public abstract float CurrentTime { get; }

		/// <summary>
		/// Gets the <c>Duration</c> of the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		public abstract float Duration { get; }

		/// <summary>
		/// Gets the <c>IsPlaying</c> property of the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		public abstract bool IsPlaying { get; }

		/// <summary>
		/// Gets the <c>IsPaused</c> property of the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		public abstract bool IsPaused { get; }

		#endregion


		#region Public Methods

		/// <summary>
		/// The contained animate object such as <see cref="MotionBase{ValueT, MotionT}"/>, 
		/// <see cref="Timer"/> or <see cref="Sequence"/> should be created here.
		/// </summary>
		public abstract void Initialize();

		/// <summary>
		/// Plays the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		public abstract void Play();

		/// <summary>
		/// Plays the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		public abstract void Stop();

		/// <summary>
		/// Pauses the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		public abstract void Pause();

		/// <summary>
		/// Resumes the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		public abstract void Resume();

		/// <summary>
		/// Disposes any Animate object that was created with this asset as owner.
		/// </summary>
		public virtual void Dispose() {
			Animate.ClearPlaybacks(this);
		}

		#endregion


		#region Protected Properties

		/// <summary>
		/// The unique ID to be used by the Animate object.
		/// </summary>
		/// 
		/// <remarks>
		/// If no ID is provided in the inspector, an automatic unique ID will be created.
		/// Specifying a <c>ReuseID</c> in the inspector may be helpful to make some animations that
		/// set values on the same properties not to conflict with each other. 
		/// 
		/// For example, two motions that set the position property of an object may have the same 
		/// <c>ReuseID</c> so that when one is interrupted by the other, the new one stops the previous
		/// one. This avoids the two animations trying to animate the same property with different 
		/// values at the same time.
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
		/// The duration of the Animate object as specified in the inspector.
		/// </summary>
		/// <remarks>
		/// This was named differently in order to differentiate it from <see cref="Duration"/>.
		/// </remarks>
		protected float DurationInput => m_Duration;

		/// <summary>
		/// The time mode to be used by the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		protected TimeMode TimeMode => m_TimeMode;

		/// <summary>
		/// The easing to be used by the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		protected AnimateEasingField Easing => m_Easing;

		/// <summary>
		/// The <c>OnStart</c> callbacks to be invoked by the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		protected UnityEvent OnStart => m_OnStart;

		/// <summary>
		/// The <c>OnUpdate</c> callbacks to be invoked by the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		protected UnityEvent OnUpdate => m_OnUpdate;

		/// <summary>
		/// The <c>OnInterrupt</c> callbacks to be invoked by the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		protected UnityEvent OnInterrupt => m_OnInterrupt;

		/// <summary>
		/// The <c>OnComplete</c> callbacks to be invoked by the Animate object managed by this <see cref="AnimateComponent"/>.
		/// </summary>
		protected UnityEvent OnComplete => m_OnComplete;

		#endregion


		#region Private Fields - Serialized

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