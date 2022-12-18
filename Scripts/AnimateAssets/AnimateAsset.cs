namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public abstract class AnimateAsset : MonoScriptableObject {


		#region Public Properties

		public abstract float Progress { get; }

		public abstract float CurrentTime { get; }

		public abstract float Duration { get; }

		public abstract bool IsPlaying { get; }

		public abstract bool IsPaused { get; }

		#endregion


		#region Public Methods

		public abstract void Init();

		public abstract void Play();

		public abstract void Stop();

		public abstract void Pause();

		public abstract void Resume();

		public virtual void Dispose() {
			Animate.ClearPlaybacks(this);
		}

		#endregion


	}

}