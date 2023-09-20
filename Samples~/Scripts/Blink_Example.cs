namespace CocodriloDog.MotionKit.Examples {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.EventSystems;

	[AddComponentMenu("")]
	public class Blink_Example : MonoBehaviour {


		#region Public Fields

		[SerializeField]
		public Blink Blink;

		#endregion


		#region Unity Methods

		private void OnEnable() {
			EventSystemUtility.AddTriggerListener(EventTrigger, EventTriggerType.PointerClick, EventTrigger_PointerClick);
		}

		private void Start() {
			m_DefaultColor = ColorAdapter.Color;
		}

		private void OnDisable() {
			EventSystemUtility.RemoveTriggerListener(EventTrigger, EventTrigger_PointerClick);
		}

		#endregion


		#region Event Handlers

		private void EventTrigger_PointerClick(BaseEventData arg0) {
			MotionKit.GetMotion(this, "Color", c => ColorAdapter.Color = c)
				.SetEasing(Blink.ColorEasing)
				.Play(Color.black, m_DefaultColor, 1);
		}

		#endregion


		#region Private Fields

		private EventTrigger m_EventTrigger;

		private ColorAdapter m_ColorAdapter;

		private Color m_DefaultColor;

		#endregion


		#region Private Properties

		private EventTrigger EventTrigger {
			get {
				if(m_EventTrigger == null) {
					m_EventTrigger = GetComponent<EventTrigger>();
				}
				return m_EventTrigger;
			}
		}

		private ColorAdapter ColorAdapter {
			get {
				if(m_ColorAdapter == null) {
					m_ColorAdapter = GetComponent<ColorAdapter>();
				}
				return m_ColorAdapter;
			}
		}

		#endregion


	}

}