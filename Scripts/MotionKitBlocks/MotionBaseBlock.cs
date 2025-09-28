namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using UnityEngine;
	using UnityEngine.Events;

	/// <summary>
	/// Interface to be implemented by <see cref="MotionBlock{ValueT, MotionT}"/>
	/// </summary>
	/// 
	/// <remarks>
	/// It was created so that a <see cref="MotionBlock{ValueT, MotionT}"/> is easily
	/// identifiable instead of using the concrete types derived from the template.
	/// </remarks>
	public interface IMotionBaseBlock : IMotionKitBlock {
		UnityEngine.Object Object { get; set; }
	}

	/// <summary>
	/// A <see cref="IMotionBaseBlock"/> with the provided <typeparamref name="T"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IMotionBaseBlock<T> : IMotionBaseBlock { 
		public T InitialValue { get; set; }
		public T FinalValue { get; set; }
	}

	/// <summary>
	/// Base class for motion blocks.
	/// </summary>
	/// <typeparam name="ValueT">The animatable type of the motion objects.</typeparam>
	/// <typeparam name="MotionT">The motion type.</typeparam>
	public abstract class MotionBaseBlock<ValueT, MotionT, SharedValuesT> : MotionKitBlock, IMotionBaseBlock<ValueT>
		where MotionT : MotionBase<ValueT, MotionT>
		where SharedValuesT : MotionValues<ValueT> {


		#region Public Properties

		public override bool IsInitialized => m_Motion != null;

		public override bool ShouldResetPlayback => 
			base.ShouldResetPlayback || m_Motion == null || InitialValueIsRelative || FinalValueIsRelative || m_HaveValuesChanged;

		/// <summary>
		/// The object that will be target of the animatable property.
		/// </summary>
		public UnityEngine.Object Object {
			get => m_Object;
			set => m_Object = value;
		}

		/// <summary>
		/// The motion that this block manages.
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

		/// <summary>
		/// The initial value for the motion.
		/// </summary>
		public virtual ValueT InitialValue {
			get => SharedValues != null ? SharedValues.InitialValue : m_InitialValue;
			set {
				_ = SharedValues != null ? SharedValues.InitialValue = value : m_InitialValue = value;
				m_HaveValuesChanged = true; // TODO: Handle equivalent logic for SharedValues
			}
		}

		/// <summary>
		/// Whether the initial value for the motion is relative or not.
		/// </summary>
		/// 
		/// <remarks>
		/// If it is relative, the <see cref="InitialValue"/> will be summed to the current value of the property
		/// at the beginning of the animation and the result will be used as the initial value for the motion.
		/// </remarks>
		public bool InitialValueIsRelative {
			get => SharedValues != null ? SharedValues.InitialValueIsRelative : m_InitialValueIsRelative;
			set {
				if (Application.isPlaying) {
					throw new InvalidOperationException($"{nameof(InitialValueIsRelative)} can not be set at runtime.");
				}
				m_InitialValueIsRelative = value;
			}
		}

		/// <summary>
		/// The final value for the motion.
		/// </summary>
		public virtual ValueT FinalValue {
			get => SharedValues != null ? SharedValues.FinalValue : m_FinalValue;
			set {
				_ = SharedValues != null ? SharedValues.FinalValue = value : m_FinalValue = value;
				m_HaveValuesChanged = true;  // TODO: Handle equivalent logic for SharedValues
			}
		}

		/// <summary>
		/// Whether the final value for the motion is relative or not.
		/// </summary>
		/// 
		/// <remarks>
		/// If it is relative, the <see cref="FinalValue"/> will be summed to the current value of the property
		/// at the beginning of the animation and the result will be used as the initial value for the motion.
		/// </remarks>
		public bool FinalValueIsRelative {
			get => SharedValues != null ? SharedValues.FinalValueIsRelative : m_FinalValueIsRelative;
			set {
				if (Application.isPlaying) {
					throw new InvalidOperationException($"{nameof(FinalValueIsRelative)} can not be set at runtime.");
				}
				m_FinalValueIsRelative = value;
			}
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Creates the setter delegate and the getter delegate, if needed and creates the motion object.
		/// </summary>
		public override void Initialize() {

			base.Initialize();

			if (Application.isPlaying) {

				// These are strings created by the editor like this: "Transform/localPosition"
				var setterStringParts = SetterString.Split('/');
				var getterStringParts = GetterString.Split('/');

				// Variables for setter and getter
				Component component;
				MethodInfo methodInfo;

				if (Object == null) {
					throw new InvalidOperationException($"{Name}: Object can not be null.");
				} else if (SetterString == "No Function") {
					throw new InvalidOperationException($"{Name}: A setter function must be set.");
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
							throw new InvalidOperationException($"{Name}: A getter function must be set.");
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
					ResetPlayback();

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

				if (SharedValues != null) {
					SharedValues.OnValuesChange += SharedValues_OnValuesChange;
				}

			}

		}

		public override void Dispose() {
			base.Dispose();
			if (IsInitialized) {
				if (SharedValues != null) {
					SharedValues.OnValuesChange -= SharedValues_OnValuesChange;
				}
			}
		}

		public override void Play() {
			base.Play();
			Motion.Play();
		}

		public override void Stop() => Motion.Stop();

		public override void Pause() => Motion.Pause();

		public override void Resume() => Motion.Resume();

		#endregion


		#region Event Handlers

		private void SharedValues_OnValuesChange() => m_HaveValuesChanged = true;

		#endregion


		#region Protected Fields

		[NonSerialized]
		protected MotionT m_Motion;

		[NonSerialized]
		protected bool m_HaveValuesChanged;

		#endregion


		#region Protected Properties

		/// <summary>
		/// A string that points to the setter of the animatable property.
		/// </summary>
		protected string SetterString => m_SetterString;

		/// <summary>
		/// A string that points to the getter of the animatable property.
		/// </summary>
		protected string GetterString => m_GetterString;

		/// <summary>
		/// An asset that can be used as the overriding source of the <see cref="InitialValue"/> and <see cref="FinalValue"/>
		/// and their corresponding <see cref="InitialValueIsRelative"/> and <see cref="FinalValueIsRelative"/> properties.
		/// </summary>
		protected SharedValuesT SharedValues {
			get => m_SharedValues;
			set => m_SharedValues = value;
		}

		#endregion


		#region Protected Methods

		protected override void ResetPlayback() {
			base.ResetPlayback();
			m_HaveValuesChanged = false;
			m_Motion = GetMotion(m_SetterDelegate, m_GetterDelegate);
		}

		/// <summary>
		/// This is implemented in subclasses to create the concrete type of motion that each class is related to.
		/// </summary>
		/// <param name="setterDelegate">The setter to be used in the motion object.</param>
		/// <returns>The motion</returns>
		protected virtual MotionT GetMotion(Action<ValueT> setterDelegate, Func<ValueT> getterDelegate) => null;

		#endregion


		#region Private Fields - Serialized

		[Tooltip("The object that will be animated.")] // This tooltip won't be shown
		[SerializeField]
		private UnityEngine.Object m_Object;

		[Tooltip("The property or method that will be used for the animation.")] // This tooltip won't be shown
		[SerializeField]
		private string m_SetterString;

		[Tooltip("The read property or method that will be used when either the initial or final value is set to \"Is Relative\"")] // This tooltip won't be shown
		[SerializeField]
		private string m_GetterString;

		[Tooltip("The initial value for the motion.")]
		[SerializeField]
		private ValueT m_InitialValue;

		[Tooltip("If checked, the animation will begin at the current value plus this value.")]
		[SerializeField]
		private bool m_InitialValueIsRelative;

		[Tooltip("The final value for the motion.")]
		[SerializeField]
		private ValueT m_FinalValue;

		[Tooltip("If checked, the animation will end at the current value plus this value.")]
		[SerializeField]
		private bool m_FinalValueIsRelative;

		[Tooltip(
			"An asset that will override the InitialValue, FinalValue, and their respective Is Relative flags, and " +
			"can be shared with other motion kit blocks.")
		]
		[CreateAsset]
		[SerializeField]
		private SharedValuesT m_SharedValues;

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private Action<ValueT> m_SetterDelegate;

		[NonSerialized]
		private Func<ValueT> m_GetterDelegate;

		#endregion


	}

}