namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using UnityEngine;
	using UnityEngine.Events;

	public abstract class MotionBaseAsset<ValueT, MotionT> : AnimateAsset
		where MotionT : MotionBase<ValueT, MotionT> {


		#region Public Properties

		public override float Progress => (float)Motion?.Progress;

		public override float CurrentTime => (float)Motion?.CurrentTime;

		public override float Duration => (float)Motion?.Duration;

		public override bool IsPlaying => (bool)Motion?.IsPlaying;

		public override bool IsPaused => (bool)Motion?.IsPaused;

		#endregion


		#region Public Methods

		public virtual MotionT GetMotion() {

			var setterStringParts = SetterString.Split('/');

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

				Motion = CreateMotion(setterDelegate);

			} else {
				// TODO: Possibly work with ScriptableObjects (and fields)
			}

			return m_Motion;

			Action<ValueT> GetDelegate(object target, MethodInfo setMethod) {
				return (Action<ValueT>)Delegate.CreateDelegate(typeof(Action<ValueT>), target, setMethod);
			}

		}

		public override void Play() => Motion?.Play();

		public override void Stop() => Motion?.Stop();

		public override void Pause() => Motion?.Pause();

		public override void Resume() => Motion?.Resume();

		#endregion


		#region Protected Properties

		protected UnityEngine.Object Object => m_Object;

		protected string ReuseID => m_ReuseID = m_ReuseID ?? Guid.NewGuid().ToString();

		protected string SetterString => m_SetterString;

		protected ValueT InitialValue => m_InitialValue;

		protected ValueT FinalValue => m_FinalValue;

		protected float _Duration => m_Duration;

		protected TimeMode TimeMode => m_TimeMode;

		protected AnimateEasingField Easing => m_Easing;

		protected UnityEvent OnStart => m_OnStart;

		protected UnityEvent OnUpdate => m_OnUpdate;
		
		protected UnityEvent OnInterrupt => m_OnInterrupt;
		
		protected UnityEvent OnComplete => m_OnComplete;

		protected MotionT Motion {
			get {
				if(m_Motion == null) {
					GetMotion();
				}
				return m_Motion;
			}
			set => m_Motion = value;
		}

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
		private ValueT m_InitialValue;

		[SerializeField]
		private ValueT m_FinalValue;

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

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private MotionT m_Motion;

		[NonSerialized]
		private string m_ReuseID;

		#endregion


	}

}