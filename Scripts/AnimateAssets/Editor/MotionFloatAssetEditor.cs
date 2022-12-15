namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(MotionFloatAsset))]
	public class MotionFloatAssetEditor : MotionBaseAssetEditor<float, MotionFloat> { }

}