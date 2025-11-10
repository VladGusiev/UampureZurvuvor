using UnityEngine;

public class BeamProjectileFunctionality : MonoBehaviour
{
    private Rigidbody rb;
    private EnemyProjectileStats projectileStats;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) return;

        projectileStats = GetComponent<EnemyProjectileStats>();

        rb.linearVelocity = transform.forward * projectileStats.projectileSpeed;
    }
    
    void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
            return;
        }
        
        // Knockback player
        Rigidbody playerRb = other.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 knockbackDirection = transform.forward;
            playerRb.AddForce(knockbackDirection * projectileStats.knockbackForce, ForceMode.VelocityChange);
        }
        
        Destroy(gameObject); // Destroy proojectile on hit
    }
}
