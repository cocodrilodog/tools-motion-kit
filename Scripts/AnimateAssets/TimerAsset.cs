namespace CocodriloDog.Animation {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class TimerAsset : AnimateAsset {


		#region Public Fields

		[SerializeField]
		public string Timer;

		public override float Progress => throw new System.NotImplementedException();

		public override float CurrentTime => throw new System.NotImplementedException();

		public override float Duration => throw new System.NotImplementedException();

		public override bool IsPlaying => throw new System.NotImplementedException();

		public override bool IsPaused => throw new System.NotImplementedException();

		public override void Init() {
			throw new System.NotImplementedException();
		}

		public override void Pause() {
			throw new System.NotImplementedException();
		}

		public override void Play() {
			throw new System.NotImplementedException();
		}

		public override void Resume() {
			throw new System.NotImplementedException();
		}

		public override void Stop() {
			throw new System.NotImplementedException();
		}

		#endregion


	}

}