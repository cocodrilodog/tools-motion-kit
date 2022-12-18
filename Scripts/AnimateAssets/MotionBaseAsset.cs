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

		public override float Progress => Motion.Progress;

		public override float CurrentTime => Motion.CurrentTime;

		public override float Duration => Motion.Duration;

		public override bool IsPlaying => Motion.IsPlaying;

		public override bool IsPaused => Motion.IsPaused;

		#endregion


		#region Public Methods

		public override void Initialize() {

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

				m_Motion = CreateMotion(setterDelegate);

			} else {
				// TODO: Possibly work with ScriptableObjects (and fields)
			}

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

		protected MotionT Motion {
			get {
				if (m_Motion == null) {
					Initialize();
				}
				return m_Motion;
			}
		}

		protected UnityEngine.Object Object => m_Object;

		protected string ReuseID => m_ReuseID = m_ReuseID ?? Guid.NewGuid().ToString();

		protected string SetterString => m_SetterString;

		protected ValueT InitialValue => m_InitialValue;

		protected ValueT FinalValue => m_FinalValue;

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

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private string m_ReuseID;

		#endregion


	}

}