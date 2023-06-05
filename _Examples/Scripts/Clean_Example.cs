namespace CocodriloDog.Animation.Examples {

	using CocodriloDog.Animation;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

	/// <summary>
	/// This was created to debug the correct functionning of the clean methods.
	/// It was debugged with logs that will no longer be there.
	/// </summary>
	[AddComponentMenu("")]
	public class Clean_Example : MonoBehaviour {


		#region Unity Methods

		private void Start() {

			// Motion
			Motion3D motion = MotionKit.GetMotion(p => transform.position = p)
				.Play(transform.position, transform.position + Vector3.up, 2)
				//.SetDuration(2)
				.SetOnStart(() => Debug.Log(" *** Motion OnStart"))
				.SetOnUpdate(() => Debug.Log(" *** Motion OnUpdate"))
				.SetOnInterrupt(() => Debug.Log(" *** Motion OnInterrupt"))
				.SetOnComplete(() => Debug.Log(" *** Motion OnComplete"));

			CleanMotion();
			void CleanMotion() {

				Debug.Log("Clean Motion Easing:");
				motion.Clean(CleanFlag.Easing);

				Debug.Log("Clean Motion OnStart:");
				motion.Clean(CleanFlag.OnStart);

				Debug.Log("Clean Motion OnUpdate:");
				motion.Clean(CleanFlag.OnUpdate);

				Debug.Log("Clean Motion OnInterrupt:");
				motion.Clean(CleanFlag.OnInterrupt);

				Debug.Log("Clean Motion OnComplete:");
				motion.Clean(CleanFlag.OnComplete);

				Debug.Log("Clean Motion Easing, OnStart, OnUpdate:");
				motion.Clean(CleanFlag.Easing | CleanFlag.OnStart | CleanFlag.OnUpdate);

				Debug.Log("Clean Motion OnUpdate, OnInterrupt, OnComplete:");
				motion.Clean(CleanFlag.OnUpdate | CleanFlag.OnInterrupt | CleanFlag.OnComplete);

				Debug.Log("Clean Motion All:");
				motion.Clean(CleanFlag.All);

			}

			// Timer
			Sequence sequence = null;
			Timer timer = MotionKit.GetTimer()
				.Play(1)
				//.SetDuration(1)
				.SetOnStart(() => Debug.Log(" *** Timer OnStart"))
				.SetOnUpdate(() => Debug.Log(" *** Timer OnUpdate"))
				.SetOnInterrupt(() => Debug.Log(" *** Timer OnInterrupt"))
				.SetOnComplete(() => {
					// Interrupt the motion
					motion.Stop();
					//sequence.Stop();
					Debug.Log(" *** Timer OnComplete");
				});

			CleanTimer();
			void CleanTimer() {

				Debug.Log("Clean Timer Easing:");
				timer.Clean(CleanFlag.Easing);

				Debug.Log("Clean Timer OnStart:");
				timer.Clean(CleanFlag.OnStart);

				Debug.Log("Clean Timer OnUpdate:");
				timer.Clean(CleanFlag.OnUpdate);

				Debug.Log("Clean Timer OnInterrupt:");
				timer.Clean(CleanFlag.OnInterrupt);

				Debug.Log("Clean Timer OnComplete:");
				timer.Clean(CleanFlag.OnComplete);

				Debug.Log("Clean Timer Easing, OnStart, OnUpdate:");
				timer.Clean(CleanFlag.Easing | CleanFlag.OnStart | CleanFlag.OnUpdate);

				Debug.Log("Clean Timer OnUpdate, OnInterrupt, OnComplete:");
				timer.Clean(CleanFlag.OnUpdate | CleanFlag.OnInterrupt | CleanFlag.OnComplete);

				Debug.Log("Clean Timer All:");
				timer.Clean(CleanFlag.All);

			}

			// Sequence
			sequence = MotionKit.GetSequence(motion, timer).Play()
				.SetOnStart(() => Debug.Log(" *** Sequence OnStart"))
				.SetOnUpdate(() => Debug.Log(" *** Sequence OnUpdate"))
				.SetOnInterrupt(() => Debug.Log(" *** Sequence OnInterrupt"))
				.SetOnComplete(() => Debug.Log(" *** Sequence OnComplete"));

			CleanSequence();
			void CleanSequence() {

				Debug.Log("Clean Sequence Easing:");
				sequence.Clean(CleanFlag.Easing);

				Debug.Log("Clean Sequence OnStart:");
				sequence.Clean(CleanFlag.OnStart);

				Debug.Log("Clean Sequence OnUpdate:");
				sequence.Clean(CleanFlag.OnUpdate);

				Debug.Log("Clean Sequence OnInterrupt:");
				sequence.Clean(CleanFlag.OnInterrupt);

				Debug.Log("Clean Sequence OnComplete:");
				sequence.Clean(CleanFlag.OnComplete);

				Debug.Log("Clean Sequence Easing, OnStart, OnUpdate:");
				sequence.Clean(CleanFlag.Easing | CleanFlag.OnStart | CleanFlag.OnUpdate);

				Debug.Log("Clean Sequence OnUpdate, OnInterrupt, OnComplete:");
				sequence.Clean(CleanFlag.OnUpdate | CleanFlag.OnInterrupt | CleanFlag.OnComplete);

				Debug.Log("Clean Sequence All:");
				sequence.Clean(CleanFlag.All);

			}

		}

		#endregion


	}

}