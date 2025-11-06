using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpOptions : MonoBehaviour
{
    [SerializeField] private Button[] optionButtons;
    [SerializeField] private TextMeshProUGUI[] optionTexts;

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowLevelUpOptions(string[] options)
    {
        gameObject.SetActive(true);

        UnlockCursor();

        Time.timeScale = 0f; // Pause the game

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionTexts[i].text = options[i];
                int index = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }
    private void OnOptionSelected(int index)
    {
        // Determine which weapon was selected
        WeaponType selectedWeapon = WeaponType.AssaultRifle; // Default
        
        switch (index)
        {
            case 0:
                selectedWeapon = WeaponType.AssaultRifle;
                break;
            case 1:
                selectedWeapon = WeaponType.Minigun;
                break;
            default:
                Debug.Log("No valid option selected");
                break;
        }

        DropBoxSpawner dropBoxSpawner = FindObjectOfType<DropBoxSpawner>();
        if (dropBoxSpawner != null)
        {
            dropBoxSpawner.SpawnDropBox(selectedWeapon);
        }
        else
        {
            Debug.LogError("DropBoxSpawner not found in the scene!");
        }
        
        
        // Hide menu and resume game
        gameObject.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        LockCursor();
    }

    public void UnlockCursor()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
