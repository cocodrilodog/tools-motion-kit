namespace CocodriloDog.Animation.Examples {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AnimateAssets_Example : MonoBehaviour {


		#region Public Properties

		public Vector3 Vector3 { 
			get => transform.position; 
			set => transform.position = value; 
		}

		public Vector2 Vector2 {
			get => transform.position;
			set => transform.position = value;
		}

		public float Float {
			get => transform.position.y;
			set {
				var pos = transform.position;
				pos.y = value;
				transform.position = pos;
			}
		}

		public Color Color {
			get {
				var propertyBlock = new MaterialPropertyBlock();
				Renderer.GetPropertyBlock(propertyBlock);
				return propertyBlock.GetColor("_BaseColor");
			}
			set {
				var propertyBlock = new MaterialPropertyBlock();
				propertyBlock.SetColor("_BaseColor", value);
				Renderer.SetPropertyBlock(propertyBlock);
			}
		}

		#endregion
		

		#region Public Methods

		public void SetVector3(Vector3 value) => transform.position = value;

		public void SetVector2(Vector2 value) => transform.position = value;

		public void SetFloat(float value) => Float = value;

		public void SetColor(Color value) => Color = value;

		public void OnStart() => Debug.Log($"{name}: OnStart");

		public void OnUpdate() => Debug.Log($"{name}: OnUpdate");
		
		public void OnInterrupt() => Debug.Log($"{name}: OnInterrupt");
		
		public void OnComplete() => Debug.Log($"{name}: OnComplete");

		#endregion


		#region Private Fields

		[NonSerialized]
		private Renderer m_Renderer;

		#endregion


		#region Private Properties

		private Renderer Renderer {
			get {
				if(m_Renderer == null) {
					m_Renderer = GetComponent<Renderer>();
				}
				return m_Renderer;
			}
		}

		#endregion


	}

}