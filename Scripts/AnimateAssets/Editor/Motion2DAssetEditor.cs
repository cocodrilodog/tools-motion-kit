namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(Motion2DAsset))]
	public class Motion2DAssetEditor : MotionBaseAssetEditor<Vector2> { }

}