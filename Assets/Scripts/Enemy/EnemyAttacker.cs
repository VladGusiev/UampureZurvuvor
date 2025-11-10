using UnityEngine;

public class EnemyAttacker : MonoBehaviour
{
    private EnemyStats enemyStats;
    private GameObject player;
    private float lastAttackTime = 0f;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void TryRangedAttack() {
        if (enemyStats.attackProjectilePrefab != null && enemyStats.projectileSpawnPoint != null)
        {
            // Rotate enemy toward player
            gameObject.transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);

            // Spawn range projectile
            GameObject beam = Instantiate(
                enemyStats.attackProjectilePrefab,
                enemyStats.projectileSpawnPoint.position,
                enemyStats.projectileSpawnPoint.rotation
            );
            
            Vector3 direction = (player.transform.position - enemyStats.projectileSpawnPoint.position).normalized;
            beam.transform.rotation = Quaternion.LookRotation(direction);
            

        }
        lastAttackTime = Time.time;
    }
     
    private void TryKnockbackAttack()
    {
        Vector3 knockbackDirection = player.transform.position - transform.position;
        knockbackDirection.Normalize();

        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.AddForce(knockbackDirection * enemyStats.knockbackForce, ForceMode.VelocityChange);
        }

        lastAttackTime = Time.time;
    }

    void Update()
    {
        if (player == null || enemyStats == null) return;
        if (Vector3.Distance(transform.position, player.transform.position) > enemyStats.attackRange) return;
        if (Time.time - lastAttackTime < enemyStats.attackInterval) return;

        if (enemyStats.enemyAttackType == EnemyAttackType.Ranged) {
            TryRangedAttack();
            return;
        }
        TryKnockbackAttack();        
    }
}
