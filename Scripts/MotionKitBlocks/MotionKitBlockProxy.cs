namespace CocodriloDog.MotionKit {
	
	using CocodriloDog.Core;
	using UnityEngine;

	/// <summary>
	/// A component that allows to access the selected <see cref="MotionKitBlock"/> API.
	/// </summary>
	/// 
	/// <remarks>
	/// This was created to facilitate Unity events playing <see cref="MotionKitBlock"/>s without
	/// relying on the name of the block.
	/// </remarks>
	public class MotionKitBlockProxy : MonoBehaviour, IMotionKitBlock {


		#region Public Properties

		public bool IsInitialized => (m_Block.Value?.IsInitialized).Value;

		public ITimedProgressable TimedProgressable => m_Block.Value?.TimedProgressable;

		public float Progress { 
			get => (m_Block.Value?.Progress).Value;
			set {
				if (m_Block.Value != null) {
					m_Block.Value.Progress = value;
				}
			}
		}

		public float CurrentTime => (m_Block.Value?.CurrentTime).Value;

		public float Duration => (m_Block.Value?.Duration).Value;

		public bool IsPlaying => (m_Block.Value?.IsPlaying).Value;

		public bool IsPaused => m_Block.Value.IsPaused;

		#endregion


		#region Public Methods

		public void Dispose() => m_Block.Value?.Dispose();

		public void ForceResetPlayback() => m_Block.Value?.ForceResetPlayback();

		public void Initialize() => m_Block.Value?.Initialize();

		public void LockResetPlayback(bool recursive) => m_Block.Value?.LockResetPlayback(recursive);

		public void Pause() => m_Block.Value?.Pause();

		public void Play() => m_Block.Value?.Play();

		public void Resume() => m_Block.Value?.Resume();

		public void Stop() => m_Block.Value?.Stop();

		public void TryResetPlayback(bool recursive) => m_Block.Value?.TryResetPlayback(recursive);

		public void UnlockResetPlayback(bool recursive) => m_Block.Value?.UnlockResetPlayback(recursive);

		#endregion


		#region Private Fields

		[Tooltip("The block to communicate with.")]
		[SerializeField]
		private CompositeObjectReference<IMotionKitBlock> m_Block;

		#endregion


	}

}