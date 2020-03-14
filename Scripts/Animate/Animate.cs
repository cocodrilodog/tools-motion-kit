namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections.Generic;
	using UnityEngine;


	#region Interfaces

	public interface IPlayback {
		void Reset();
		void Stop();
		bool IsPlaying { get; }
		bool IsPaused { get; }
	}

	public interface ITimedProgressable {
		float Progress { get; set; }
		float Duration { get; }
	}

	#endregion

	/// 
	/// <summary>
	/// Utility singleton to create reusable animations and/or timed-callbacks with coroutines.
	/// </summary>
	/// 
	/// <remarks>
	/// <c>Motion</c>, <c>Timer</c> and <c>Sequence</c> are objects used to animate any
	/// property of any object.
	/// Playback objects are created and returned by these methods: 
	/// <see cref="GetMotion(object, string, MotionBase{float, MotionFloat}.Setter)"/>,
	/// <see cref="GetMotion(object, string, MotionBase{Vector3, Motion3D}.Setter)"/>, or
	/// <see cref="GetMotion(object, string, MotionBase{Color, MotionColor}.Setter)"/>
	/// <see cref="GetTimer(object, string)"/>
	/// 
	/// Those methods receive the following parameters: <c>owner</c>, <c>reuseKey</c>
	/// and <c>setter</c>, describd below.
	/// 
	/// If a <c>Playback</c> object with the same owner and reuseKey was already created before, 
	/// that will be the returned object. Otherwise a new <c>Playback</c> object  instance is 
	/// created and returned which will be stored internally for later reuse with the newly assigned
	/// combination of owner and reuseKey.
	/// 
	/// <example>
	/// <code>
	/// Animate.GetMotion(
	/// 	this, "MyPosition", s => transform.localPosition = s
	/// ).SetEasing(Quadratic.EaseOut).Play(Vector3.up, 0.3f);
	/// </code>
	/// </example>
	/// 
	/// The <c>owner</c>:
	/// Any object with which you want to relate the <c>Playback</c> object in order to 
	/// reuse it in multiple animations/callbacks. After you are done with the animations/callbacks
	/// of this owner, you can call <see cref="ClearPlaybacks(object)"/> to remove all of its related
	/// <c>Playback</c> objects forever.
	/// 
	/// The <c>reuseKey</c>:
	/// A string key that identifies all the animations/callbacks that will be performed by the same 
	/// <c>Playback</c> object object that is related to the same owner. <c>Playback</c> objects with 
	/// the same reuseKey and owner will stop to one another if one is started while the other 
	/// was playing.
	/// </remarks>
	public class Animate : MonoSingleton<Animate> {


		#region Public Static Methods

		/// <summary>
		/// Gets a <see cref="Motion3D"/> object ready to use for any animation.
		/// </summary>
		/// 
		/// <returns>The <see cref="Motion3D"/>.</returns>
		/// 
		/// <param name="owner">
		/// The owner of this motion.
		/// </param>
		/// 
		/// <param name="reuseKey">
		/// The reuseKey of this motion.
		/// </param>
		/// 
		/// <param name="setter">
		/// A function that sets the animated <see cref="Vector3"/> value. Can be a lambda expression.
		/// </param>
		public static Motion3D GetMotion(object owner, string reuseKey, Motion3D.Setter setter) {
			if (Instance != null) {
				return Instance._GetMotion(owner, reuseKey, setter);
			} else {
				return null;
			}
		}

		/// <summary>
		/// Gets a <see cref="MotionFloat"/> object ready to use for any animation.
		/// </summary>
		/// 
		/// <returns>The <see cref="MotionFloat"/>.</returns>
		/// 
		/// <param name="owner">
		/// The ownwer of this motion.
		/// </param>
		/// 
		/// <param name="reuseKey">
		/// The reuseKey of this motion.
		/// </param>
		/// 
		/// <param name="setter">
		/// A function that sets the animated <see cref="float"/> value. Can be a lambda expression.
		/// </param>
		public static MotionFloat GetMotion(object owner, string reuseKey, MotionFloat.Setter setter) {
			if (Instance != null) {
				return Instance._GetMotion(owner, reuseKey, setter);
			} else {
				return null;
			}
		}

		/// <summary>
		/// Gets a <see cref="MotionColor"/> object ready to use for any animation.
		/// </summary>
		/// 
		/// <returns>The <see cref="MotionColor"/>.</returns>
		/// 
		/// <param name="owner">
		/// The ownwer of this motion.
		/// </param>
		/// 
		/// <param name="reuseKey">
		/// The reuseKey of this motion.
		/// </param>
		/// 
		/// <param name="setter">
		/// A function that sets the animated <see cref="Color"/> value. Can be a lambda expression.
		/// </param>
		public static MotionColor GetMotion(object owner, string reuseKey, MotionColor.Setter setter) {
			if (Instance != null) {
				return Instance._GetMotion(owner, reuseKey, setter);
			} else {
				return null;
			}
		}

		/// <summary>
		/// Gets a <see cref="Timer"/> object ready be played.
		/// </summary>
		/// 
		/// <returns>The timer.</returns>
		/// 
		/// <param name="owner">
		/// The ownwer of this timer.
		/// </param>
		/// 
		/// <param name="reuseKey">
		/// The reuseKey of this timer.
		/// </param>
		public static Timer GetTimer(object owner, string reuseKey) {
			if (Instance != null) {
				return Instance._GetTimer(owner, reuseKey);
			} else {
				return null;
			}
		}

		public static Sequence GetSequence(object owner, string reuseKey, params ITimedProgressable[] timedProgressables) {
			if (Instance != null) {
				return Instance._GetSequence(owner, reuseKey, timedProgressables);
			} else {
				return null;
			}
		}

		/// <summary>
		/// Is the <c>Playback</c> object currently playing?
		/// </summary>
		/// <returns>
		/// <c>true</c>, if the <c>Playback</c> object is playing, <c>false</c> otherwise.
		/// </returns>
		/// <param name="owner">Owner.</param>
		/// <param name="reuseKey">Reuse key.</param>
		public static bool IsPlaybackPlaying(object owner, string reuseKey) {
			return Instance._IsPlaybackPlaying(owner, reuseKey);
		}

		/// <summary>
		/// Is the <c>Playback</c> object currently paused?
		/// </summary>
		/// <returns>
		/// <c>true</c>, if the <c>Playback</c> object is paused, <c>false</c> otherwise.
		/// </returns>
		/// <param name="owner">Owner.</param>
		/// <param name="reuseKey">Reuse key.</param>
		public static bool IsPlaybackPaused(object owner, string reuseKey) {
			return Instance._IsPlaybackPaused(owner, reuseKey);
		}

		/// <summary>
		/// Stops the <c>Playback</c> object with the specified <paramref name="owner"/> and 
		/// <paramref name="reuseKey"/>, if one is found.
		/// </summary>
		/// <returns>
		/// <c>true</c>, if the <c>Playback</c> object was found and stopped, <c>false</c> otherwise.
		/// </returns>
		/// <param name="owner">Owner.</param>
		/// <param name="reuseKey">Reuse key.</param>
		public static bool StopPlayback(object owner, string reuseKey) {
			return Instance._StopPlayback(owner, reuseKey);
		}

		/// <summary>
		/// Removes all cached <c>Playback</c> objects that are related to 
		/// <c>owner</c>
		/// </summary>
		/// <returns>
		/// <c>true</c>, if playbacks were cleared, <c>false</c> otherwise.
		/// </returns>
		/// <param name="owner">Owner.</param>
		public static bool ClearPlaybacks(object owner) {
			// Check for null because this may be called from OnDestroy() of another 
			// object and at that point, it is possible that the animate instance is 
			// already destroyed.
			if (Instance != null) {
				return Instance._ClearPlaybacks(owner);
			}
			return false;
		}

		/// <summary>
		/// Removes all cached <c>Playback</c> objects.
		/// </summary>
		public static void ClearAllPlaybacks() {
			// Check for null because this may be called from OnDestroy() of another 
			// object and at that point, it is possible that the animate instance is 
			// already destroyed.
			if (Instance != null) {
				Instance._ClearAllPlaybacks();
			}
		}

		#endregion


		#region MonoBehaviour Methods

		protected override void Awake() {
			base.Awake();
			DontDestroyOnLoad(gameObject);
		}

		#endregion


		#region Internal Fields

		[NonSerialized]
		Dictionary<object, Dictionary<string, IPlayback>> m_Playbacks;

		#endregion


		#region Internal Properties

		Dictionary<object, Dictionary<string, IPlayback>> Playbacks {
			get { return m_Playbacks = m_Playbacks ?? new Dictionary<object, Dictionary<string, IPlayback>>(); }
		}

		#endregion


		#region Internal Methods

		private Motion3D _GetMotion(object owner, string reuseKey, Motion3D.Setter setter) {
			return (Motion3D)_GetPlayback(owner, reuseKey, () => new Motion3D(this, setter));
		}

		private MotionFloat _GetMotion(object owner, string reuseKey, MotionFloat.Setter setter) {
			return (MotionFloat)_GetPlayback(owner, reuseKey, () => new MotionFloat(this, setter));
		}

		private MotionColor _GetMotion(object owner, string reuseKey, MotionColor.Setter setter) {
			return (MotionColor)_GetPlayback(owner, reuseKey, () => new MotionColor(this, setter));
		}

		private Timer _GetTimer(object owner, string reuseKey) {
			return (Timer)_GetPlayback(owner, reuseKey, () => new Timer(this));
		}

		private Sequence _GetSequence(object owner, string reuseKey, ITimedProgressable[] timedProgressables) {
			return (Sequence)_GetPlayback(owner, reuseKey, () => new Sequence(this, timedProgressables));
		}

		private IPlayback _GetPlayback(object owner, string reuseKey, Func<IPlayback> createPlayback) {
			Dictionary<string, IPlayback> ownerPlaybacks;
			if (Playbacks.TryGetValue(owner, out ownerPlaybacks)) {
				IPlayback playback;
				// There is a owner object registered, let's search for the reuseKey
				if (ownerPlaybacks.TryGetValue(reuseKey, out playback)) {
					// That owner did register that reuseKey, so return the existing motion
					return playback;
				} else {
					// The target doesn't have the key yet, so create the reuseKey and motion
					return ownerPlaybacks[reuseKey] = createPlayback();
				}
			} else {
				// There is no target registered. Must register it as well as the reuseKey and 
				// create a new motion
				ownerPlaybacks = new Dictionary<string, IPlayback>();
				Playbacks[owner] = ownerPlaybacks;
				return ownerPlaybacks[reuseKey] = createPlayback();
			}
		}

		private bool _IsPlaybackPlaying(object owner, string reuseKey) {
			Dictionary<string, IPlayback> ownerPlaybacks;
			if (Playbacks.TryGetValue(owner, out ownerPlaybacks)) {
				IPlayback playback;
				if (ownerPlaybacks.TryGetValue(reuseKey, out playback)) {
					return playback.IsPlaying;
				}
			}
			return false;
		}

		private bool _IsPlaybackPaused(object owner, string reuseKey) {
			Dictionary<string, IPlayback> ownerPlaybacks;
			if (Playbacks.TryGetValue(owner, out ownerPlaybacks)) {
				IPlayback playback;
				if (ownerPlaybacks.TryGetValue(reuseKey, out playback)) {
					return playback.IsPaused;
				}
			}
			return false;
		}

		private bool _StopPlayback(object owner, string reuseKey) {
			Dictionary<string, IPlayback> ownerPlaybacks;
			if (Playbacks.TryGetValue(owner, out ownerPlaybacks)) {
				IPlayback playback;
				if (ownerPlaybacks.TryGetValue(reuseKey, out playback)) {
					playback.Stop();
					return true;
				}
			}
			return false;
		}

		private bool _ClearPlaybacks(object owner) {
			Dictionary<string, IPlayback> ownerPlaybacks;
			if (Playbacks.TryGetValue(owner, out ownerPlaybacks)) {
				foreach (KeyValuePair<string, IPlayback> entry in ownerPlaybacks) {
					entry.Value.Reset();
				}
				Playbacks.Remove(owner);
				return true;
			} else {
				return false;
			}
		}

		private void _ClearAllPlaybacks() {
			foreach (KeyValuePair<object, Dictionary<string, IPlayback>> ownerEntry in Playbacks) {
				foreach (KeyValuePair<string, IPlayback> entry in ownerEntry.Value) {
					entry.Value.Reset();
				}
				ownerEntry.Value.Clear();
			}
			Playbacks.Clear();
		}

		#endregion


	}
}
