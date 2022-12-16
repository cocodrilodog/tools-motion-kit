namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AnimateAsset : MonoScriptableObject {


		#region Public Properties

		public virtual float Progress => 0;

		public virtual float CurrentTime => 0;

		public virtual float Duration => 0;

		public virtual bool IsPlaying => false;

		public virtual bool IsPlaused => false;

		#endregion


		#region Public Methods

		public virtual void Play() { }

		public virtual void Stop() { }

		public virtual void Pause() { }

		public virtual void Resume() { }

		public virtual void Dispose() {
			Animate.ClearPlaybacks(this);
		}

		#endregion


	}

}