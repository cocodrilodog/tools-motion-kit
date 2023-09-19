namespace CocodriloDog.MotionKit {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(MotionColorBlock))]
	public class MotionColorBlockPropertyDrawer : MotionBlockPropertyDrawer<Color> { }

}