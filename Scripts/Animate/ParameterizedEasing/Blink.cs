namespace CocodriloDog.Animation {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

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
					int value = (int)(t * BlinkCount * 2) % 2;
					return value == 0 ? b : a;
				};
			}
		}

		public override Motion3D.Easing Vector3Easing {
			get {
				return (a, b, t) => {
					int value = (int)(t * BlinkCount * 2) % 2;
					return value == 0 ? b : a;
				};
			}
		}

		public override MotionColor.Easing ColorEasing {
			get {
				return (a, b, t) => {
					int value = (int)(t * BlinkCount * 2) % 2;
					return value == 0 ? b : a;
				};
			}
		}

		#endregion


	}

}