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
	/// Utility singleton to create reusable animations with coroutines. It can be used in combination
	/// with <see cref="SagoEasing"/> classes.
	/// </summary>
	/// 
	/// <remarks>
	/// Motion objects are created and returned by these methods: 
	/// <see cref="GetMotion(object, string, MotionBase{float, MotionFloat}.Setter)"/>,
	/// <see cref="GetMotion(object, string, MotionBase{Vector3, Motion3D}.Setter)"/>, or
	/// <see cref="GetMotion(object, string, MotionBase{Color, MotionColor}.Setter)"/>
	/// <see cref="GetDelay(object, string)"/>
	/// 
	/// Those methods receive the following parameters: <c>owner</c>, <c>reuseKey</c>, <c>getter</c> 
	/// and <c>setter</c>, describd below.
	/// 
	/// If a <see cref="MotionBase{ValueT, MotionT}"/> with the same owner and reuseKey was already created before, 
	/// that will be the returned object. Otherwise a new <see cref="MotionBase{ValueT, MotionT}"/> instance is 
	/// created and returned which will be stored internally for later reuse with the newly assigned
	/// combination of owner and reuseKey.
	/// 
	/// <example>
	/// <code>
	/// Animate.GetMotion(
	/// 	this, "MyPosition", () => transform.localPosition, (s) => transform.localPosition = s
	/// ).SetEasing(Quadratic.EaseOut).Play(Vector3.up, 0.3f);
	/// </code>
	/// </example>
	/// 
	/// The <c>owner</c>:
	/// Any object with which you want to relate the <see cref="MotionBase{ValueT, MotionT}"/> object in order to 
	/// reuse it in multiple animations. After you are done with the animations of this owner, you 
	/// can call <see cref="ClearMotions(object)"/> to remove all of its related <see cref="MotionBase{ValueT, MotionT}"/>
	/// objects forever.
	/// 
	/// The <c>reuseKey</c>:
	/// A string key that identifies all the animations that will be performed by the same 
	/// <see cref=" MotionBase{ValueT, MotionT}"/> object that is related to the same owner. Animations with 
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
		/// The ownwe of thi motion.
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
		/// Gets a <see cref="Delay"/> object ready be played.
		/// </summary>
		/// 
		/// <returns>The delay.</returns>
		/// 
		/// <param name="owner">
		/// The ownwer of this delay.
		/// </param>
		/// 
		/// <param name="reuseKey">
		/// The reuseKey of this delay.
		/// </param>
		public static Delay GetDelay(object owner, string reuseKey) {
			if (Instance != null) {
				return Instance._GetDelay(owner, reuseKey);
			} else {
				return null;
			}
		}

		/// <summary>
		/// Is the motion currently playing?
		/// </summary>
		/// <returns><c>true</c>, if motion playing is playing, <c>false</c> otherwise.</returns>
		/// <param name="owner">Owner.</param>
		/// <param name="reuseKey">Reuse key.</param>
		public static bool IsMotionPlaying(object owner, string reuseKey) {
			return Instance._IsMotionPlaying(owner, reuseKey);
		}

		/// <summary>
		/// Is the motion currently paused?
		/// </summary>
		/// <returns><c>true</c>, if motion is paused, <c>false</c> otherwise.</returns>
		/// <param name="owner">Owner.</param>
		/// <param name="reuseKey">Reuse key.</param>
		public static bool IsMotionPaused(object owner, string reuseKey) {
			return Instance._IsMotionPaused(owner, reuseKey);
		}

		/// <summary>
		/// Stops the motion with the specified <paramref name="owner"/> and 
		/// <paramref name="reuseKey"/>, if one is found.
		/// </summary>
		/// <returns><c>true</c>, if motion was found and stopped, <c>false</c> otherwise.</returns>
		/// <param name="owner">Owner.</param>
		/// <param name="reuseKey">Reuse key.</param>
		public static bool StopMotion(object owner, string reuseKey) {
			return Instance._StopMotion(owner, reuseKey);
		}

		/// <summary>
		/// Removes all cached <see cref="MotionBase{ValueT, MotionT}"/> objects that are related to 
		/// <c>owner</c>
		/// </summary>
		/// <returns><c>true</c>, if motions were cleared, <c>false</c> otherwise.</returns>
		/// <param name="owner">Owner.</param>
		public static bool ClearMotions(object owner) {
			// Check for null because this may be called from OnDestroy() of another 
			// object and at that point, it is possible that the animate instance is 
			// already destroyed.
			if (Instance != null) {
				return Instance._ClearMotions(owner);
			}
			return false;
		}

		/// <summary>
		/// Removes all cached <see cref="MotionBase{ValueT, MotionT}"/> objects.
		/// </summary>
		public static void ClearAllMotions() {
			// Check for null because this may be called from OnDestroy() of another 
			// object and at that point, it is possible that the animate instance is 
			// already destroyed.
			if (Instance != null) {
				Instance._ClearAllMotions();
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
		Dictionary<object, Dictionary<string, IPlayback>> m_Motions;

		#endregion


		#region Internal Properties

		Dictionary<object, Dictionary<string, IPlayback>> Motions {
			get { return m_Motions = m_Motions ?? new Dictionary<object, Dictionary<string, IPlayback>>(); }
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

		private Delay _GetDelay(object owner, string reuseKey) {
			return (Delay)_GetPlayback(owner, reuseKey, () => new Delay(this));
		}

		private IPlayback _GetPlayback(object owner, string reuseKey, Func<IPlayback> createMotion) {
			Dictionary<string, IPlayback> ownerMotions;
			if (Motions.TryGetValue(owner, out ownerMotions)) {
				IPlayback motion;
				// There is a owner object registered, let's search for the reuseKey
				if (ownerMotions.TryGetValue(reuseKey, out motion)) {
					// That owner did register that reuseKey, so return the existing motion
					return motion;
				} else {
					// The target doesn't have the key yet, so create the reuseKey and motion
					return ownerMotions[reuseKey] = createMotion();
				}
			} else {
				// There is no target registered. Must register it as well as the reuseKey and 
				// create a new motion
				ownerMotions = new Dictionary<string, IPlayback>();
				Motions[owner] = ownerMotions;
				return ownerMotions[reuseKey] = createMotion();
			}
		}

		private bool _IsMotionPlaying(object owner, string reuseKey) {
			Dictionary<string, IPlayback> ownerMotions;
			if (Motions.TryGetValue(owner, out ownerMotions)) {
				IPlayback motion;
				if (ownerMotions.TryGetValue(reuseKey, out motion)) {
					return motion.IsPlaying;
				}
			}
			return false;
		}

		private bool _IsMotionPaused(object owner, string reuseKey) {
			Dictionary<string, IPlayback> ownerMotions;
			if (Motions.TryGetValue(owner, out ownerMotions)) {
				IPlayback motion;
				if (ownerMotions.TryGetValue(reuseKey, out motion)) {
					return motion.IsPaused;
				}
			}
			return false;
		}

		private bool _StopMotion(object owner, string reuseKey) {
			Dictionary<string, IPlayback> ownerMotions;
			if (Motions.TryGetValue(owner, out ownerMotions)) {
				IPlayback motion;
				if(ownerMotions.TryGetValue(reuseKey, out motion)) {
					motion.Stop();
					return true;
				}
			}
			return false;
		}

		private bool _ClearMotions(object owner) {
			Dictionary<string, IPlayback> ownerMotions;
			if (Motions.TryGetValue(owner, out ownerMotions)) {
				foreach (KeyValuePair<string, IPlayback> entry in ownerMotions) {
					entry.Value.Reset();
				}
				Motions.Remove(owner);
				return true;
			} else {
				return false;
			}
		}

		private void _ClearAllMotions() {
			foreach (KeyValuePair<object, Dictionary<string, IPlayback>> ownerEntry in Motions) {
				foreach (KeyValuePair<string, IPlayback> entry in ownerEntry.Value) {
					entry.Value.Reset();
				}
				ownerEntry.Value.Clear();
			}
			Motions.Clear();
		}

		#endregion


	}
}
