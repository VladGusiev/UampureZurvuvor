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
            GameObject projectile = Instantiate(enemyStats.attackProjectilePrefab, enemyStats.projectileSpawnPoint.position, Quaternion.identity);
            Vector3 direction = (player.transform.position - enemyStats.projectileSpawnPoint.position).normalized;
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            if (projectileRb != null)
            {
                projectileRb.linearVelocity = direction * enemyStats.projectileSpeed;
            }
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
            // Use VelocityChange so knockback is noticeable regardless of player mass
            playerRb.AddForce(knockbackDirection * enemyStats.knockbackForce, ForceMode.VelocityChange);
        }

        lastAttackTime = Time.time;
    }

    void Update()
    {
        // if player is not in range, return
        if (player == null || enemyStats == null) return;
        if (Vector3.Distance(transform.position, player.transform.position) > enemyStats.attackRange) return;
        if (Time.time - lastAttackTime < enemyStats.attackInterval) return;

        // Perform attacks
        if (enemyStats.enemyAttackType == EnemyAttackType.Ranged) {
            TryRangedAttack();
            return;
        }
        TryKnockbackAttack();        
    }
}
