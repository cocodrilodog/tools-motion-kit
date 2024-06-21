namespace CocodriloDog.MotionKit {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Old base class way to create adapters that sets properties on materials. Instead is easier
	/// to use <see cref="MaterialPropertyAdapter{T}"/>.
	/// </summary>
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