namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using UnityEngine;

	/// <summary>
	/// Block for <see cref="Vector2"/> motions.
	/// </summary>
	/// 
	/// <remarks>
	/// This is a Motion3DBlock, but overrides the necessary members to adapt it for 2D motions.
	/// There was no <see cref="MotionBase{ValueT, MotionT}"/> created for <see cref="Vector2"/>
	/// because it made the <see cref="MotionKit"/> API very akward when it came to creating setters for 
	/// <see cref="Vector3"/> vs <see cref="Vector2"/>. It showed an ambiguity error so I decided 
	/// to only leave the <see cref="Motion3D"/>
	/// </remarks>
	[Serializable]
	public class Motion2DBlock : MotionBaseBlock<Vector3, Motion3D, Motion2DValues> {


		#region Public Methods

		/// <summary>
		/// This creates a <see cref="Motion3D"/> but with a <see cref="Vector2"/> setter.
		/// </summary>
		public override void Initialize() {

			// HACK: The code in this method is copied from the MotionBlock implementation, 
			// but with Vector2 as type.

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
				methodInfo = component.GetType().GetMethod(setterStringParts[1], new Type[] { typeof(Vector2) });
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

			Action<Vector2> GetSetterDelegate(object target, MethodInfo setMethod) {
				return (Action<Vector2>)Delegate.CreateDelegate(typeof(Action<Vector2>), target, setMethod);
			}
			Func<Vector2> GetGetterDelegate(object target, MethodInfo getMethod) {
				return (Func<Vector2>)Delegate.CreateDelegate(typeof(Func<Vector2>), target, getMethod);
			}

		}

		public override void ResetPlayback() {
			m_Motion = GetMotion(m_SetterDelegate, m_GetterDelegate);
		}

		public override string DefaultName => "Motion2D";

		#endregion


		#region Private Methods

		/// <summary>
		/// Instead of overriding the base member <c>MotionT GetMotion(Action<ValueT> setterDelegate, Func<ValueT> getterDelegate)</c>,
		/// I created a specific version that receives a <see cref="Vector2"/>, but returns a <see cref="Motion3D"/>
		/// object.
		/// </summary>
		/// <param name="setterDelegate">A setter for <see cref="Vector2"/></param>
		/// <param name="getterDelegate">A getter for <see cref="Vector2"/></param>
		/// <returns>A <see cref="Motion3D"/> object</returns>
		private Motion3D GetMotion(Action<Vector2> setterDelegate, Func<Vector2> getterDelegate) {

			// HACK: The code in this method is copied from the MotionBaseAsset implementation, 
			// but casting the values to Vector2 to solve the ambiguity.

			// Motion, easing and time mode
			var motion = MotionKit.GetMotion(Owner, ReuseID, v => setterDelegate(v))
				.SetEasing(Easing.Vector3Easing)
				.SetTimeMode(TimeMode);

			// Set values and duration
			motion.SetInitialValue(InitialValueIsRelative ? getterDelegate() + (Vector2)InitialValue : InitialValue);
			motion.SetFinalValue(FinalValueIsRelative ? getterDelegate() + (Vector2)FinalValue : FinalValue);
			motion.SetDuration(DurationInput);

			// Callbacks: This approach will only work if the listeners are added via editor
			motion.SetOnStart(() => {
				if (InitialValueIsRelative || FinalValueIsRelative) {
					if (m_DontResetRelativeValuesOnStart) {
						// Reset the flag
						m_DontResetRelativeValuesOnStart = false;
					} else {
						// By default, reset the motion when we have relative values
						ResetPlayback();
					}
				}
				if (OnStart.GetPersistentEventCount() > 0) OnStart.Invoke();
			});
			if (OnUpdate.GetPersistentEventCount() > 0) motion.SetOnUpdate(OnUpdate.Invoke);
			if (OnInterrupt.GetPersistentEventCount() > 0) motion.SetOnInterrupt(OnInterrupt.Invoke);
			if (OnComplete.GetPersistentEventCount() > 0) motion.SetOnComplete(OnComplete.Invoke);

			return motion;

		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private Action<Vector2> m_SetterDelegate;

		[NonSerialized]
		private Func<Vector2> m_GetterDelegate;

		#endregion


	}

}