using UnityEngine;

public class WeaponDropBox : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    
    // Set the weapon type for this dropbox
    public void SetWeapon(WeaponType weapon)
    {
        weaponType = weapon;
    }

    // When player collides with the dropbox
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Find the WeaponGetter and give the weapon
            WeaponGetter weaponGetter = FindObjectOfType<WeaponGetter>();
            if (weaponGetter != null)
            {
                weaponGetter.ProvideWeapon(weaponType);
            }
            
            // Destroy the dropbox after collection
            Destroy(gameObject);
        }
    }
}
