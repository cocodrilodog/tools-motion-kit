namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using UnityEngine;

	public class Motion2DAsset : MotionBaseAsset<Vector3, Motion3D> {


		#region Protected Methods

		public override Motion3D GetMotion() {

			var setterStringParts = SetterString.Split('/');
			Motion3D motion = null;
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

				motion = CreateMotion(setterDelegate);

			} else {
				// TODO: Possibly work with ScriptableObjects (and fields)
			}

			return motion;

			Action<Vector2> GetDelegate(object target, MethodInfo setMethod) {
				return (Action<Vector2>)Delegate.CreateDelegate(typeof(Action<Vector2>), target, setMethod);
			}

		}

		private Motion3D CreateMotion(Action<Vector2> setterDelegate) {;

			var motion = Animate.GetMotion(this, ReuseID, v => setterDelegate(v))
				.SetEasing(Easing.Vector3Easing)
				.SetValuesAndDuration(InitialValue, FinalValue, Duration);

			return motion;

		}

		#endregion


	}

}