namespace CocodriloDog.MotionKit {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// This interpolates the initial and final values by blinking multiple times between 
	/// them.
	/// </summary>
	[Serializable]
	public class Blink : ParameterizedEasing {


		#region Public Fields

		[SerializeField]
		public int BlinkCount = 4;

		#endregion


		#region Public Properties

		public override MotionFloat.Easing FloatEasing {
			get {
				return (a, b, t) => {
					if (Mathf.Approximately(t, 1)) {
						return b;
					}
					// When t is exactly 1 this will return a that's why I put the
					// previous condition.
					int value = (int)(t * BlinkCount * 2) % 2;
					return value == 0 ? a : b;
				};
			}
		}

		public override Motion3D.Easing Vector3Easing {
			get {
				return (a, b, t) => {
					if (Mathf.Approximately(t, 1)) {
						return b;
					}
					int value = (int)(t * BlinkCount * 2) % 2;
					return value == 0 ? a : b;
				};
			}
		}

		public override MotionColor.Easing ColorEasing {
			get {
				return (a, b, t) => {
					if (Mathf.Approximately(t, 1)) {
						return b;
					}
					int value = (int)(t * BlinkCount * 2) % 2;
					return value == 0 ? a : b;
				};
			}
		}

		#endregion


		#region Public Constructors

		public Blink() { }

		public Blink(int blinkCount) {
			BlinkCount = blinkCount;
		}

		#endregion


		#region Public Methods

		public override ParameterizedEasing Copy() => new Blink(BlinkCount);

		#endregion


	}

}