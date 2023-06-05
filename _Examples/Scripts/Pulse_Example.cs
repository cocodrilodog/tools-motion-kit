namespace CocodriloDog.Animation {

	using CocodriloDog.Rendering;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

	[AddComponentMenu("")]
	public class Pulse_Example : MonoBehaviour {


		#region Public Fields

		[SerializeField]
        public Pulse Pulse;

		#endregion


		#region Public Methods

		public void PulseY() {

			float currentY = YCube.localPosition.y;

			MotionKit.GetMotion(this, "YMotion", y => {
				Vector3 position = YCube.localPosition;
				position.y = y;
				YCube.localPosition = position;
			}).SetEasing(Pulse.FloatEasing).Play(currentY, currentY, 0.5f);

		}

		public void PulseScale() {
			MotionKit.GetMotion(this, "ScaleMotion", s => ScaleCube.localScale = s)
				.SetEasing(Pulse.Vector3Easing).Play(Vector3.one, Vector3.one, 0.5f);
        }

		public void PulseColor() {

			Color currentColor = ColorCube.Color;

			MotionKit.GetMotion(this, "ColorMotion", c => ColorCube.Color = c)
				.SetEasing(Pulse.ColorEasing).Play(currentColor, currentColor, 0.5f);

		}

		#endregion


		#region Unity Methods

		private void OnDestroy() {
			MotionKit.ClearPlaybacks(this);
		}

		#endregion


		#region Private Fields

		[SerializeField]
		private Transform m_YCube;

		[SerializeField]
		private Transform m_ScaleCube;

		[SerializeField]
		private ColorModifier m_ColorCube;

		#endregion


		#region Private Properties

		private Transform YCube => m_YCube;

		private Transform ScaleCube => m_ScaleCube;

		private ColorModifier ColorCube => m_ColorCube;

		#endregion


	}

}