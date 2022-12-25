namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using UnityEngine;

	/// <summary>
	/// Asset for <see cref="Vector2"/> motions.
	/// </summary>
	/// 
	/// <remarks>
	/// This is a Motion3DAsset, but overrides the necessary members to adapt it for 2D motions.
	/// There was no <see cref="MotionBase{ValueT, MotionT}"/> created for <see cref="Vector2"/>
	/// because it made the <c>Animate</c> API very akward when it came to creating setters for 
	/// <see cref="Vector3"/> vs <see cref="Vector2"/>. It showed an ambiguity error so I decided 
	/// to only leave the <see cref="Motion3D"/>
	/// </remarks>
	public class Motion2DAsset : MotionBaseAsset<Vector3, Motion3D> {


		#region Public Methods

		/// <summary>
		/// This creates a <see cref="Motion3D"/> but with a <see cref="Vector2"/> setter.
		/// </summary>
		public override void Initialize() {

			var setterStringParts = SetterString.Split('/');

			Action<Vector2> setterDelegate;

			var gameObject = Object as GameObject;
			if (gameObject != null) {

				var component = gameObject.GetComponent(setterStringParts[0]);

				var methodInfo = component.GetType().GetMethod(setterStringParts[1]);
				if (methodInfo != null) {
					setterDelegate = GetDelegate(component, methodInfo);
				} else {
					var propertyInfo = component.GetType().GetProperty(setterStringParts[1]);
					setterDelegate = GetDelegate(component, propertyInfo.GetSetMethod());
				}

				m_Motion = CreateMotion(setterDelegate);

			} else {
				// TODO: Possibly work with ScriptableObjects (and fields)
			}

			Action<Vector2> GetDelegate(object target, MethodInfo setMethod) {
				return (Action<Vector2>)Delegate.CreateDelegate(typeof(Action<Vector2>), target, setMethod);
			}

		}

		#endregion


		#region Private Methods

		/// <summary>
		/// Instead of overriding the base member <c>MotionT CreateMotion(Action<ValueT> setterDelegate)</c>, I
		/// created a specific version that receives a <see cref="Vector2"/>, but returns a <see cref="Motion3D"/>
		/// object.
		/// </summary>
		/// <param name="setterDelegate">A setter for <see cref="Vector2"/></param>
		/// <returns>A <see cref="Motion3D"/> object</returns>
		private Motion3D CreateMotion(Action<Vector2> setterDelegate) {;

			var motion = Animate.GetMotion(this, ReuseID, v => setterDelegate(v))
				.SetEasing(Easing.Vector3Easing)
				.SetValuesAndDuration(InitialValue, FinalValue, DurationInput)
				.SetTimeMode(TimeMode);

			// This approach will only work if the listeners are added via editor
			if (OnStart.GetPersistentEventCount() > 0) motion.SetOnStart(OnStart.Invoke);
			if (OnUpdate.GetPersistentEventCount() > 0) motion.SetOnUpdate(OnUpdate.Invoke);
			if (OnInterrupt.GetPersistentEventCount() > 0) motion.SetOnInterrupt(OnInterrupt.Invoke);
			if (OnComplete.GetPersistentEventCount() > 0) motion.SetOnComplete(OnComplete.Invoke);

			return motion;

		}

		#endregion


	}

}