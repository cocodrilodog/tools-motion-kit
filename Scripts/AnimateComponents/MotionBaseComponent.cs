namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using UnityEngine;
	using UnityEngine.Events;

	public interface IMotionBaseComponent : IAnimateComponent {
		void ResetMotion();
	}

	/// <summary>
	/// Base class for motion components.
	/// </summary>
	/// <typeparam name="ValueT">The animatable type of the motion objects.</typeparam>
	/// <typeparam name="MotionT">The motion type.</typeparam>
	public abstract class MotionBaseComponent<ValueT, MotionT> : AnimateComponent, IMotionBaseComponent
		where MotionT : MotionBase<ValueT, MotionT> {


		#region Public Properties

		/// <summary>
		/// The motion that this component manages.
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

		/// <summary>
		/// Creates the setter delegate and the getter delegate, if needed and creates the motion object.
		/// </summary>
		public override void Initialize() {

			// These are strings created by the editor like this: "Transform/localPosition"
			var setterStringParts = SetterString.Split('/');
			var getterStringParts = GetterString.Split('/');

			// Variables for setter and getter
			Component component;
			MethodInfo methodInfo;

			if(Object == null) {
				throw new InvalidOperationException($"{this.name}: Object can not be null.");
			} else if (SetterString == "No Function") {
				throw new InvalidOperationException($"{this.name}: A setter function must be set.");
			}

			var gameObject = Object as GameObject;
			if (gameObject != null) {

				// SETTER
				//
				// The first part of the setter string is the component
				component = gameObject.GetComponent(setterStringParts[0]);

				// The second part is the setter. First we'll look for the method setter
				// Check for 1 ValueT parameter to avoid ambiguity
				methodInfo = component.GetType().GetMethod(setterStringParts[1], new Type[] { typeof(ValueT) });
				if (methodInfo != null) {
					m_SetterDelegate = GetSetterDelegate(component, methodInfo);
				} else {
					// If a method setter is not found, then we look for a property setter
					var propertyInfo = component.GetType().GetProperty(setterStringParts[1]);
					m_SetterDelegate = GetSetterDelegate(component, propertyInfo.GetSetMethod());
				}

				if (InitialValueIsRelative || FinalValueIsRelative) {

					if (GetterString == "No Function") {
						throw new InvalidOperationException($"{this.name}: A getter function must be set.");
					}

					// GETTER
					//
					// The first part of the getter string is the component
					component = gameObject.GetComponent(getterStringParts[0]);

					// The second part is the getter. First we'll look for the method getter
					// Check for 0 parameters to avoid ambiguity
					methodInfo = component.GetType().GetMethod(getterStringParts[1], new Type[] { }); 
					if (methodInfo != null) {
						m_GetterDelegate = GetGetterDelegate(component, methodInfo);
					} else {
						// If a method getter is not found, then we look for a property getter
						var propertyInfo = component.GetType().GetProperty(getterStringParts[1]);
						m_GetterDelegate = GetGetterDelegate(component, propertyInfo.GetGetMethod());
					}

				}

				// Initialize the motion
				ResetMotion();

			} else {
				// TODO: Possibly work with ScriptableObjects (and fields)
			}

			// Creates a delegate for the target and method info. Here is a discussion about this technique:
			// https://blogs.msmvps.com/jonskeet/2008/08/09/making-reflection-fly-and-exploring-delegates/

			Action<ValueT> GetSetterDelegate(object target, MethodInfo setMethod) {
				return (Action<ValueT>)Delegate.CreateDelegate(typeof(Action<ValueT>), target, setMethod);
			}
			Func<ValueT> GetGetterDelegate(object target, MethodInfo getMethod) {
				return (Func<ValueT>)Delegate.CreateDelegate(typeof(Func<ValueT>), target, getMethod);
			}

		}

		/// <summary>
		/// Reassigns the values of the motion.
		/// </summary>
		/// 
		/// <remarks>
		/// This can be used to update the initial and final values if they are set to be relative because they
		/// need to be calculated before the animation begins.
		/// </remarks>
		public virtual void ResetMotion() {
			m_Motion = CreateMotion(m_SetterDelegate, m_GetterDelegate);
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
		/// A string that points to the getter of the animatable property.
		/// </summary>
		protected string GetterString => m_GetterString;

		/// <summary>
		/// The initial value for the motion.
		/// </summary>
		protected ValueT InitialValue => m_InitialValue;
		
		/// <summary>
		/// Whether the initial value for the motion is relative or not.
		/// </summary>
		/// 
		/// <remarks>
		/// If it is relative, it will be summed to the current value of the property in the target object
		/// to calculate the value that this motion starts with.
		/// </remarks>
		protected bool InitialValueIsRelative => m_InitialValueIsRelative;

		/// <summary>
		/// The final value for the motion.
		/// </summary>
		protected ValueT FinalValue => m_FinalValue;

		/// <summary>
		/// Whether the final value for the motion is relative or not.
		/// </summary>
		/// 
		/// <remarks>
		/// If it is relative, it will be summed to the current value of the property in the target object
		/// to calculate the value that this motion finalizes with.
		/// </remarks>
		protected bool FinalValueIsRelative => m_FinalValueIsRelative;

		#endregion


		#region Protected Methods

		/// <summary>
		/// This is implemented in subclasses to create the concrete type of motion that each class is related to.
		/// </summary>
		/// <param name="setterDelegate">The setter to be used in the motion object.</param>
		/// <returns>The motion</returns>
		protected virtual MotionT CreateMotion(Action<ValueT> setterDelegate, Func<ValueT> getterDelegate) => null;

		#endregion


		#region Private Fields - Serialized

		[SerializeField]
		private UnityEngine.Object m_Object;

		[SerializeField]
		private string m_SetterString;
		
		[SerializeField]
		private string m_GetterString;

		[SerializeField]
		private ValueT m_InitialValue;

		[SerializeField]
		private bool m_InitialValueIsRelative;

		[SerializeField]
		private ValueT m_FinalValue;

		[SerializeField]
		private bool m_FinalValueIsRelative;

		#endregion


		#region Private Fields

		[NonSerialized]
		private Action<ValueT> m_SetterDelegate;

		[NonSerialized]
		private Func<ValueT> m_GetterDelegate;

		#endregion


	}

}