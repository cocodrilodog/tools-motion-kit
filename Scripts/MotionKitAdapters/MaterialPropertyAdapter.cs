namespace CocodriloDog.MotionKit {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;


	#region Small Types

	public interface IApplicableValue {
		void ApplyValue();
	}

	#endregion


	/// <summary>
	/// Base class that allows to set a property on a material via <see cref="MaterialPropertyBlock"/>
	/// on the renderer, and updates it in edit mode.
	/// </summary>
	/// 
	/// <typeparam name="T">The type of property to set.</typeparam>
	/// 
	/// <remarks>
	/// This is easier to derive subclasses from, as compared to <see cref="MaterialAdapter"/> because it 
	/// does no require additional editor code.
	/// </remarks>
	public abstract class MaterialPropertyAdapter<T> : MonoBehaviour, IApplicableValue {


		#region Public Fields

		/// <summary>
		/// The value of the property
		/// </summary>
		public T Value {
			get => m_Value;
			set {
				m_Value = value;
				ApplyValue();
			}
		}

		#endregion


		#region Public Methods

		public void ApplyValue() {
			SetPropertyValue(MaterialPB, m_Property, m_Value);
			Renderer.SetPropertyBlock(MaterialPB);
		}

		#endregion


		#region Protected Methods

		/// <summary>
		/// Override this to set the property on the <see cref="MaterialPropertyBlock"/>.
		/// </summary>
		/// <param name="materialPB">The material property block.</param>
		/// <param name="property">The property name.</param>
		/// <param name="value">The value.</param>
		protected abstract void SetPropertyValue(MaterialPropertyBlock materialPB, string property, T value);

		#endregion


		#region Pritave Fields - Serialized

		[SerializeField]
		private string m_Property;

		[SerializeField]
		private T m_Value;

		#endregion


		#region Private Fields - Non Serialized

		private Renderer m_Renderer;

		private MaterialPropertyBlock m_MaterialPB;

		#endregion


		#region Private Properties

		private Renderer Renderer {
			get {
				if (m_Renderer == null) {
					m_Renderer = GetComponent<Renderer>();
				}
				return m_Renderer;
			}
		}

		private MaterialPropertyBlock MaterialPB => m_MaterialPB = m_MaterialPB ?? new MaterialPropertyBlock();

		#endregion


	}

}
