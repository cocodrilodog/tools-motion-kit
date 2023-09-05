namespace CocodriloDog.Animation {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class MaterialAdapter : MonoBehaviour {


		#region Public Properties

		public Renderer Renderer {
			get {
				if (m_Renderer == null) {
					m_Renderer = GetComponent<Renderer>();
				}
				return m_Renderer;
			}
		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private Renderer m_Renderer;

		[NonSerialized]
		private MaterialPropertyBlock m_MaterialPropertyBlock;

		#endregion


		#region Private Properties

		protected MaterialPropertyBlock MaterialPropertyBlock => 
			m_MaterialPropertyBlock = m_MaterialPropertyBlock ?? new MaterialPropertyBlock();

		#endregion


	}
}