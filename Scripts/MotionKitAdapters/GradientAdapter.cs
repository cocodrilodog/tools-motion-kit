namespace CocodriloDog.MotionKit {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// This sets the color of a <see cref="CocodriloDog.MotionKit.ColorAdapter"/> or an
	/// <see cref="UnityEngine.UI.Image"/>
	/// according the the <see cref="m_Gradient"/> and its corresponding <see cref="Value"/>
	/// from 0 to 1.
	/// </summary>
	public class GradientAdapter : MonoBehaviour {


		#region Public Methods

		public void ApplyValue() {
			if (ColorAdapter != null) {
				ColorAdapter.Color = m_Gradient.Evaluate(Mathf.Repeat(m_Value, 1));
			}
			if (Image != null) {
				Image.color = m_Gradient.Evaluate(Mathf.Repeat(m_Value, 1));
			}
		} 

		#endregion

		#region Public Properties

		public float Value {
			get => m_Value;
			set {
				m_Value = value;
				ApplyValue();
			}
		}

		#endregion


		#region Unity Methods

		/// <summary>
		/// Use this if the <see cref="ColorAdapter"/> is attached later.
		/// </summary>
		public void Reset() {
			m_MissingColorAdapter = false;
			m_MissingImage = false;
		}

		#endregion


		#region Private Fields - Seerialized

		[SerializeField]
		private Gradient m_Gradient;

		[Range(0, 1)]
		[SerializeField]
		private float m_Value;

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private ColorAdapter m_ColorAdapter;

		/// <summary>
		/// Use this to have only one search.
		/// </summary>
		[NonSerialized]
		private bool m_MissingColorAdapter;

		[NonSerialized]
		private Image m_Image;

		/// <summary>
		/// Use this to have only one search.
		/// </summary>
		[NonSerialized]
		private bool m_MissingImage;

		#endregion

		#region Private Properties

		private ColorAdapter ColorAdapter {
			get {
				if (m_ColorAdapter == null && !m_MissingColorAdapter) {
					m_ColorAdapter = GetComponent<ColorAdapter>();
					if (m_ColorAdapter == null) {
						m_MissingColorAdapter = true;
					}
				}
				return m_ColorAdapter;
			}
		}

		private Image Image {
			get {
				if (m_Image == null && !m_MissingImage) {
					m_Image = GetComponent<Image>();
					if (m_Image == null) {
						m_MissingImage = true;
					}
				}
				return m_Image;
			}
		}

		#endregion


	}
}