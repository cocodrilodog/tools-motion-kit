namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	public interface IAnimateParent {

		/// <summary>
		/// Gets the <see cref="AnimateBlock"/> named <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The <see cref="AnimateBlock.Name"/></param>
		/// <returns>The <see cref="AnimateBlock"/></returns>
		AnimateBlock GetChildBlock(string name);

		/// <summary>
		/// Gets all the children <see cref="AnimateBlock"/>.
		/// </summary>
		/// <returns>An array with the children.</returns>
		AnimateBlock[] GetChildrenBlocks();

	}

	/// <summary>
	/// This is the component that will be used to own and manage <see cref="AnimateBlock"/>s.
	/// </summary>
	/// <remarks>
	/// Its interface has been designed for ease of use by <c>UnityEvents</c>.
	/// </remarks>
	public class AnimateComponent : CompositeRoot, IAnimateParent {


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
		/// Plays the <see cref="AnimateBlock"/> with the specified <paramref name="blockPath"/>.
		/// </summary>
		/// <param name="blockPath">The name of the block.</param>
		public void Play(string blockPath) => GetChildBlockAtPath(blockPath)?.Play();

		/// <summary>
		/// Plays all the <see cref="AnimateBlock"/>s managed by this behaviour.
		/// </summary>
		public void PlayAll() => AnimateBlocks.ForEach(b => b?.Play());

		/// <summary>
		/// Stops the <see cref="DefaultAnimateBlock"/>.
		/// </summary>
		public void Stop() => DefaultAnimateBlock?.Stop();

		/// <summary>
		/// Stops the <see cref="AnimateBlock"/> with the specified <paramref name="blockPath"/>.
		/// </summary>
		/// <param name="blockPath">The name of the block.</param>
		public void Stop(string blockPath) => GetChildBlockAtPath(blockPath)?.Stop();

		/// <summary>
		/// Stops all the <see cref="AnimateBlock"/>s managed by this behaviour.
		/// </summary>
		public void StopAll() => AnimateBlocks.ForEach(b => b?.Stop());

		/// <summary>
		/// Pauses the <see cref="DefaultAnimateBlock"/>.
		/// </summary>
		public void Pause() => DefaultAnimateBlock?.Pause();

		/// <summary>
		/// Pauses the <see cref="AnimateBlock"/> with the specified <paramref name="blockPath"/>.
		/// </summary>
		/// <param name="blockPath">The name of the block.</param>
		public void Pause(string blockPath) => GetChildBlockAtPath(blockPath)?.Pause();


		/// <summary>
		/// Pauses all the <see cref="AnimateBlock"/>s managed by this behaviour.
		/// </summary>
		public void PauseAll() => AnimateBlocks.ForEach(b => b?.Pause());

		/// <summary>
		/// Resumes the <see cref="DefaultAnimateBlock"/>.
		/// </summary>
		public void Resume() => DefaultAnimateBlock?.Resume();

		/// <summary>
		/// Resumes the <see cref="AnimateBlock"/> with the specified <paramref name="blockPath"/>.
		/// </summary>
		/// <param name="blockPath">The name of the block.</param>
		public void Resume(string blockPath) => GetChildBlockAtPath(blockPath)?.Resume();

		/// <summary>
		/// Resumes all the <see cref="AnimateBlock"/>s managed by this behaviour.
		/// </summary>
		public void ResumeAll() => AnimateBlocks.ForEach(b => b?.Resume());


		/// <summary>
		/// Resets the <see cref="DefaultAnimateBlock"/> if it is a <see cref="IMotionBlock"/>.
		/// </summary>
		public void ResetMotion() => (DefaultAnimateBlock as IMotionBlock)?.ResetMotion();

		/// <summary>
		/// Resets the <see cref="AnimateBlock"/> with the specified <paramref name="blockPath"/>
		/// if it is a motion.
		/// </summary>
		/// <param name="blockPath">The path of the block. For example "Parallel/Sequence1/Motion2D"</param>
		public void ResetMotion(string blockPath) => (GetChildBlockAtPath(blockPath) as IMotionBlock)?.ResetMotion();

		/// <summary>
		/// Calls <see cref="IMotionBlock.ResetMotion()"/> in all the motions that it in this <see cref="AnimateComponent"/>.
		/// If <paramref name="recursive"/> is <c>true</c>, it looks for children blocks too.
		/// </summary>
		public void ResetAllMotions(bool recursive = false) {
			if (recursive) {
				AnimateBlocks.ForEach(B => ForAllAnimateBlocks(B, b => (b as IMotionBlock)?.ResetMotion()));
			} else {
				AnimateBlocks.ForEach(B => (B as IMotionBlock)?.ResetMotion());
			}
		}

		/// <summary>
		/// Disposes all the <see cref="AnimateBlock"/>s of this compopnent.
		/// </summary>
		public void Dispose() => AnimateBlocks.ForEach(b => b?.Dispose());

		public AnimateBlock GetChildBlock(string name) => AnimateBlocks.FirstOrDefault(b => b != null && b.Name == name);

		/// <summary>
		/// Finds a block at the specified path.
		/// </summary>
		/// <param name="blockPath">The path of the block. For example "Parallel/Sequence1/Motion2D"</param>
		/// <returns>The <see cref="AnimateBlock"/> if it was found</returns>
		public AnimateBlock GetChildBlockAtPath(string blockPath) {

			var pathParts = blockPath.Split('/');

			IAnimateParent parent = this;
			AnimateBlock block = null;

			for (int i = 0; i < pathParts.Length; i++) {
				block = parent.GetChildBlock(pathParts[i]);
				if (block is IAnimateParent) {
					parent = (block as IAnimateParent);
				} else {
					break;
				}
			}
			return block;

		}

		public AnimateBlock[] GetChildrenBlocks() => AnimateBlocks.ToArray();

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

		/// <summary>
		/// Performs an action in all <see cref="AnimateBlock"/>s under <paramref name="animateBlock"/>.
		/// </summary>
		/// <param name="animateBlock">The root <see cref="AnimateBlock"/></param>
		/// <param name="action">The action to perform in it.</param>
		private void ForAllAnimateBlocks(AnimateBlock animateBlock, Action<AnimateBlock> action) {

			if (animateBlock == null) {
				return;
			}

			// Do the action
			action(animateBlock);

			if (animateBlock is IAnimateParent) {
				var animateParent = animateBlock as IAnimateParent;
				foreach (var item in animateParent.GetChildrenBlocks()) {
					// Recursion
					ForAllAnimateBlocks(item, action);
				}
			}

		}

		#endregion


	}

}