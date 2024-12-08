namespace CocodriloDog.MotionKit {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Playables;
	using UnityEngine.Animations;
	using System;

	/// <summary>
	/// Allows to scrub through the timeline of an <see cref="AnimationClip"/> via the <c>Time</c>
	/// and <c>NormalizedTime</c> properties. 
	/// </summary>
	/// 
	/// <remarks>
	/// This is designed to be used by the <see cref="MotionKit"/> playback system, by creating a 
	/// <see cref="MotionFloat"/> and animating either <c>Time</c> or <c>NormalizedTime</c>.
	/// For optimal use, disable this component while no animation is needed.
	/// </remarks>
	public class AnimationClipAdapter : MonoBehaviour {

		// TODO: Implement a culling system.
		#region Public Properties

		/// <summary>
		/// The playhead position in seconds.
		/// </summary>
		public float Time {
			get {
				if (Application.isPlaying) {
					return (float)ClipPlayable.GetTime();
				} else {
					return 0;
				}
			}
			set {
				if (Application.isPlaying) {
					Initialize();
					ClipPlayable.SetTime(value);
				}
			}
		}

		/// <summary>
		/// The playhead position from 0 to 1.
		/// </summary>
		public float NormalizedTime {
			get => Time / m_AnimationClip.length;
			set => Time = value * m_AnimationClip.length;
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Pauses the ClipPlayable.
		/// </summary>
		/// <remarks>
		/// Implemented to be used by the editor on undo. It is needed because on undo, Unity
		/// resets the ClipPlayable state to playing.
		/// </remarks>
		public void Pause() => ClipPlayable.Pause();

		#endregion


		#region Unity Methods

		private void Awake() => Initialize();

		private void OnEnable() {
			PlayableGraph.Play();
			ClipPlayable.Pause();
		}

		private void OnDisable() => PlayableGraph.Stop();

		private void OnDestroy() => PlayableGraph.Destroy();

		#endregion


		#region Private Fields - Serialized

		[SerializeField]
		private AnimationClip m_AnimationClip;

		#endregion


		#region Private Fields - Non Serialized

		[NonSerialized]
		private Animator m_Animator;

		[NonSerialized]
		private PlayableGraph? m_PlayableGraph;

		[NonSerialized]
		private AnimationClipPlayable? m_ClipPlayable;

		[NonSerialized]
		private bool m_IsInitialized;

		#endregion


		#region Private Properties

		private Animator Animator {
			get {
				if (m_Animator == null) {
					m_Animator = gameObject.AddComponent<Animator>();
					m_Animator.hideFlags = HideFlags.HideInInspector | HideFlags.DontSaveInEditor;
				}
				return m_Animator;
			}
		}

		private PlayableGraph PlayableGraph {
			get {
				if(m_PlayableGraph == null) {
					m_PlayableGraph = PlayableGraph.Create("AnimationClipGraph");
				}
				return m_PlayableGraph.Value;
			}
		}

		private AnimationClipPlayable ClipPlayable {
			get {
				if (m_ClipPlayable == null) {
					m_ClipPlayable = AnimationClipPlayable.Create(PlayableGraph, m_AnimationClip);
				}
				return m_ClipPlayable.Value;
			}
		}

		#endregion


		#region Private Methods

		private void Initialize() {
			if (!m_IsInitialized) {
				m_IsInitialized = true;
				var playableOutput = AnimationPlayableOutput.Create(PlayableGraph, "AnimationOutput", Animator);
				playableOutput.SetSourcePlayable(ClipPlayable);
			}
		}

		#endregion


	}

}