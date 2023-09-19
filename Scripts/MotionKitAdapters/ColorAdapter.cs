namespace CocodriloDog.MotionKit {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// An object to change the color and alpha of a game object which material
	/// has a <c>"_Color"</c> property.
	/// </summary>
	public class ColorAdapter : MaterialAdapter {


		#region Public Properties

		public Color Color {
			get { return m_Color; }
			set {
				m_Color = value;
				ApplyColor();
			}
		}

		public float Alpha {
			get { return m_Alpha; }
			set {
				m_Alpha = value;
				ApplyColor();
			}
		}

		#endregion


		#region MonoBehaviour Methods

		private void OnEnable() {
			ApplyColor();
		}

		#endregion


		#region Private Fields

		[SerializeField]
		public string m_ColorPropertyName = "_BaseColor";

		[SerializeField]
		private Color m_Color = Color.white;

		[Range(0, 1)]
		[SerializeField]
		private float m_Alpha = 1;

		#endregion


		#region Internal Methods

		public void ApplyColor() {
			m_Color.a = Alpha;
			MaterialPropertyBlock.SetColor(m_ColorPropertyName, m_Color);
			Renderer.SetPropertyBlock(MaterialPropertyBlock);
		}

		#endregion


	}

}