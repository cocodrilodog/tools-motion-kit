namespace CocodriloDog.Animation.Examples {

	using CocodriloDog.Rendering;
	using CocodriloDog.Utility;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.EventSystems;

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
			m_DefaultColor = ColorModifier.Color;
		}

		private void OnDisable() {
			EventSystemUtility.RemoveTriggerListener(EventTrigger, EventTrigger_PointerClick);
		}

		#endregion


		#region Event Handlers

		private void EventTrigger_PointerClick(BaseEventData arg0) {
			Animate.GetMotion(this, "Color", c => ColorModifier.Color = c)
				.SetEasing(Blink.ColorEasing)
				.Play(m_DefaultColor, Color.black, 1)
				.SetOnComplete(() => ColorModifier.Color = m_DefaultColor);
		}

		#endregion


		#region Private Fields

		private EventTrigger m_EventTrigger;

		private ColorModifier m_ColorModifier;

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

		private ColorModifier ColorModifier {
			get {
				if(m_ColorModifier == null) {
					m_ColorModifier = GetComponent<ColorModifier>();
				}
				return m_ColorModifier;
			}
		}

		#endregion


	}

}