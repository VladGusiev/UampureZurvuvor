using UnityEngine;

public class EnemySharedFunctionality : MonoBehaviour
{
    private GameObject player;
    private EnemyStats enemyStats;

    void Start() {
        enemyStats = GetComponent<EnemyStats>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void TakeDamage(float amount)
    {
        enemyStats.hp -= amount; 
        if (enemyStats.hp <= 0) {
            GiveExperience(player.GetComponent<PlayerLeveling>());
            Destroy(gameObject);
        }
    }

    public void GiveExperience(PlayerLeveling playerLeveling)
    {
        if (playerLeveling != null)
        {
            playerLeveling.AddExperience(enemyStats.experienceValue);
        }
    }

}
