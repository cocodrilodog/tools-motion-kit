namespace CocodriloDog.Animation.Examples {

	using CocodriloDog.Rendering;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Sequence_Example : MonoBehaviour {


		#region Unity Methods

		private void Start() {

			Sequence sequence = Animate.GetSequence(

				this, "Sequence",

				Animate.GetMotion(p => PositionCube.transform.position = p)
					.SetInitialValue(new Vector3(2, -2, 0)).SetFinalValue(new Vector3(-2, -2, 0))
					.SetDuration(2).SetEasing(AnimateEasing.ElasticOut),

				Animate.GetMotion(c => ColorSphere.Color = c)
					.SetInitialValue(Color.red).SetFinalValue(Color.black)
					.SetDuration(1).SetEasing(new Blink(2).ColorEasing),

				Animate.GetMotion(p => PositionCube.transform.position = p)
					.SetInitialValue(new Vector3(-2, -2, 0)).SetFinalValue(new Vector3(-2, 2, 0))
					.SetDuration(2).SetEasing(AnimateEasing.ElasticOut),

				Animate.GetMotion(c => ColorSphere.Color = c)
					.SetInitialValue(Color.green).SetFinalValue(Color.black)
					.SetDuration(1).SetEasing(new Blink(2).ColorEasing),

				Animate.GetMotion(p => PositionCube.transform.position = p)
					.SetInitialValue(new Vector3(-2, 2, 0)).SetFinalValue(new Vector3(2, 2, 0))
					.SetDuration(2).SetEasing(AnimateEasing.ElasticOut),

				Animate.GetMotion(c => ColorSphere.Color = c)
					.SetInitialValue(Color.blue).SetFinalValue(Color.black)
					.SetDuration(1).SetEasing(new Blink(2).ColorEasing),

				Animate.GetMotion(p => PositionCube.transform.position = p)
					.SetInitialValue(new Vector3(2, 2, 0)).SetFinalValue(new Vector3(2, -2, 0))
					.SetDuration(2).SetEasing(AnimateEasing.ElasticOut),

				Animate.GetMotion(c => ColorSphere.Color = c)
					.SetInitialValue(Color.white).SetFinalValue(Color.black)
					.SetDuration(1).SetEasing(new Blink(2).ColorEasing)

			).Play();

			Debug.LogFormat("Sequence: {0}", sequence);

		}

		#endregion


		#region Private Fields

		[SerializeField]
		private Transform m_PositionCube;

		[SerializeField]
		private ColorModifier m_ColorSphere;

		#endregion


		#region Private Properties

		[SerializeField]
		private Transform PositionCube { get { return m_PositionCube; } }

		[SerializeField]
		private ColorModifier ColorSphere { get { return m_ColorSphere; } }

		#endregion


	}

}