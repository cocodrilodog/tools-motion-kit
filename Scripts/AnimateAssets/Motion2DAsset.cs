namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using UnityEngine;

	public class Motion2DAsset : MotionBaseAsset<Vector3, Motion3D> {


		#region Public Methods

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