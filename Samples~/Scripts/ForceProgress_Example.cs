namespace CocodriloDog.MotionKit.Examples {

	using CocodriloDog.Core;
	using UnityEngine;

	public class ForceProgress_Example : MonoBehaviour {


		#region Public Methods

		public void SetProgress(float progress) {
			Debug.Log($"Progress: {progress} | {name}");
			//m_ExampleMotion.Value.Progress = progress; // This fails in some edge cases
			m_ExampleMotion.Value.ForceProgress(progress);
		}

		#endregion

		#region Private Fields

		[SerializeField]
		private CompositeObjectReference<MotionKitBlock> m_ExampleMotion;

		#endregion


	}

}