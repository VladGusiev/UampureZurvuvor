using UnityEngine;

/// <summary>
/// Simple damageable interface. Any object that can take damage should implement this.
/// </summary>
public interface IDamageable
{
    void TakeDamage(float amount);
}
