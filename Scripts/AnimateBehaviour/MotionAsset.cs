namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using UnityEngine;
	using UnityEngine.Events;

	public class MotionAsset<T> : AnimateAsset {


		#region Public Methods

		public Motion3D GetMotion() {

			var setterStringParts = SetterString.Split('/');
			var gameObject = Object as GameObject;
			var component = gameObject.GetComponent(setterStringParts[0]);

			Motion3D motion3D = null;
			Action<Vector3> setterDelegate = null;

			var methodInfo = component.GetType().GetMethod(setterStringParts[1]);
			if (methodInfo != null) {
				setterDelegate = GetDelegate<Vector3>(component, methodInfo);
			} else {
				var propertyInfo = component.GetType().GetProperty(setterStringParts[1]);
				setterDelegate = GetDelegate<Vector3>(component, propertyInfo.GetSetMethod());
			}

			motion3D = Animate.GetMotion(Set3DValue)
				.SetEasing(Easing.Vector3Easing)
				.Play(InitialValue, FinalValue, Duration);

			void Set3DValue(Vector3 value) {
				setterDelegate(value);
			}

			Action<T2> GetDelegate<T2>(object target, MethodInfo setMethod) {
				return (Action<T2>)Delegate.CreateDelegate(typeof(Action<T2>), target, setMethod);
			}

			return motion3D;

		}



		#endregion


		#region Private Fields

		[SerializeField]
		private UnityEngine.Object m_Object;

		[SerializeField]
		private string m_SetterString;

		[SerializeField]
		private Vector3 m_InitialValue;

		[SerializeField]
		private Vector3 m_FinalValue;

		[SerializeField]
		private float m_Duration;

		[SerializeField]
		private AnimateEasingField m_Easing;

		[SerializeField]
		private UnityEvent m_OnComplete;

		#endregion


		#region Private Properties

		private UnityEngine.Object Object => m_Object;

		private string SetterString => m_SetterString;

		private Vector3 InitialValue => m_InitialValue;

		private Vector3 FinalValue => m_FinalValue;

		private float Duration => m_Duration;

		private AnimateEasingField Easing => m_Easing;

		#endregion


	}

}