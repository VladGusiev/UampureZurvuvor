using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Basic hitscan shooting using the camera center (viewport 0.5,0.5). Designed for use with Cinemachine.
/// - Uses the new Input System via an InputActionReference (Fire action).
/// - Performs a Raycast from the camera center and applies damage to IDamageable.
/// - Can optionally spawn a projectile prefab instead of hitscan.
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [Header("Input")]
    [Tooltip("Reference to a Button action (Fire). Use performed callback or press/release.)")]
    [SerializeField] private InputActionReference fireAction;

    [Header("Hitscan")]
    [SerializeField] private float damage = 25f;
    [SerializeField] private float range = 200f;
    [SerializeField] private LayerMask hitMask = ~0; // everything by default

    [Header("Rate & Recoil")]
    [SerializeField] private float fireRate = 0.12f; // seconds between shots

    [Header("Visuals")]
    [SerializeField] private Transform muzzleTransform; // optional
    [SerializeField] private ParticleSystem muzzleFlashPrefab;
    [SerializeField] private GameObject impactPrefab;

    [Header("Projectile (optional)")]
    [SerializeField] private bool useProjectile = false;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 40f;

    private float nextFireTime = 0f;

    void OnEnable()
    {
        if (fireAction != null) fireAction.action.Enable();
        if (fireAction != null) fireAction.action.performed += OnFirePerformed;
    }

    void OnDisable()
    {
        if (fireAction != null) fireAction.action.performed -= OnFirePerformed;
        if (fireAction != null) fireAction.action.Disable();
    }

    private void OnFirePerformed(InputAction.CallbackContext ctx)
    {
        TryFire();
    }

    private void TryFire()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;

        // Visuals
        if (muzzleTransform != null && muzzleFlashPrefab != null)
        {
            var flash = Instantiate(muzzleFlashPrefab, muzzleTransform.position, muzzleTransform.rotation);
            flash.Play();
            Destroy(flash.gameObject, 2f);
        }

        // Determine shot origin and direction from camera center
        var cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (useProjectile && projectilePrefab != null && muzzleTransform != null)
        {
            // Spawn projectile from muzzle and give velocity towards ray direction
            var go = Instantiate(projectilePrefab, muzzleTransform.position, Quaternion.LookRotation(ray.direction));
            if (go.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.linearVelocity = ray.direction * projectileSpeed;
            }
            return;
        }

        if (Physics.Raycast(ray, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Ignore))
        {
            // Instantiate impact visual
            if (impactPrefab != null)
            {
                var impact = Instantiate(impactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 4f);
            }

            // Apply damage if target supports IDamageable
            var dmg = hit.collider.GetComponentInParent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
            }
        }
    }
}
