using UnityEngine;

public class EnemyStats : MonoBehaviour {

    [SerializeField] public float hp = 100f;
    [SerializeField] public float speed = 3f;
    [SerializeField] public float damage = 10f;
    [SerializeField] public float attackRange = 5f;
    [SerializeField] public float attackInterval = 1.5f;
    [SerializeField] public float knockbackForce = 5f;
    [SerializeField] public int experienceValue = 10;
    [SerializeField] public EnemyAttackType enemyAttackType;
    [SerializeField] public EnemyMovementType enemyMovementType;

    [Header("Ranged Units Parameters")]
    [SerializeField] public GameObject attackProjectilePrefab;
    [SerializeField] public float projectileSpeed = 10f;
    [SerializeField] public Transform projectileSpawnPoint;

    private GameObject player;

    public void TakeDamage(float amount)
    {
        hp -= amount; 
        if (hp <= 0) {
            GiveExperience(player.GetComponent<PlayerLeveling>());
            Destroy(gameObject);
        }
    }

    public void GiveExperience(PlayerLeveling playerLeveling)
    {
        if (playerLeveling != null)
        {
            playerLeveling.AddExperience(experienceValue);
        }
    }
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

}