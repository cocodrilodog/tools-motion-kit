namespace CocodriloDog.MotionKit.Examples {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Use this script in its corresponding scene as a guide to getting started with 
	/// <see cref="MotionKit"/>. Have fun!
	/// </summary>
	[AddComponentMenu("")]
	public class GettingStarted : MonoBehaviour {


		private void Start() {

			m_TitleRenderer.SetAlpha(0); // Set alpha to 0 first

			// Fade in the title: Alpha from 0 to 1 during 2 seconds
			// Register the motion with owner m_TitleRenderer and id "Alpha"
			MotionKit.GetMotion(m_TitleRenderer, "Alpha", a => m_TitleRenderer.SetAlpha(a))
				.Play(0, 1, 2);

			// Move the ball from -6, 0, 0 to -6, 3, 0 during 2 seconds
			// Register the motion with owner m_BallMove and id "Position"
			MotionKit.GetMotion(m_BallMove, "Position", p => m_BallMove.transform.localPosition = p)
				.Play(new Vector3(-6, 0, 0), new Vector3(-6, 3, 0), 2);

			var colorAtStart = m_BallColor.Color;	// Save the current color
			m_BallColor.Color = Color.black;		// Set color to black first

			// Color the ball from black to its color at start (yellow) during 2 seconds
			// Register the motion with owner m_BallColor and id "Color"
			MotionKit.GetMotion(m_BallColor, "Color", c => m_BallColor.Color = c)
				.Play(Color.black, colorAtStart, 2);

			// Up scale the ball from 1, 1, 1 to 2, 2, 2 during 2 seconds
			// Register the motion with owner m_BallScale and id "Scale"
			MotionKit.GetMotion(m_BallScale, "Scale", s => m_BallScale.transform.localScale = s)
				.Play(Vector3.one, Vector3.one * 2, 2);

			// Schedule a timer to complete after 1 second.
			// This will start a downscale motion before the up scale motion ends.
			// It will gracefully interrupt the up scale
			MotionKit.GetTimer(this, "DownScaleTimer").Play(1).SetOnComplete(DownScale);

			void DownScale() {
				// Down scale the ball: From its current value to 1, 1, 1 during 1 second
				// Register the motion with the same owner and id as the up scale motion.
				//
				// This will reuse the same motion object from the up scale because it shares
				// the owner and id. Therefore, the up scale motion stops and the down scale motion starts
				MotionKit.GetMotion(m_BallScale, "Scale", s => m_BallScale.transform.localScale = s)
					.Play(m_BallScale.localScale, Vector3.one, 1);
			}

			// Move the ball with two sequential motions. First down, then left.
			MotionKit.GetSequence(m_BallSequence, "MoveSequence",
				MotionKit.GetMotion(p => m_BallSequence.localPosition = p).SetValuesAndDuration(new Vector3(3, 0, 0), new Vector3(3, -3, 0), 1),
				MotionKit.GetMotion(p => m_BallSequence.localPosition = p).SetValuesAndDuration(new Vector3(3, -3, 0), new Vector3(0, -3, 0), 1)
			).Play();

			// Move the ball with two sequential motions, but this time, go pro!
			// This time, we don't use hardcoded values, but the motion is set to start in the ball's current position
			// each time and move relative to that position. Additionally we are adding easing curves to make the movement
			// look more interesting.
			MotionKit.GetSequence(m_BallSequencePro, "MoveSequence",

				MotionKit.GetMotion(p => m_BallSequencePro.localPosition = p)
					.SetDuration(1)
					.SetEasing(MotionKitEasing.BackOut) // <- Use a curve for fancier motions
					.SetOnStart(m => SetMotionValues(m, Vector3.up * 3)),   // <- See what is happenning in SetMotionValues()

				MotionKit.GetMotion(p => m_BallSequencePro.localPosition = p)
					.SetDuration(1)
					.SetEasing(MotionKitEasing.BackOut)
					.SetOnStart(m => SetMotionValues(m, Vector3.left * 3))

			).Play();

			// This is invoked when the motion starts and receives the corresponding motion as a parameter
			void SetMotionValues(Motion3D motion3D, Vector3 delta) {
				// The motion starts at its current position
				motion3D.SetInitialValue(m_BallSequencePro.localPosition);
				// The motion ends at certain position, relative to its current position
				motion3D.SetFinalValue(m_BallSequencePro.localPosition + delta);
			}

		}

		private void OnDestroy() {
			// Dispose the motions that were registered
			MotionKit.ClearPlaybacks(m_TitleRenderer);
			MotionKit.ClearPlaybacks(m_BallMove);
			MotionKit.ClearPlaybacks(m_BallColor);
			MotionKit.ClearPlaybacks(m_BallScale);
			MotionKit.ClearPlaybacks(m_BallSequence);
			MotionKit.ClearPlaybacks(m_BallSequencePro);
		}

		#region Private Fields

		[SerializeField]
		private CanvasRenderer m_TitleRenderer;

		[SerializeField]
		private Transform m_BallMove;

		[SerializeField]
		private ColorAdapter m_BallColor;

		[SerializeField]
		private Transform m_BallScale;

		[SerializeField]
		private Transform m_BallSequence;

		[SerializeField]
		private Transform m_BallSequencePro;

		#endregion


	}

}