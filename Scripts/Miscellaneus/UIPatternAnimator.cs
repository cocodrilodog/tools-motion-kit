namespace CocodriloDog.Animation {

	using CocodriloDog.Animation;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Animates a tiled image on a <see cref="Image"/> component.
	/// </summary>
	[RequireComponent(typeof(Image))]
	public class UIPatternAnimator : MonoBehaviour {


		#region Public Fields

		[SerializeField]
		public Vector2 Speed = new Vector2(20, 0);

		[SerializeField]
		public bool AnimateOnStart = true;

		#endregion


		#region Public Methods

		public void StartAnimation() {
			StopAnimation();
			m_IsAnimating = true;
			m_AnimationCoroutine = StartCoroutine(_Animation());
		}

		public void StopAnimation() {
			m_IsAnimating = false;
			if (m_AnimationCoroutine != null) {
				StopCoroutine(m_AnimationCoroutine);
			}
		}

		#endregion


		#region Unity Methods

		private void Awake() {
			m_DefaultOffsetMin = RectTransform.offsetMin;
		}

		private void OnEnable() {
			if(m_IsAnimating) {
				StartAnimation();
			}
		}

		private void Start() {
			Pattern.type = Image.Type.Tiled;
			if (AnimateOnStart) {
				StartAnimation();
			}
		}

		#endregion


		#region Private Constants

		private const string OffsetMinKey = "OffsetMin";

		#endregion


		#region Private Fields

		[NonSerialized]
		private Image m_Pattern;

		[NonSerialized]
		private RectTransform m_RectTransform;

		[NonSerialized]
		private Vector2 m_DefaultOffsetMin;

		[NonSerialized]
		private Coroutine m_AnimationCoroutine;

		[NonSerialized]
		private bool m_IsAnimating;

		#endregion


		#region Private Properties

		private Image Pattern {
			get {
				if(m_Pattern == null) {
					m_Pattern = GetComponent<Image>();
				}
				return m_Pattern;
			}
		}

		private RectTransform RectTransform {
			get {
				if (m_RectTransform == null) {
					m_RectTransform = GetComponent<RectTransform>();
				}
				return m_RectTransform;
			}
		}

		#endregion


		#region Private Methods

		private IEnumerator _Animation() {
			while (true) {

				RectTransform.offsetMin += Speed * Time.deltaTime;

				float patternWidth = Pattern.sprite.texture.width;
				float patternHeight = Pattern.sprite.texture.height;

				if (Speed.x > 0) {
					if (RectTransform.offsetMin.x > m_DefaultOffsetMin.x) {
						Vector2 offsetMin = RectTransform.offsetMin;
						offsetMin.x -= patternWidth;
						RectTransform.offsetMin = offsetMin;
					}
				}

				if (Speed.x < 0) {
					if (RectTransform.offsetMin.x < m_DefaultOffsetMin.x - patternWidth) {
						Vector2 offsetMin = RectTransform.offsetMin;
						offsetMin.x += patternWidth;
						RectTransform.offsetMin = offsetMin;
					}
				}

				if (Speed.y > 0) {
					if (RectTransform.offsetMin.y > m_DefaultOffsetMin.y) {
						Vector2 offsetMin = RectTransform.offsetMin;
						offsetMin.y -= patternHeight;
						RectTransform.offsetMin = offsetMin;
					}
				}

				if (Speed.y < 0) {
					if (RectTransform.offsetMin.y < m_DefaultOffsetMin.y - patternHeight) {
						Vector2 offsetMin = RectTransform.offsetMin;
						offsetMin.y += patternHeight;
						RectTransform.offsetMin = offsetMin;
					}
				}

				yield return null;

			}
		}

		#endregion


	}

}