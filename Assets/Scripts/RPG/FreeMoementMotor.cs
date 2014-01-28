using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class FreeMoementMotor : MovementMotor {

	public float walkingSpeed = 5.0f;
	public float walkingSnappyness = 50.0f;
	public float turningSmoothing  = 0.3f;

	void FixedUpdate() {
	
		Vector3 targetVelocity = movementDirection * walkingSpeed;
		Vector3 deltaVelocity  = targetVelocity - rigidbody.velocity;

		if (rigidbody.useGravity)
			deltaVelocity.y = 0.0f;
		
		
		rigidbody.AddForce (deltaVelocity * walkingSnappyness, ForceMode.Acceleration);

		Vector3 faceDir = facingDirection;

		if (faceDir == Vector3.zero)
			faceDir = movementDirection;
		
		if (faceDir == Vector3.zero) {
			rigidbody.angularVelocity = Vector3.zero;
		}
		else {
			float rotationAngle = AngleAroundAxis (transform.forward, faceDir, Vector3.up);
			rigidbody.angularVelocity = (Vector3.up * rotationAngle * turningSmoothing);
		}
		
	}

	float AngleAroundAxis (Vector3 dirA, Vector3 dirB, Vector3 axis) {
		// Project A and B onto the plane orthogonal target axis
		dirA = dirA - Vector3.Project (dirA, axis);
		dirB = dirB - Vector3.Project (dirB, axis);
		
		// Find (positive) angle between A and B
		float angle = Vector3.Angle (dirA, dirB);
		
		// Return angle multiplied with 1 or -1
		return angle * (Vector3.Dot (axis, Vector3.Cross (dirA, dirB)) < 0.0f ? -1.0f : 1.0f);
	}
}
