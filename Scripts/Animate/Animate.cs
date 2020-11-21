namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections.Generic;
	using UnityEngine;


	#region Interfaces

	public interface IDisposable {
		void Dispose();
	}

	public interface IPlayback : IDisposable {
		bool IsPlaying { get; }
		bool IsPaused { get; }
		void Stop();
		void SetAnimatableElement(object element);
	}

	public interface ITimedProgressable : IDisposable {
		float Progress { get; set; }
		float Duration { get; }
		void InvokeOnUpdate();
		void InvokeOnInterrupt();
		void InvokeOnComplete();
	}

	#endregion

	/// 
	/// <summary>
	/// Singleton to create reusable motions, timers and sequences which altogether
	/// are used to create animations and called playback objects.
	/// </summary>
	/// 
	/// <remarks>
	/// <c>Motion</c>, <c>Timer</c> and <c>Sequence</c> are objects used to animate any
	/// property of any object.
	///
	/// Playback objects are created and returned by Methods like these: 
	/// <see cref="GetMotion(MotionBase{float, MotionFloat}.Setter)"/>,
	/// <see cref="GetTimer()"/> and
	/// <see cref="GetSequence(ITimedProgressable[])"/>
	/// 
	/// There are also overloads to create registered motions, timers and sequences:
	/// <see cref="GetMotion(object, string, MotionBase{float, MotionFloat}.Setter)"/>,
	/// <see cref="GetTimer(object, string)"/> and
	/// <see cref="GetSequence(object, string, ITimedProgressable[])"/>
	/// 
	/// Those methods receive the following parameters: <c>owner</c>, <c>reuseID</c>
	/// and other parameters.
	/// 
	/// If a <c>Playback</c> object with the same owner and reuseID was already created before, 
	/// that will be the returned object. Otherwise a new <c>Playback</c> object  instance is 
	/// created and returned which will be stored internally for later reuse with the newly assigned
	/// combination of owner and reuseID.
	/// 
	/// <example>
	/// <code>
	/// Animate.GetMotion(
	/// 	this, "PositionMotion", s => transform.localPosition = s
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
	/// The <c>reuseID</c>:
	/// A string that identifies all the animations/callbacks that will be performed by the same 
	/// <c>Playback</c> object object that is related to the same owner. Motions, timers and sequences
	/// started with the same reuseID and owner will stop to one another if one is started while the other 
	/// was playing because they are carried out by the same <c>Playback</c> object.
	///
	/// </remarks>
	public class Animate : MonoSingleton<Animate> {


		#region Public Static Methods

		/// <summary>
		/// Gets a <see cref="Motion3D"/> object ready to use for any animation.
		/// </summary>
		/// 
		/// <returns>The <see cref="Motion3D"/>.</returns>
		/// 
		/// <param name="setter">
		/// A function that sets the animated <see cref="Vector3"/> value. Can be a lambda expression.
		/// </param>
		public static Motion3D GetMotion(Motion3D.Setter setter) {
			if (Instance != null) {
				return Instance._GetMotion(setter);
			} else {
				return null;
			}
		}

		/// <summary>
		/// Gets a <see cref="Motion3D"/> object ready to use for any animation and registers it with
		/// its <paramref name="owner"/> and <paramref name="reuseID"/> for later reuse.
		/// </summary>
		/// 
		/// <returns>The <see cref="Motion3D"/>.</returns>
		/// 
		/// <param name="owner">
		/// The owner of this motion.
		/// </param>
		/// 
		/// <param name="reuseID">
		/// The reuseID of this motion.
		/// </param>
		/// 
		/// <param name="setter">
		/// A function that sets the animated <see cref="Vector3"/> value. Can be a lambda expression.
		/// </param>
		public static Motion3D GetMotion(object owner, string reuseID, Motion3D.Setter setter) {
			if (Instance != null) {
				return Instance._GetMotion(owner, reuseID, setter);
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
		/// <param name="setter">
		/// A function that sets the animated <see cref="float"/> value. Can be a lambda expression.
		/// </param>
		public static MotionFloat GetMotion(MotionFloat.Setter setter) {
			if (Instance != null) {
				return Instance._GetMotion(setter);
			} else {
				return null;
			}
		}

		/// <summary>
		/// Gets a <see cref="MotionFloat"/> object ready to use for any animation and registers it with
		/// its <paramref name="owner"/> and <paramref name="reuseID"/> for later reuse.
		/// </summary>
		/// 
		/// <returns>The <see cref="MotionFloat"/>.</returns>
		/// 
		/// <param name="owner">
		/// The ownwer of this motion.
		/// </param>
		/// 
		/// <param name="reuseID">
		/// The reuseID of this motion.
		/// </param>
		/// 
		/// <param name="setter">
		/// A function that sets the animated <see cref="float"/> value. Can be a lambda expression.
		/// </param>
		public static MotionFloat GetMotion(object owner, string reuseID, MotionFloat.Setter setter) {
			if (Instance != null) {
				return Instance._GetMotion(owner, reuseID, setter);
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
		/// <param name="setter">
		/// A function that sets the animated <see cref="Color"/> value. Can be a lambda expression.
		/// </param>
		public static MotionColor GetMotion(MotionColor.Setter setter) {
			if (Instance != null) {
				return Instance._GetMotion(setter);
			} else {
				return null;
			}
		}

		/// <summary>
		/// Gets a <see cref="MotionColor"/> object ready to use for any animation and registers it with
		/// its <paramref name="owner"/> and <paramref name="reuseID"/> for later reuse.
		/// </summary>
		/// 
		/// <returns>The <see cref="MotionColor"/>.</returns>
		/// 
		/// <param name="owner">
		/// The ownwer of this motion.
		/// </param>
		/// 
		/// <param name="reuseID">
		/// The reuseID of this motion.
		/// </param>
		/// 
		/// <param name="setter">
		/// A function that sets the animated <see cref="Color"/> value. Can be a lambda expression.
		/// </param>
		public static MotionColor GetMotion(object owner, string reuseID, MotionColor.Setter setter) {
			if (Instance != null) {
				return Instance._GetMotion(owner, reuseID, setter);
			} else {
				return null;
			}
		}

		/// <summary>
		/// Gets a <see cref="Timer"/> object ready be played.
		/// </summary>
		/// 
		/// <returns>The timer.</returns>
		public static Timer GetTimer() {
			if (Instance != null) {
				return Instance._GetTimer();
			} else {
				return null;
			}
		}

		/// <summary>
		/// Gets a <see cref="Timer"/> object ready be played and registers it with
		/// its <paramref name="owner"/> and <paramref name="reuseID"/> for later reuse.
		/// </summary>
		/// 
		/// <returns>The timer.</returns>
		/// 
		/// <param name="owner">
		/// The ownwer of this timer.
		/// </param>
		/// 
		/// <param name="reuseID">
		/// The reuseID of this timer.
		/// </param>
		public static Timer GetTimer(object owner, string reuseID) {
			if (Instance != null) {
				return Instance._GetTimer(owner, reuseID);
			} else {
				return null;
			}
		}

		/// <summary>
		/// Gets a <see cref="Sequence"/> object ready be played.
		/// </summary>
		/// 
		/// <returns>The sequence.</returns>
		///
		/// <param name="sequenceItems">
		/// The items that will make the sequence.
		/// </param>
		public static Sequence GetSequence(params ITimedProgressable[] sequenceItems) {
			if (Instance != null) {
				return Instance._GetSequence(sequenceItems);
			} else {
				return null;
			}
		}

		/// <summary>
		/// Gets a <see cref="Sequence"/> object ready be played and registers it with
		/// its <paramref name="owner"/> and <paramref name="reuseID"/> for later reuse.
		/// </summary>
		/// 
		/// <returns>The sequence.</returns>
		/// 
		/// <param name="owner">
		/// The ownwer of this sequence.
		/// </param>
		/// 
		/// <param name="reuseID">
		/// The reuseID of this sequence.
		/// </param>
		///
		/// <param name="sequenceItems">
		/// The items that will make the sequence.
		/// </param>
		public static Sequence GetSequence(object owner, string reuseID, params ITimedProgressable[] sequenceItems) {
			if (Instance != null) {
				return Instance._GetSequence(owner, reuseID, sequenceItems);
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
		/// <param name="reuseID">Reuse id.</param>
		public static bool IsPlaybackPlaying(object owner, string reuseID) {
			return Instance._IsPlaybackPlaying(owner, reuseID);
		}

		/// <summary>
		/// Is the <c>Playback</c> object currently paused?
		/// </summary>
		/// <returns>
		/// <c>true</c>, if the <c>Playback</c> object is paused, <c>false</c> otherwise.
		/// </returns>
		/// <param name="owner">Owner.</param>
		/// <param name="reuseID">Reuse id.</param>
		public static bool IsPlaybackPaused(object owner, string reuseID) {
			return Instance._IsPlaybackPaused(owner, reuseID);
		}

		/// <summary>
		/// Stops the <c>Playback</c> object with the specified <paramref name="owner"/> and 
		/// <paramref name="reuseID"/>, if one is found.
		/// </summary>
		/// <returns>
		/// <c>true</c>, if the <c>Playback</c> object was found and stopped, <c>false</c> otherwise.
		/// </returns>
		/// <param name="owner">Owner.</param>
		/// <param name="reuseID">Reuse id.</param>
		public static bool StopPlayback(object owner, string reuseID) {
			return Instance._StopPlayback(owner, reuseID);
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


		#region Private Properties

		private Dictionary<object, Dictionary<string, IPlayback>> Playbacks {
			get { return m_Playbacks = m_Playbacks ?? new Dictionary<object, Dictionary<string, IPlayback>>(); }
		}

		#endregion


		#region Private Methods

		private Motion3D _GetMotion(Motion3D.Setter setter) {
			return new Motion3D(this, setter);
		}

		private Motion3D _GetMotion(object owner, string reuseID, Motion3D.Setter setter) {
			return (Motion3D)_GetPlayback(owner, reuseID, () => new Motion3D(this, setter), setter);
		}

		private MotionFloat _GetMotion(MotionFloat.Setter setter) {
			return new MotionFloat(this, setter);
		}

		private MotionFloat _GetMotion(object owner, string reuseID, MotionFloat.Setter setter) {
			return (MotionFloat)_GetPlayback(owner, reuseID, () => new MotionFloat(this, setter), setter);
		}

		private MotionColor _GetMotion(MotionColor.Setter setter) {
			return new MotionColor(this, setter);
		}

		private MotionColor _GetMotion(object owner, string reuseID, MotionColor.Setter setter) {
			return (MotionColor)_GetPlayback(owner, reuseID, () => new MotionColor(this, setter), setter);
		}

		private Timer _GetTimer() {
			return new Timer(this);
		}

		private Timer _GetTimer(object owner, string reuseID) {
			return (Timer)_GetPlayback(owner, reuseID, () => new Timer(this));
		}

		private Sequence _GetSequence(ITimedProgressable[] sequenceItems) {
			return new Sequence(this, sequenceItems);
		}

		private Sequence _GetSequence(object owner, string reuseID, ITimedProgressable[] sequenceItems) {
			return (Sequence)_GetPlayback(owner, reuseID, () => new Sequence(this, sequenceItems), sequenceItems);
		}

		private IPlayback _GetPlayback(object owner, string reuseID, Func<IPlayback> createPlayback, object animatableElement = null) {
			if (Playbacks.TryGetValue(owner, out Dictionary<string, IPlayback> ownerPlaybacks)) {
				// There is an owner object registered, let's search for the reuseID
				if (ownerPlaybacks.TryGetValue(reuseID, out IPlayback playback)) {
					// That owner did register that reuseID, so return the existing playback
					playback.SetAnimatableElement(animatableElement);
					return playback;
				} else {
					// The target doesn't have the key yet, so create the reuseID and playback
					return ownerPlaybacks[reuseID] = createPlayback();
				}
			} else {
				// There is no target registered. Must register it as well as the reuseID and 
				// create a new motion
				ownerPlaybacks = new Dictionary<string, IPlayback>();
				Playbacks[owner] = ownerPlaybacks;
				return ownerPlaybacks[reuseID] = createPlayback();
			}
		}

		private bool _IsPlaybackPlaying(object owner, string reuseID) {
			Dictionary<string, IPlayback> ownerPlaybacks;
			if (Playbacks.TryGetValue(owner, out ownerPlaybacks)) {
				IPlayback playback;
				if (ownerPlaybacks.TryGetValue(reuseID, out playback)) {
					return playback.IsPlaying;
				}
			}
			return false;
		}

		private bool _IsPlaybackPaused(object owner, string reuseID) {
			Dictionary<string, IPlayback> ownerPlaybacks;
			if (Playbacks.TryGetValue(owner, out ownerPlaybacks)) {
				IPlayback playback;
				if (ownerPlaybacks.TryGetValue(reuseID, out playback)) {
					return playback.IsPaused;
				}
			}
			return false;
		}

		private bool _StopPlayback(object owner, string reuseID) {
			Dictionary<string, IPlayback> ownerPlaybacks;
			if (Playbacks.TryGetValue(owner, out ownerPlaybacks)) {
				IPlayback playback;
				if (ownerPlaybacks.TryGetValue(reuseID, out playback)) {
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
					entry.Value.Dispose();
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
					entry.Value.Dispose();
				}
				ownerEntry.Value.Clear();
			}
			Playbacks.Clear();
		}

		#endregion


	}
}
