namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using UnityEngine;
	using UnityEngine.Events;

	public abstract class MotionBaseAsset<ValueT, MotionT> : AnimateAsset
		where MotionT : MotionBase<ValueT, MotionT> {


		#region Public Methods

		public MotionT GetMotion() {

			var setterStringParts = SetterString.Split('/');
			MotionT motion = null;
			Action<ValueT> setterDelegate;
			
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

		}

		#endregion


		#region Protected Properties

		protected UnityEngine.Object Object => m_Object;

		protected string ReuseID => m_ReuseID = m_ReuseID ?? Guid.NewGuid().ToString();

		protected string SetterString => m_SetterString;

		protected Vector3 InitialValue => m_InitialValue;

		protected Vector3 FinalValue => m_FinalValue;

		protected float Duration => m_Duration;

		protected AnimateEasingField Easing => m_Easing;

		#endregion


		#region Protected Methods

		protected virtual MotionT CreateMotion(Action<ValueT> setterDelegate) => null;

		#endregion


		#region Private Fields - Serialized

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


		#region Private Fields - Non Serialized

		[NonSerialized]
		private string m_ReuseID;

		#endregion


		#region Private Methods

		Action<ValueT> GetDelegate(object target, MethodInfo setMethod) {
			return (Action<ValueT>)Delegate.CreateDelegate(typeof(Action<ValueT>), target, setMethod);
		}

		#endregion


	}

}