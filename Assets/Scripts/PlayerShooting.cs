using UnityEngine;
using UnityEngine.InputSystem;


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
    [SerializeField] private GameObject enemyHitEffectPrefab;
    [SerializeField] private GameObject terrainHitEffectPrefab;

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
            Destroy(flash.gameObject, 1f);
        }

        // Determine shot origin and direction from camera center
        var cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Ignore))
        {
            // Instantiate impact visual
            if (enemyHitEffectPrefab != null && hit.collider.CompareTag("Enemy"))
            {
                var impact = Instantiate(enemyHitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 1f);
            }
            else if (terrainHitEffectPrefab != null)
            {
                var impact = Instantiate(terrainHitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 1f);
            }

            var enemyComponent = hit.collider.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(damage);
            }
        }
    }

    void Update()
    {
        if(fireAction != null && fireAction.action.IsPressed())
        {
            TryFire();
        }
    }
}
