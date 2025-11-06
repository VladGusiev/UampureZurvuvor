using UnityEngine;

public class DropBoxSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dropBoxPrefab;
    [SerializeField] private float dropBoxSpawnHeight = 25f;
    [SerializeField] private float dropBoxSpawnRadius = 10f;

    public void SpawnDropBox(WeaponType weaponType)
    {
        // Find player object
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        if (dropBoxPrefab == null)
        {
            Debug.LogError("DropBox prefab is not assigned!");
            return;
        }

        // Calculate spawn position near player
        Vector3 playerPos = player.transform.position;
        Vector3 randomOffset = new Vector3(
            Random.Range(-dropBoxSpawnRadius, dropBoxSpawnRadius),
            0,
            Random.Range(-dropBoxSpawnRadius, dropBoxSpawnRadius)
        );
        Vector3 spawnPosition = playerPos + randomOffset + Vector3.up * dropBoxSpawnHeight;

        // Instantiate the dropbox
        GameObject dropBox = Instantiate(dropBoxPrefab, spawnPosition, Quaternion.identity);
        
        // Configure the dropbox with the selected weapon
        WeaponDropBox dropBoxScript = dropBox.GetComponent<WeaponDropBox>();
        if (dropBoxScript != null)
        {
            dropBoxScript.SetWeapon(weaponType);
        }
        else
        {
            Debug.LogError("WeaponDropBox script not found on dropbox prefab!");
        }
    }
}
