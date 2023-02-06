namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// This is the component that will be used to own and manage <see cref="AnimateBlock"/>s.
	/// </summary>
	/// <remarks>
	/// Its interface has been designed for ease of use by <c>UnityEvents</c>.
	/// </remarks>
	public class AnimateComponent : CompositeRoot {


		#region Public Properties

		/// <summary>
		/// The first <see cref="AnimateBlock"/> of this <see cref="AnimateBehaviour"/> if any, or <c>null</c>.
		/// </summary>
		public AnimateBlock DefaultAnimateBlock => AnimateBlocks.Count > 0 ? AnimateBlocks[0] : null;

		#endregion


		#region Public Methods

		/// <summary>
		/// Plays the <see cref="DefaultAnimateBlock"/>.
		/// </summary>
		public void Play() => DefaultAnimateBlock?.Play();

		/// <summary>
		/// Plays the <see cref="AnimateBlock"/> with the specified <paramref name="blockName"/>.
		/// </summary>
		/// <param name="blockName">The name of the block.</param>
		public void Play(string blockName) => GetAnimateBlock(blockName)?.Play();

		/// <summary>
		/// Plays all the <see cref="AnimateBlock"/>s managed by this behaviour.
		/// </summary>
		public void PlayAll() => AnimateBlocks.ForEach(b => b?.Play());

		/// <summary>
		/// Stops the <see cref="DefaultAnimateBlock"/>.
		/// </summary>
		public void Stop() => DefaultAnimateBlock?.Stop();

		/// <summary>
		/// Stops the <see cref="AnimateBlock"/> with the specified <paramref name="blockName"/>.
		/// </summary>
		/// <param name="blockName">The name of the block.</param>
		public void Stop(string blockName) => GetAnimateBlock(blockName)?.Stop();

		/// <summary>
		/// Stops all the <see cref="AnimateBlock"/>s managed by this behaviour.
		/// </summary>
		public void StopAll() => AnimateBlocks.ForEach(b => b?.Stop());

		/// <summary>
		/// Pauses the <see cref="DefaultAnimateBlock"/>.
		/// </summary>
		public void Pause() => DefaultAnimateBlock?.Pause();

		/// <summary>
		/// Pauses the <see cref="AnimateBlock"/> with the specified <paramref name="blockName"/>.
		/// </summary>
		/// <param name="blockName">The name of the block.</param>
		public void Pause(string blockName) => GetAnimateBlock(blockName)?.Pause();


		/// <summary>
		/// Pauses all the <see cref="AnimateBlock"/>s managed by this behaviour.
		/// </summary>
		public void PauseAll() => AnimateBlocks.ForEach(b => b?.Pause());

		/// <summary>
		/// Resumes the <see cref="DefaultAnimateBlock"/>.
		/// </summary>
		public void Resume() => DefaultAnimateBlock?.Resume();

		/// <summary>
		/// Resumes the <see cref="AnimateBlock"/> with the specified <paramref name="blockName"/>.
		/// </summary>
		/// <param name="blockName">The name of the block.</param>
		public void Resume(string blockName) => GetAnimateBlock(blockName)?.Resume();

		/// <summary>
		/// Resumes all the <see cref="AnimateBlock"/>s managed by this behaviour.
		/// </summary>
		public void ResumeAll() => AnimateBlocks.ForEach(b => b?.Resume());


		/// <summary>
		/// Resets the default motion.
		/// </summary>
		public void ResetMotion() => (DefaultAnimateBlock as IMotionBlock)?.ResetMotion();

		/// <summary>
		/// Resets the <see cref="AnimateBlock"/> with the specified <paramref name="motionPath"/>
		/// if it is a motion.
		/// </summary>
		/// <param name="motionPath">The path of the block.</param>
		public void ResetMotion(string motionPath) { // TODO: Allow paths like: "Sequence/Motion1"
			(GetAnimateBlock(motionPath) as IMotionBlock)?.ResetMotion();
		}

		/// <summary>
		/// Calls <see cref="IMotionBlock.ResetMotion()"/> in all the motions that it finds.
		/// </summary>
		public void ResetAllMotions() => AnimateBlocks.ForEach(b => _ResetMotion(b));

		/// <summary>
		/// Disposes all the <see cref="AnimateBlock"/>s of this compopnent.
		/// </summary>
		public void Dispose() => AnimateBlocks.ForEach(b => b?.Dispose());

		/// <summary>
		/// Gets the <see cref="AnimateBlock"/> named <paramref name="name"/>.
		/// </summary>
		/// <remarks>
		/// This method can be used for further control over the managed <see cref="AnimateBlock"/>s.
		/// </remarks>
		/// <param name="name">The <see cref="AnimateBlock.Name"/></param>
		/// <returns>The <see cref="AnimateBlock"/></returns>
		public AnimateBlock GetAnimateBlock(string name) => AnimateBlocks.FirstOrDefault(b => b != null && b.Name == name);

		#endregion


		#region Unity Methods

		private void Start() {
			if (PlayAllOnStart) {
				PlayAll();
			} else {
				// This avoids errors OnDestroy in case it was not played at all.
				Initialize();
			}
		}

		private void OnDestroy() {
			Dispose();
		}

		#endregion


		#region Private Fields

		[SerializeReference]
		private List<AnimateBlock> m_AnimateBlocks = new List<AnimateBlock>();

		[SerializeField]
		private bool m_PlayAllOnStart;

		#endregion


		#region Private Properties		

		private List<AnimateBlock> AnimateBlocks => m_AnimateBlocks;

		private bool PlayAllOnStart => m_PlayAllOnStart;

		#endregion


		#region Private Methods

		/// <summary>
		/// Initializes all the <see cref="AnimateBlock"/>s of this compopnent.
		/// </summary>
		private void Initialize() => AnimateBlocks.ForEach(bf => bf?.Initialize());

		private void _ResetMotion(AnimateBlock animateBlock) {
			if (animateBlock == null) {
				return;
			}
			if (animateBlock is IMotionBlock) {
				var motionBlock = animateBlock as IMotionBlock;
				motionBlock.ResetMotion();
			}
			//if (animateBlock is SequenceBlock) {
			//	var sequenceAsset = animateBlock as SequenceBlock;
			//	foreach (var itemField in sequenceAsset.SequenceItemsFields) {
			//		// Recursion
			//		_ResetMotion(itemField.Object);
			//	}
			//}
			//if (animateBlock is ParallelBlock) {
			//	var parallelAsset = animateBlock as ParallelBlock;
			//	foreach (var itemField in parallelAsset.ParallelItemsFields) {
			//		// Recursion
			//		_ResetMotion(itemField.Object);
			//	}
			//}
		}

		#endregion


	}

}