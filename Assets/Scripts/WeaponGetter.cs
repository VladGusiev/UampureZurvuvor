using UnityEngine;

public class WeaponGetter : MonoBehaviour
{

    [SerializeField] GameObject ARPrefab;
    [SerializeField] GameObject MinigunPrefab;
    // When object with player tag collides with this object, give player the weapon
    // place weapon inside player child obect "WeaponHolder"
    public void ProvideWeapon(WeaponType weaponType)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Transform weaponHolder = player.transform.Find("WeaponHolder");

        foreach (Transform child in weaponHolder)
        {
            GameObject.Destroy(child.gameObject);
        }

        switch (weaponType)
        {
            case WeaponType.AssaultRifle:
                Instantiate(ARPrefab, weaponHolder);
                break;
            case WeaponType.Minigun:
                Instantiate(MinigunPrefab, weaponHolder);
                break;
            default:
                Debug.Log("No valid weapon type provided");
                break;
        }
    }
}
