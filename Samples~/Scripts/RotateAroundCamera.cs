namespace CocodriloDog.MotionKit.Examples {

	using CocodriloDog.Core;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class RotateAroundCamera : MonoBehaviour {


		#region Public Fields

		[SerializeField]
		public Transform Target;

		[SerializeField]
		public bool EnableYRotation = true;

		[SerializeField]
		public float MouseXMultiplier = 0.2f;

		[SerializeField]
		public bool EnableXRotation = true;

		[SerializeField]
		public float MouseYMultiplier = 0.2f;

		[SerializeField]
		public bool EnableZoom = true;

		[SerializeField]
		public float ZoomSpeed = 2;

		[SerializeField]
		public FloatRange DistanceToTargetRange = new FloatRange(1, Mathf.Infinity);

		#endregion


		#region Public Methods

		public void RotateAroundTargetOnLocalX(float angle) {
			transform.RotateAround(
				Target.position,
				transform.TransformPoint(Vector3.right) - transform.position,
				angle
			);
		}

		public void RotateAroundTargetY(float angle) {
			transform.RotateAround(Target.position, Vector3.up, angle);
		}

		public void ZoomTarget(float zoomAmount) {
			transform.position = Vector3.MoveTowards(
				transform.position, Target.position, zoomAmount
			);
			float distanceToTarget = (Target.position - transform.position).magnitude;
			if (distanceToTarget < DistanceToTargetRange.MinValue) {
				Vector3 direction = (transform.position - Target.position).normalized;
				transform.position = Target.position + direction * DistanceToTargetRange.MinValue;
			}
			if (distanceToTarget > DistanceToTargetRange.MaxValue) {
				Vector3 direction = (transform.position - Target.position).normalized;
				transform.position = Target.position + direction * DistanceToTargetRange.MaxValue;
			}
		}

		#endregion


		#region Unity Methods

		private void Start() {
			transform.LookAt(Target.position);
		}

		private void Update() {
			RotateAroundTarget();
			ZoomTarget();
		}

		private void OnValidate() {
			DistanceToTargetRange.MinValue = Mathf.Max(DistanceToTargetRange.MinValue, 0.5f);
		}

		private void Reset() {
			EnableYRotation = true;
			MouseXMultiplier = 0.2f;
			EnableXRotation = true;
			MouseYMultiplier = 0.2f;
			EnableZoom = true;
			ZoomSpeed = 2;
			DistanceToTargetRange = new FloatRange(1, Mathf.Infinity);
		}

		#endregion


		#region Private Methods

		[NonSerialized]
		private bool m_IsMouseDown;

		[NonSerialized]
		private Vector3 m_LastMouseDownPosition;

		#endregion


		#region Private Methods

		private void RotateAroundTarget() {
			if (Input.GetMouseButtonDown(0)) {
				m_IsMouseDown = true;
				m_LastMouseDownPosition = Input.mousePosition;
			}
			if (Input.GetMouseButtonUp(0)) {
				m_IsMouseDown = false;
			}
			if (m_IsMouseDown) {
				Vector2 mouseDelta = Input.mousePosition - m_LastMouseDownPosition;
				if (EnableXRotation) {
					RotateAroundTargetOnLocalX(-mouseDelta.y * MouseYMultiplier);
				}
				if (EnableYRotation) {
					RotateAroundTargetY(mouseDelta.x * MouseXMultiplier);
				}
				m_LastMouseDownPosition = Input.mousePosition;
			}
		}

		private void ZoomTarget() {
			if (EnableZoom) {
				float vertical = Input.GetAxis("Vertical");
				ZoomTarget(ZoomSpeed * Time.deltaTime * vertical);
			}
		}

		#endregion


	}

}