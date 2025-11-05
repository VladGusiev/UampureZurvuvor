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

        // if number 1 is selected, give player an assault rifle if 2, minigun 
        WeaponGetter weaponGetter = FindObjectOfType<WeaponGetter>();
        if (weaponGetter != null)
        {
            switch (index)
            {
                case 0:
                    weaponGetter.ProvideWeapon(WeaponType.AssaultRifle);
                    break;
                case 1:
                    weaponGetter.ProvideWeapon(WeaponType.Minigun);
                    break;
                default:
                    Debug.Log("No valid option selected");
                    break;
            }
        }
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
