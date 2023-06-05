namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[AddComponentMenu("")]
	public class OwnerAndReuseID_Example : MonoBehaviour {


		#region Public Properties

		public float PropertyFloat {
			get => transform.localScale.x;
			set {
				var scale = transform.localScale;
				scale.x = value;
				transform.localScale = scale;
			}
		}

		public Vector2 Property2D {
			get => transform.localScale;
			set {
				var scale = transform.localScale;
				scale.x = value.x;
				scale.y = value.y;
				transform.localScale = scale;
			}
		}

		#endregion


		#region Event Handlers

		public void OnClick() {
			IsSelected = !IsSelected;
		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private MotionKitComponent m_MotionKitComponent;

		[NonSerialized]
		private bool m_IsSelected;

		#endregion


		#region Private Properties

		private MotionKitComponent MotionKitComponent {
			get {
				if(m_MotionKitComponent == null) {
					m_MotionKitComponent = GetComponent<MotionKitComponent>();
				}
				return m_MotionKitComponent;
			}
		}

		private bool IsSelected {
			get => m_IsSelected;
			set { 
				m_IsSelected = value;
				MotionKitComponent.Play(m_IsSelected ? "SelectedPlayback" : "UnselectedPlayback");
			}
		}

		#endregion


	}

}