using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    private EnemyStats enemyStats;
    private GameObject player;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyStats = GetComponent<EnemyStats>();
    }

    private void MoveToPlayer()
    {
        if (player == null || enemyStats == null) return;

        if (Vector3.Distance(transform.position, player.transform.position) <= enemyStats.attackRange) {
            return;
        }

        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 move = direction * enemyStats.speed * Time.deltaTime;

        transform.position += new Vector3(move.x, 0, move.z);

    }

    void Update()
    {
        MoveToPlayer();
    }
}
 