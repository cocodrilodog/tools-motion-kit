namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	[CustomEditor(typeof(Motion3DAsset))]
	public class Motion3DAssetEditor : MotionBaseAssetEditor<Vector3> { }

}