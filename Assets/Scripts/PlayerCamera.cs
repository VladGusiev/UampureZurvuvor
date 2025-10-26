using UnityEngine;

/// <summary>
/// Minimal camera helper for use with Cinemachine.
///
/// Responsibilities:
/// - Optionally lock the cursor.
/// - Read the runtime (main) Camera's forward vector and align the player's yaw
///   and the movement "orientation" to match the camera yaw (so movement follows camera direction).
///
/// Important: Cinemachine should handle input (via its own Input Provider or bindings).
/// This class does NOT read look input anymore — it only follows the final camera orientation.
/// </summary>
public class PlayerCamera : MonoBehaviour
{
	[Header("Targets")]
	[Tooltip("The player's body to rotate with camera yaw (usually the player root or model root)")]
	[SerializeField] private Transform playerBody;

	[Tooltip("Movement orientation reference (used by PlayerMovement). Will be aligned to camera yaw")]
	[SerializeField] private Transform orientation;

	[Header("Smoothing")]
	[Tooltip("Rotation smoothing factor. 0 = no smoothing, higher = smoother (0.05–0.2 is typical)")]
	[SerializeField] private float smoothing = 0.08f;

	[Header("Cursor")]
	[SerializeField] private bool lockCursor = true;

	void Reset()
	{
		playerBody = transform;
		if (orientation == null) orientation = transform;
	}

	void Start()
	{
		if (lockCursor)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void Update()
	{
		var cam = Camera.main;
		if (cam == null) return; // no camera present

		// Project camera forward onto XZ plane to get yaw direction
		Vector3 camForward = cam.transform.forward;
		camForward.y = 0f;
		if (camForward.sqrMagnitude < 1e-6f) return;

		Quaternion targetYaw = Quaternion.LookRotation(camForward.normalized, Vector3.up);

		float t = smoothing <= 0f ? 1f : 1f - Mathf.Exp(-Time.deltaTime / Mathf.Max(0.0001f, smoothing));

		if (orientation != null)
			orientation.rotation = Quaternion.Slerp(orientation.rotation, targetYaw, t);

		if (playerBody != null)
			playerBody.rotation = Quaternion.Slerp(playerBody.rotation, targetYaw, t);
	}
}
