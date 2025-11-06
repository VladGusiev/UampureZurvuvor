using UnityEngine;

public class GunAimer : MonoBehaviour
{
    [SerializeField] private Transform gunTransform;
    [SerializeField] private float smoothSpeed = 10f; // 0 = instant

    void LateUpdate()
    {
        if (Camera.main == null || gunTransform == null) return;

        // Aim gun at camera center point
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 aimDirection = ray.GetPoint(100f) - gunTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(aimDirection);

        // Smooth rotation
        gunTransform.rotation = smoothSpeed <= 0f 
            ? targetRotation 
            : Quaternion.Slerp(gunTransform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }
}
