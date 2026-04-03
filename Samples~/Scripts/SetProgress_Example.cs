namespace CocodriloDog.MotionKit.Examples {

	using CocodriloDog.Core;
	using UnityEngine;

	public class SetProgress_Example : MonoBehaviour {


		#region Public Methods

		public void SetProgress(float progress) {
			Debug.Log($"Progress: {progress} | {name}");
			m_ExampleMotion.Value.Progress = progress;
		}

		#endregion

		#region Private Fields

		[SerializeField]
		private CompositeObjectReference<MotionKitBlock> m_ExampleMotion;

		#endregion


	}

}