namespace CocodriloDog.MotionKit.Examples {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.EventSystems;

	[AddComponentMenu("")]
	public class Shake_Example : MonoBehaviour {


		#region Public Fields

		[Header("Values")]

		[SerializeField]
		public Shake Shake;

		#endregion


		#region Unity Methods

		private void OnDestroy() {
			foreach (var cube in m_Cubes) {
				MotionKit.ClearPlaybacks(cube);	
			}
		}

		#endregion


		#region Event Handlers

		public void FloatCube_PointerClick(Transform cube) {

			if (!m_Cubes.Contains(cube)) {
				m_Cubes.Add(cube);
			}

			var rotY = cube.localEulerAngles.y;
			MotionKit.GetMotion(cube, "Rotation", r => {
				var rotation = Quaternion.AngleAxis(r, Vector3.up);
				cube.localRotation = rotation;
			}).SetEasing(Shake.FloatEasing).Play(rotY, rotY, 1);

		}

		public void Vector3Cube_PointerClick(Transform cube) {

			if (!m_Cubes.Contains(cube)) {
				m_Cubes.Add(cube);
			}

			var pos = cube.transform.position;
			MotionKit.GetMotion(cube, "Position", p => cube.position = p)
				.SetEasing(Shake.Vector3Easing).Play(pos, pos, 1);

		}

		public void VectorXCube_PointerClick(Transform cube) {

			if (!m_Cubes.Contains(cube)) {
				m_Cubes.Add(cube);
			}

			Shake shakeX = Shake.Copy() as Shake;
			// zero out y and z so that only the magnitude in x is applied
			shakeX.Magnitude.Vector3.y = 0;
			shakeX.Magnitude.Vector3.z = 0;

			var pos = cube.transform.position;

			MotionKit.GetMotion(cube, "Position", p => cube.position = p)
				.SetEasing(shakeX.Vector3Easing).Play(pos, pos, 1);

		}

		public void ColorCube_PointerClick(Transform cube) {

			if (!m_Cubes.Contains(cube)) {
				m_Cubes.Add(cube);
			}

			var colorModifier = cube.GetComponent<ColorAdapter>();
			var color = colorModifier.Color;

			MotionKit.GetMotion(cube, "Color", c => colorModifier.Color = c)
				.SetEasing(Shake.ColorEasing).Play(color, color, 1);

		}

		#endregion


		#region Private Fields

		[NonSerialized]
		private List<Transform> m_Cubes = new List<Transform>();

		#endregion


	}

}