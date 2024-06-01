namespace CocodriloDog.MotionKit {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(menuName = "Cocodrilo Dog/MotionKit/Settings")]
	public class MotionKitSettings : ScriptableObject {


		#region Public Properties

		public float Duration {
			get => m_Duration;
			set {
				var raiseEvent = !Mathf.Approximately(value, m_Duration);
				m_Duration = value;
				if (raiseEvent) {
					OnSettingsChange?.Invoke();
				}
			}
		}

		public TimeMode TimeMode {
			get => m_TimeMode;
			set {
				var raiseEvent = value != m_TimeMode;
				m_TimeMode = value;
				if (raiseEvent) {
					OnSettingsChange?.Invoke();
				}
			}
		}

		public MotionKitEasingField Easing {
			get => m_Easing;
			set {
				var raiseEvent = value != m_Easing;
				m_Easing = value;
				if (raiseEvent) {
					OnSettingsChange?.Invoke();
				}
			}
		}

		#endregion


		#region Public Events

		public Action OnSettingsChange;

		#endregion


		#region Private Fields

		[SerializeField]
		private float m_Duration;

		[SerializeField]
		private TimeMode m_TimeMode;

		[SerializeField]
		private MotionKitEasingField m_Easing;

		#endregion


	}

}