﻿namespace CocodriloDog.MotionKit {

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
		void SetProgress(float progress, bool invokeCallbacks);
		void ResetState();
		void InvokeOnInterrupt();
	}

	#endregion

	/// <summary>
	/// Singleton to create reusable motions, timers, sequences and parallels which altogether
	/// are used to create animations and are called playback objects.
	/// </summary>
	/// 
	/// <remarks>
	/// <c>Motion</c>, <c>Timer</c>, <c>Sequence</c>, and <c>Parallel</c> are objects used to animate any
	/// property of any object.
	///
	/// Registered playback objects that can be reused are created and returned by the methods that implement 
	/// the <c>owner</c> and <c>reuseID</c>, like this one: 
	/// <see cref="GetMotion(object, string, MotionBase{float, MotionFloat}.Setter)"/>,
	/// 
	/// There are also overloads to create non-registered motions, timers, sequences, and parallels, like this one:
	/// <see cref="GetMotion(MotionBase{float, MotionFloat}.Setter)"/>,
	/// </remarks>
	[AddComponentMenu("")]
	public class MotionKit : MonoSingleton<MotionKit> {


		#region Public Static Methods

		/// <summary>
		/// Gets a <see cref="Motion3D"/> object ready to use for any animation and registers it with
		/// its <paramref name="owner"/> and <paramref name="reuseID"/> for later reuse.
		/// </summary>
		/// 
		/// <remarks>
		/// If this method is invoked subsequently with the same <paramref name="owner"/> and <paramref name="reuseID"/>
		/// as before, it will use and return the same motion object that was created previously.
		/// </remarks>
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
			}
			return null;
		}

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
			}
			return null;
		}

		/// <summary>
		/// Gets a <see cref="MotionFloat"/> object ready to use for any animation and registers it with
		/// its <paramref name="owner"/> and <paramref name="reuseID"/> for later reuse.
		/// </summary>
		/// 
		/// <remarks>
		/// If this method is invoked subsequently with the same <paramref name="owner"/> and <paramref name="reuseID"/>
		/// as before, it will use and return the same motion object that was created previously.
		/// </remarks>
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
			}
			return null;
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
			}
			return null;
		}

		/// <summary>
		/// Gets a <see cref="MotionColor"/> object ready to use for any animation and registers it with
		/// its <paramref name="owner"/> and <paramref name="reuseID"/> for later reuse.
		/// </summary>
		/// 
		/// <remarks>
		/// If this method is invoked subsequently with the same <paramref name="owner"/> and <paramref name="reuseID"/>
		/// as before, it will use and return the same motion object that was created previously.
		/// </remarks>
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
			}
			return null;
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
			}
			return null;
		}

		/// <summary>
		/// Gets a <see cref="Timer"/> object ready be played and registers it with
		/// its <paramref name="owner"/> and <paramref name="reuseID"/> for later reuse.
		/// </summary>
		/// 
		/// <remarks>
		/// If this method is invoked subsequently with the same <paramref name="owner"/> and <paramref name="reuseID"/>
		/// as before, it will use and return the same timer object that was created previously.
		/// </remarks>
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
			}
			return null;
		}

		/// <summary>
		/// Gets a <see cref="Timer"/> object ready be played.
		/// </summary>
		/// 
		/// <returns>The timer.</returns>
		public static Timer GetTimer() {
			if (Instance != null) {
				return Instance._GetTimer();
			}
			return null;
		}

		/// <summary>
		/// Gets a <see cref="Sequence"/> object ready be played and registers it with
		/// its <paramref name="owner"/> and <paramref name="reuseID"/> for later reuse.
		/// </summary>
		/// 
		/// <remarks>
		/// If this method is invoked subsequently with the same <paramref name="owner"/> and <paramref name="reuseID"/>
		/// as before, it will use and return the same sequence object that was created previously.
		/// </remarks>
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
			}
			return null;
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
			}
			return null;
		}

		/// <summary>
		/// Gets a <see cref="Parallel"/> object ready be played and registers it with
		/// its <paramref name="owner"/> and <paramref name="reuseID"/> for later reuse.
		/// </summary>
		/// 
		/// <remarks>
		/// If this method is invoked subsequently with the same <paramref name="owner"/> and <paramref name="reuseID"/>
		/// as before, it will use and return the same parallel object that was created previously.
		/// </remarks>
		/// 
		/// <returns>The parallel.</returns>
		/// 
		/// <param name="owner">
		/// The ownwer of this parallel.
		/// </param>
		/// 
		/// <param name="reuseID">
		/// The reuseID of this parallel.
		/// </param>
		///
		/// <param name="parallelItems">
		/// The items that will make the parallel.
		/// </param>
		public static Parallel GetParallel(object owner, string reuseID, params ITimedProgressable[] parallelItems) {
			if (Instance != null) {
				return Instance._GetParallel(owner, reuseID, parallelItems);
			}
			return null;
		}

		/// <summary>
		/// Gets a <see cref="Parallel"/> object ready be played.
		/// </summary>
		/// 
		/// <returns>The parallel.</returns>
		///
		/// <param name="parallelItems">
		/// The items that will make the parallel.
		/// </param>
		public static Parallel GetParallel(params ITimedProgressable[] parallelItems) {
			if (Instance != null) {
				return Instance._GetParallel(parallelItems);
			}
			return null;
		}

		/// <summary>
		/// Finds the <c>Motion</c>, <c>Timer</c>, <c>Sequence</c> or <c>Parallel</c> registered with 
		/// <paramref name="owner"/> and the <paramref name="reuseID"/>.
		/// </summary>
		/// <param name="owner">The owner</param>
		/// <param name="reuseID">The reuse id</param>
		/// <returns>The playback object</returns>
		public static IPlayback FindPlayback(object owner, string reuseID) {
			if (Instance != null) {
				return Instance._FindPlayback(owner, reuseID);
			}
			return null;
		}

		/// <summary>
		/// Finds the <c>Motion</c>, <c>Timer</c>, <c>Sequence</c> or <c>Parallel</c> registered with 
		/// <paramref name="owner"/> and the <paramref name="reuseID"/>.
		/// </summary>
		/// <param name="owner">The owner</param>
		/// <param name="reuseID">The reuse id</param>
		/// <returns>The playback object</returns>
		public static T FindPlayback<T>(object owner, string reuseID) where T : class, IPlayback {
			if (Instance != null) {
				return Instance._FindPlayback(owner, reuseID) as T;
			}
			return default(T);
		}

		/// <summary>
		/// Removes the cached <c>Playback</c> object that is owned by <paramref name="owner"/> 
		/// and has the <paramref name="reuseId"/>.
		/// </summary>
		/// <returns>
		/// <c>true</c>, if the playback was cleared, <c>false</c> otherwise.
		/// </returns>
		/// <param name="owner">the owner</param>
		/// <param name="reuseId">The reuse id</param>
		public static bool ClearPlayback(object owner, string reuseId) {
			// Check for null because this may be called from OnDestroy() of another 
			// object and at that point, it is possible that the animate instance is 
			// already destroyed.
			if (Instance != null) {
				return Instance._ClearPlayback(owner, reuseId);
			}
			return false;
		}

		/// <summary>
		/// Removes all registered <c>Playback</c> objects that are related to 
		/// <c>owner</c>
		/// </summary>
		/// <returns>
		/// <c>true</c>, if playbacks were cleared, <c>false</c> otherwise.
		/// </returns>
		/// <param name="owner">The owner</param>
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
		
		private Parallel _GetParallel(ITimedProgressable[] parallelItems) {
			return new Parallel(this, parallelItems);
		}

		private Parallel _GetParallel(object owner, string reuseID, ITimedProgressable[] parallelItems) {
			return (Parallel)_GetPlayback(owner, reuseID, () => new Parallel(this, parallelItems), parallelItems);
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

		private IPlayback _FindPlayback(object owner, string reuseID) {
			if (Playbacks.TryGetValue(owner, out Dictionary<string, IPlayback> ownerPlaybacks)) {
				IPlayback playback;
				if (ownerPlaybacks.TryGetValue(reuseID, out playback)) {
					return playback;
				}
			}
			return null;
		}

		private bool _ClearPlaybacks(object owner) {
			if (Playbacks.TryGetValue(owner, out var ownerPlaybacks)) {
				foreach (KeyValuePair<string, IPlayback> entry in ownerPlaybacks) {
					entry.Value.Dispose();
				}
				Playbacks.Remove(owner);
				return true;
			} else {
				return false;
			}
		}

		private bool _ClearPlayback(object owner, string reuseID) {
			if (Playbacks.TryGetValue(owner, out var ownerPlaybacks)) {
				if (ownerPlaybacks.TryGetValue(reuseID, out var playback)) {
					playback.Dispose();
					ownerPlaybacks.Remove(reuseID);
					if (ownerPlaybacks.Count == 0) {
						Playbacks.Remove(owner);
					}
					return true;
				}
			}
			return false;
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
