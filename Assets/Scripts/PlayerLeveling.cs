using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.
using TMPro;

public class PlayerLeveling : MonoBehaviour
{
    [SerializeField] private int currentExperience = 0;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int experienceToLevelUp = 100;
    [SerializeField] private Image xpBarImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private LevelUpOptions levelUpOptions;

    public void Start()
    {
        if (xpBarImage != null)
        {
            xpBarImage.fillAmount = (float)currentExperience / experienceToLevelUp;
            levelText.text = "Lvl. " + currentLevel.ToString();
        }
    }
    public void AddExperience(int amount)
    {
        // Add experience and check for level up
        currentExperience += amount;
        if (currentExperience >= experienceToLevelUp)
        {
            LevelUp();
        }

        xpBarImage.fillAmount = (float)currentExperience / experienceToLevelUp;
    }

    private void LevelUp()
    {
        currentLevel++;
        currentExperience = 0;
        experienceToLevelUp = Mathf.RoundToInt(experienceToLevelUp * 1.3f);

        string levelStr = "Lvl. " + currentLevel.ToString();
        levelText.text = levelStr;

        // find object with name "LevelUpPanel" and access script LevelUpOptions to show level up options
        if (levelUpOptions != null) levelUpOptions.ShowLevelUpOptions(new string[] { "Assault Rifle", "Minigun" });
    }
    
}
