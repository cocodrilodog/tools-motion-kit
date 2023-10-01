namespace CocodriloDog.MotionKit {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// This is the component that will be used to own and manage <see cref="MotionKitBlock"/>s.
	/// </summary>
	/// <remarks>
	/// Its interface has been designed for ease of use by <c>UnityEvents</c>.
	/// </remarks>
	public class MotionKitComponent : CompositeRoot, ICompositeParent<MotionKitBlock> {


		#region Public Properties

		/// <summary>
		/// The first <see cref="Blocks"/> item if any, or <c>null</c>.
		/// </summary>
		public MotionKitBlock DefaultBlock {
			get {
				TryInitialize();
				return Blocks.Count > 0 ? Blocks[0] : null;
			}
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Plays the <see cref="DefaultBlock"/>.
		/// </summary>
		public void Play() {
			TryInitialize();
			DefaultBlock?.Play();
		}

		/// <summary>
		/// Plays the <see cref="MotionKitBlock"/> with the specified <paramref name="blockPath"/>.
		/// </summary>
		/// <param name="blockPath">The name of the block.</param>
		public void Play(string blockPath) {
			TryInitialize();
			GetChildAtPath(blockPath)?.Play();
		}

		/// <summary>
		/// Plays all the <see cref="MotionKitBlock"/>s managed by this behaviour.
		/// </summary>
		public void PlayAll() {
			TryInitialize();
			Blocks.ForEach(b => b?.Play());
		}

		/// <summary>
		/// Stops the <see cref="DefaultBlock"/>.
		/// </summary>
		public void Stop() {
			TryInitialize();
			DefaultBlock?.Stop();
		}

		/// <summary>
		/// Stops the <see cref="MotionKitBlock"/> with the specified <paramref name="blockPath"/>.
		/// </summary>
		/// <param name="blockPath">The name of the block.</param>
		public void Stop(string blockPath) {
			TryInitialize();
			GetChildAtPath(blockPath)?.Stop();
		}

		/// <summary>
		/// Stops all the <see cref="MotionKitBlock"/>s managed by this behaviour.
		/// </summary>
		public void StopAll() {
			TryInitialize();
			Blocks.ForEach(b => b?.Stop());
		}

		/// <summary>
		/// Pauses the <see cref="DefaultBlock"/>.
		/// </summary>
		public void Pause() {
			TryInitialize();
			DefaultBlock?.Pause();
		}

		/// <summary>
		/// Pauses the <see cref="MotionKitBlock"/> with the specified <paramref name="blockPath"/>.
		/// </summary>
		/// <param name="blockPath">The name of the block.</param>
		public void Pause(string blockPath) {
			TryInitialize();
			GetChildAtPath(blockPath)?.Pause();
		}


		/// <summary>
		/// Pauses all the <see cref="MotionKitBlock"/>s managed by this behaviour.
		/// </summary>
		public void PauseAll() {
			TryInitialize();
			Blocks.ForEach(b => b?.Pause());
		}

		/// <summary>
		/// Resumes the <see cref="DefaultBlock"/>.
		/// </summary>
		public void Resume() {
			TryInitialize();
			DefaultBlock?.Resume();
		}

		/// <summary>
		/// Resumes the <see cref="MotionKitBlock"/> with the specified <paramref name="blockPath"/>.
		/// </summary>
		/// <param name="blockPath">The name of the block.</param>
		public void Resume(string blockPath) {
			TryInitialize();
			GetChildAtPath(blockPath)?.Resume();
		}

		/// <summary>
		/// Resumes all the <see cref="MotionKitBlock"/>s managed by this behaviour.
		/// </summary>
		public void ResumeAll() {
			TryInitialize();
			Blocks.ForEach(b => b?.Resume());
		}


		/// <summary>
		/// Resets the <see cref="DefaultBlock"/> if it is a <see cref="IMotionBaseBlock"/>.
		/// </summary>
		public void ResetMotion() {
			TryInitialize();
			(DefaultBlock as IMotionBaseBlock)?.ForceResetPlayback();
		}

		/// <summary>
		/// Resets the <see cref="MotionKitBlock"/> with the specified <paramref name="blockPath"/>
		/// if it is a motion.
		/// </summary>
		/// <param name="blockPath">The path of the block. For example "Parallel/Sequence1/Motion2D"</param>
		public void ResetMotion(string blockPath) {
			TryInitialize();
			(GetChildAtPath(blockPath) as IMotionBaseBlock)?.ForceResetPlayback();
		}

		/// <summary>
		/// Calls <see cref="IMotionBaseBlock.TryResetPlayback()"/> in all the motions that are in this <see cref="MotionKitComponent"/>.
		/// If <paramref name="recursive"/> is <c>true</c>, it looks for children blocks too.
		/// </summary>
		public void ResetAllMotions(bool recursive = false) {
			TryInitialize();
			if (recursive) {
				Blocks.ForEach(B => ForAllChildrenBlocks(B, b => (b as IMotionBaseBlock)?.ForceResetPlayback()));
			} else {
				Blocks.ForEach(B => (B as IMotionBaseBlock)?.ForceResetPlayback());
			}
		}

		public MotionKitBlock GetChild(string name) {
			TryInitialize();
			return Blocks.FirstOrDefault(b => b != null && b.Name == name);
		}

		public MotionKitBlock GetChildAtPath(string path) {
			TryInitialize();
			return CompositeObjectUtility.GetChildAtPath(this, path);
		}

		public T GetChildAtPath<T>(string path) where T : MotionKitBlock {
			TryInitialize();
			return GetChildAtPath(path) as T;
		}

		public MotionKitBlock[] GetChildren() {
			TryInitialize();
			return Blocks.ToArray();
		}

		#endregion


		#region Unity Methods

		private void Start() {
			TryInitialize();
			for (int i = 0; i < Blocks.Count; i++) {
				if (Blocks[i] != null && 
					(Blocks[i].PlayOnStart || (i == 0 && PlayOnStart))) { // Supporting PlayOnStart for the first item for legacy reasons
					Blocks[i].Play();
				}
			}
		}

		private void OnDestroy() {
			Blocks.ForEach(b => b?.Dispose());
		}

		#endregion


		#region Private Fields - Serialized

		[SerializeReference]
		private List<MotionKitBlock> m_Blocks = new List<MotionKitBlock>();

		[Tooltip("Plays the first block on start")]
		[SerializeField]
		private bool m_PlayOnStart;

		/// <summary>
		/// Searches recursively for the first motions that modify properties and sets their progress
		/// to 0 so that the value of the property is set to the initial value.
		/// </summary>
		/// 
		/// <remarks>
		/// In <see cref="Parallel"/>s this searches on all parallel items. In <see cref="Sequence"/>s this
		/// searches until it finds the first motion.
		/// In this component, the search will be performed only in the <see cref="DefaultBlock"/>
		/// </remarks>
		[Tooltip(
			"On the first block, searches for motions that modify properties and set their initial values on " +
			"the corresponding objects, even if there are timers before the motions."
		)]
		[SerializeField]
		private bool m_SetInitialValuesOnStart;

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private bool m_AreBlocksInitialized;

		#endregion


		#region Private Properties		

		private List<MotionKitBlock> Blocks => m_Blocks;

		private bool PlayOnStart => m_PlayOnStart;

		private bool SetInitialValuesOnStart => m_SetInitialValuesOnStart;

		#endregion


		#region Private Methods

		private void TryInitialize() {
			if (!m_AreBlocksInitialized) {
				Initialize();
			}
		}

		/// <summary>
		/// Initializes all the <see cref="MotionKitBlock"/>s of this compopnent.
		/// </summary>
		private void Initialize() {

			m_AreBlocksInitialized = true;

			// TODO: Must review this logic since the change to support SetInitialValuesOnStart in each block

			// Initialize in reverse for the default block to be the last one. This handles an edge case where:
			// - The default block has an owner and reuse id
			// - A later block use the same owner and reuse id
			// - If initialized in order, the later block will rewrite the default one, causing it to behave like the later block
			// - Because we are locking it below, the default block won't reset on play
			for (int i = Blocks.Count - 1; i >= 0; i--) {
				Blocks[i]?.Initialize();
			}

			// This must be done after initializing the blocks
			for (int i = 0; i < Blocks.Count; i++) {
				if (Blocks[i] != null && 
					(Blocks[i].SetInitialValuesOnStart || (i == 0 && SetInitialValuesOnStart))) { // Supporting SetInitialValuesOnStart for the first item for legacy reasons
					MotionKitBlockUtility.SetInitialValues(Blocks[i]);
					// Lock recursive to make sure that no child motion will be reset OnStart
					//
					// DefaultBlock and its children will be unlocked OnStart or each playback object,
					// after trying unsuccessfully to reset each one. The unlock will happen non recursively.
					Blocks[i].LockResetPlayback(true);
				}
			}

		}

		/// <summary>
		/// Performs an action in all <see cref="MotionKitBlock"/>s under <paramref name="animateBlock"/>.
		/// </summary>
		/// <param name="animateBlock">The root <see cref="MotionKitBlock"/></param>
		/// <param name="action">The action to perform in it.</param>
		private void ForAllChildrenBlocks(MotionKitBlock animateBlock, Action<MotionKitBlock> action) {

			if (animateBlock == null) {
				return;
			}

			// Do the action
			action(animateBlock);

			if (animateBlock is ICompositeParent<MotionKitBlock>) {
				var animateParent = animateBlock as ICompositeParent<MotionKitBlock>;
				foreach (var item in animateParent.GetChildren()) {
					// Recursion
					ForAllChildrenBlocks(item, action);
				}
			}

		}

		#endregion


	}

}