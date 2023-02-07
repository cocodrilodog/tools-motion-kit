namespace CocodriloDog.Animation.Examples {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AnimateMotionBlocks_Example : MonoBehaviour {


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
				// The MaterialPropertyBlock approach is not working
				//
				//var propertyBlock = new MaterialPropertyBlock();
				//Renderer.GetPropertyBlock(propertyBlock);
				//return propertyBlock.GetColor("_BaseColor");
				return Renderer.material.GetColor("_BaseColor");
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

		public Vector3 GetVector3() => transform.position;

		// This is used to test that there will no be ambiguity when searching for the method
		public Vector3 GetVector3(string param) => transform.position;

		public void SetVector2(Vector2 value) => transform.position = value;

		public Vector2 GetVector2() => transform.position;

		public void SetFloat(float value) => Float = value;

		public float GetFloat() => Float;

		public void SetColor(Color value) => Color = value;

		public Color GetColor() => Color;

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
				if (m_Renderer == null) {
					m_Renderer = GetComponent<Renderer>();
				}
				return m_Renderer;
			}
		}

		#endregion


	}

}