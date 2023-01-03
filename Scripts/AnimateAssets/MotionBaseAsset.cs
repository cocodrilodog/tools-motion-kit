namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using UnityEngine;
	using UnityEngine.Events;

	/// <summary>
	/// Base class for motion assets.
	/// </summary>
	/// <typeparam name="ValueT">The animatable type of the motion objects.</typeparam>
	/// <typeparam name="MotionT">The motion type.</typeparam>
	public abstract class MotionBaseAsset<ValueT, MotionT> : AnimateAsset
		where MotionT : MotionBase<ValueT, MotionT> {


		#region Public Properties

		/// <summary>
		/// The motion that this asset manages.
		/// </summary>
		public MotionT Motion {
			get {
				if (m_Motion == null) {
					Initialize();
				}
				return m_Motion;
			}
		}

		public override ITimedProgressable TimedProgressable => Motion;

		public override float Progress {
			get => Motion.Progress;
			set => Motion.Progress = value;
		}

		public override float CurrentTime => Motion.CurrentTime;

		public override float Duration => Motion.Duration;

		public override bool IsPlaying => Motion.IsPlaying;

		public override bool IsPaused => Motion.IsPaused;

		#endregion


		#region Public Methods

		public override void Initialize() {

			// This is a string created by the editor like this: "Transform/localPosition"
			var setterStringParts = SetterString.Split('/');

			// The optimized way of invoking the setter of the animatable property
			Action<ValueT> setterDelegate;

			var gameObject = Object as GameObject;
			if (gameObject != null) {

				// The first part of the setter string is the component
				var component = gameObject.GetComponent(setterStringParts[0]);

				// The second part is the setter. First we'll look for the method setter
				var methodInfo = component.GetType().GetMethod(setterStringParts[1]);
				if (methodInfo != null) {
					setterDelegate = GetDelegate(component, methodInfo);
				} else {
					// If a method setter is not found, then we look for a property setter
					var propertyInfo = component.GetType().GetProperty(setterStringParts[1]);
					setterDelegate = GetDelegate(component, propertyInfo.GetSetMethod());
				}

				// Initialize the motion
				m_Motion = CreateMotion(setterDelegate);

			} else {
				// TODO: Possibly work with ScriptableObjects (and fields)
			}

			// Creates a delegate for the target and method info. Here is a discussion about this technique:
			// https://blogs.msmvps.com/jonskeet/2008/08/09/making-reflection-fly-and-exploring-delegates/
			Action<ValueT> GetDelegate(object target, MethodInfo setMethod) {
				return (Action<ValueT>)Delegate.CreateDelegate(typeof(Action<ValueT>), target, setMethod);
			}

		}

		public override void Play() => Motion.Play();

		public override void Stop() => Motion.Stop();

		public override void Pause() => Motion.Pause();

		public override void Resume() => Motion.Resume();

		#endregion


		#region Protected Fields

		[NonSerialized]
		protected MotionT m_Motion;

		#endregion


		#region Protected Properties

		/// <summary>
		/// The object that will be target of the animatable property.
		/// </summary>
		protected UnityEngine.Object Object => m_Object;

		/// <summary>
		/// A string that points to the setter of the animatable property.
		/// </summary>
		protected string SetterString => m_SetterString;

		/// <summary>
		/// The initial value for the motion.
		/// </summary>
		protected ValueT InitialValue => m_InitialValue;

		/// <summary>
		/// The final value for the motion.
		/// </summary>
		protected ValueT FinalValue => m_FinalValue;

		#endregion


		#region Protected Methods

		/// <summary>
		/// This is implemented in subclasses to create the concrete type of motion that each class is related to.
		/// </summary>
		/// <param name="setterDelegate">The setter to be used in the motion object.</param>
		/// <returns>The motion</returns>
		protected virtual MotionT CreateMotion(Action<ValueT> setterDelegate) => null;

		#endregion


		#region Private Fields - Serialized

		[SerializeField]
		private UnityEngine.Object m_Object;

		[SerializeField]
		private string m_SetterString;

		[SerializeField]
		private ValueT m_InitialValue;

		[SerializeField]
		private ValueT m_FinalValue;

		#endregion


	}

}