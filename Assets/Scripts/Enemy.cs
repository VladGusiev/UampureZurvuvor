using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] private float hp = 100f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackInterval = 1.5f;
    [SerializeField] private float knockbackForce = 5f;


    private float lastAttackTime = 0f;
    private GameObject player;
    private Rigidbody rb;



    public void TakeDamage(float amount)
    {
        hp -= amount; if (hp <= 0) Destroy(gameObject);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (player == null || rb == null) return;

        // Calculate horizontal direction to player and preserve current vertical velocity
        Vector3 toPlayer = player.transform.position - transform.position;
        Vector3 direction = toPlayer.normalized;

        // Set velocity directly for simple, responsive movement.
        // Preserve rb.velocity.y so gravity or jumps aren't overridden.
        rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, direction.z * speed);
    }

}