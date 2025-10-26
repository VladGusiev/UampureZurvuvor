using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable {
    public float hp = 100;
    public void TakeDamage(float amount) {
        hp -= amount; if (hp <= 0) Destroy(gameObject);
    }
}