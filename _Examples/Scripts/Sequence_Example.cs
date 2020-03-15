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
					.SetDuration(2).SetEasing(AnimateEasing.ElasticOut)
					.SetOnUpdate(() => Debug.LogFormat("Animation #1 Update"))
					.SetOnComplete(() => Debug.LogFormat("Animation #1 Complete")),

				Animate.GetMotion(c => ColorSphere.Color = c)
					.SetInitialValue(Color.red).SetFinalValue(Color.black)
					.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
					.SetOnUpdate(() => Debug.LogFormat("Animation #2 Update"))
					.SetOnComplete(() => Debug.LogFormat("Animation #2 Complete")),

				Animate.GetMotion(p => PositionCube.transform.position = p)
					.SetInitialValue(new Vector3(-2, -2, 0)).SetFinalValue(new Vector3(-2, 2, 0))
					.SetDuration(2).SetEasing(AnimateEasing.ElasticOut)
					.SetOnUpdate(() => Debug.LogFormat("Animation #3 Update"))
					.SetOnComplete(() => Debug.LogFormat("Animation #3 Complete")),

				Animate.GetMotion(c => ColorSphere.Color = c)
					.SetInitialValue(Color.green).SetFinalValue(Color.black)
					.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
					.SetOnUpdate(() => Debug.LogFormat("Animation #4 Update"))
					.SetOnComplete(() => Debug.LogFormat("Animation #4 Complete")),

				Animate.GetMotion(p => PositionCube.transform.position = p)
					.SetInitialValue(new Vector3(-2, 2, 0)).SetFinalValue(new Vector3(2, 2, 0))
					.SetDuration(2).SetEasing(AnimateEasing.ElasticOut)
					.SetOnUpdate(() => Debug.LogFormat("Animation #5 Update"))
					.SetOnComplete(() => Debug.LogFormat("Animation #5 Complete")),

				Animate.GetMotion(c => ColorSphere.Color = c)
					.SetInitialValue(Color.blue).SetFinalValue(Color.black)
					.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
					.SetOnUpdate(() => Debug.LogFormat("Animation #6 Update"))
					.SetOnComplete(() => Debug.LogFormat("Animation #6 Complete")),

				Animate.GetMotion(p => PositionCube.transform.position = p)
					.SetInitialValue(new Vector3(2, 2, 0)).SetFinalValue(new Vector3(2, -2, 0))
					.SetDuration(2).SetEasing(AnimateEasing.ElasticOut)
					.SetOnUpdate(() => Debug.LogFormat("Animation #7 Update"))
					.SetOnComplete(() => Debug.LogFormat("Animation #7 Complete")),

				Animate.GetMotion(c => ColorSphere.Color = c)
					.SetInitialValue(Color.white).SetFinalValue(Color.black)
					.SetDuration(1).SetEasing(new Blink(2).ColorEasing)
					.SetOnUpdate(() => Debug.LogFormat("Animation #8 Update"))
					.SetOnComplete(() => Debug.LogFormat("Animation #8 Complete"))

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