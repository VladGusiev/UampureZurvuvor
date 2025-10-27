using UnityEngine;

public class GunAimer : MonoBehaviour
{
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Transform muzzleTransform;
    [SerializeField] private float smoothTime = 0.03f; // 0 = instant

    [SerializeField] private bool clampPitch = false;
    [SerializeField] private float minPitch = -60f;
    [SerializeField] private float maxPitch = 60f;

    void Reset()
    {
        if (gunTransform == null) gunTransform = transform;
        if (muzzleTransform == null) muzzleTransform = gunTransform;
    }

    void LateUpdate()
    {
        var cam = Camera.main;
        if (cam == null || gunTransform == null) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 origin = (muzzleTransform != null) ? muzzleTransform.position : gunTransform.position;
        Vector3 target = ray.GetPoint(1000f); // far point along center ray

        Vector3 dir = (target - origin);
        if (dir.sqrMagnitude < 1e-6f) return;

        Quaternion lookRot = Quaternion.LookRotation(dir.normalized, Vector3.up);

        if (clampPitch)
        {
            Vector3 e = lookRot.eulerAngles;
            float pitch = e.x > 180f ? e.x - 360f : e.x;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            e.x = pitch;
            lookRot = Quaternion.Euler(e);
        }

        if (smoothTime <= 0f)
            gunTransform.rotation = lookRot;
        else
        {
            float t = 1f - Mathf.Exp(-Time.deltaTime / Mathf.Max(0.0001f, smoothTime));
            gunTransform.rotation = Quaternion.Slerp(gunTransform.rotation, lookRot, t);
        }
    }
}
