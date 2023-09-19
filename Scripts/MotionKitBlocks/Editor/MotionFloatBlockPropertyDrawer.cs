namespace CocodriloDog.MotionKit {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(MotionFloatBlock))]
	public class MotionFloatBlockPropertyDrawer : MotionBlockPropertyDrawer<float> { }

}