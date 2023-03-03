namespace CocodriloDog.Animation.Examples {

	using CocodriloDog.Rendering;
	using CocodriloDog.Utility;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.EventSystems;

	public class Shake_Example : MonoBehaviour {


		#region Public Fields

		[Header("Values")]

		[SerializeField]
		public Shake Shake;

		#endregion


		#region Unity Methods

		private void OnEnable() {
			EventSystemUtility.AddTriggerListener(
				Vector3Cube,
				EventTriggerType.PointerClick,
				Vector3Cube_PointerClick
			);
			EventSystemUtility.AddTriggerListener(
				FloatCube,
				EventTriggerType.PointerClick,
				FloatCube_PointerClick
			);
			EventSystemUtility.AddTriggerListener(
				ColorCube,
				EventTriggerType.PointerClick,
				ColorCube_PointerClick
			);
		}

		private void OnDisable() {
			EventSystemUtility.RemoveTriggerListener(Vector3Cube, Vector3Cube_PointerClick);
			EventSystemUtility.RemoveTriggerListener(FloatCube, FloatCube_PointerClick);
			EventSystemUtility.RemoveTriggerListener(ColorCube, ColorCube_PointerClick);
		}

		#endregion


		#region Event Handlers

		private void Vector3Cube_PointerClick(BaseEventData eventData) {
			MotionKit.GetMotion(this, "Vector3CubePosition", p => Vector3Cube.transform.position = p)
				.SetEasing(Shake.Vector3Easing)
				.Play(Vector3Cube.transform.position, Vector3.up, 1);
		}

		private void FloatCube_PointerClick(BaseEventData eventData) {

			// Shake X
			MotionKit.GetMotion(this, "FloatCubeX", v => {
				Vector3 position = FloatCube.transform.position;
				position.x = v;
				FloatCube.transform.position = position;
			}).SetEasing(Shake.FloatEasing)
			.Play(FloatCube.transform.position.x, -3, 1);

			// Shake Y
			MotionKit.GetMotion(this, "FloatCubeY", v => {
				Vector3 position = FloatCube.transform.position;
				position.y = v;
				FloatCube.transform.position = position;
			}).SetEasing(Shake.FloatEasing)
			.Play(FloatCube.transform.position.y, 1, 1);

		}

		private void ColorCube_PointerClick(BaseEventData eventData) {
			MotionKit.GetMotion(this, "ColorCubeColor", c => ColorCube.GetComponent<ColorModifier>().Color = c)
				.SetEasing(Shake.ColorEasing)
				.Play(ColorCube.GetComponent<ColorModifier>().Color, Color.red, 1);
		}

		#endregion


		#region Private Fields

		[Header("Subcomponents")]

		[SerializeField]
		private EventTrigger m_FloatCube;

		[SerializeField]
		private EventTrigger m_Vector3Cube;

		[SerializeField]
		private EventTrigger m_ColorCube;

		#endregion


		#region Private Properties

		private EventTrigger FloatCube { get { return m_FloatCube; } }

		private EventTrigger Vector3Cube { get { return m_Vector3Cube; } }

		private EventTrigger ColorCube { get { return m_ColorCube; } }

		#endregion

	}

}