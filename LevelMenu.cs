using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;

    public GameObject[] starImagesForLvl1;
    public GameObject[] starImagesForLvl2;
    public GameObject[] starImagesForLvl3;
    public GameObject[] starImagesForLvl4;
    public GameObject[] starImagesForLvl5;
    public GameObject[] starImagesForLvl6;
    public GameObject[] starImagesForLvl7;
    public GameObject[] starImagesForLvl8;
    public GameObject[] starImagesForLvl9;
    public GameObject[] starImagesForLvl10;
    public GameObject[] starImagesForLvl11;

    public bool[,] starsActivated = new bool[11, 3]; // 2D boolean array for stars

    public int unlockedLevel;
    public Dictionary<int, GameObject[]> levelToStarImages = new Dictionary<int, GameObject[]>();

    private void Awake()
    {
        // Load starsActivated array from PlayerPrefs
        LoadStarsActivated();

        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        Debug.Log("Current Level: " + currentLevel);
        int starsEarned = PlayerPrefs.GetInt("StarsEarned_" + currentLevel, 0);
        Debug.Log("Stars Earned: " + starsEarned);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }

        // Example: Associate star images for each level
        levelToStarImages.Add(1, starImagesForLvl1);
        levelToStarImages.Add(2, starImagesForLvl2);
        levelToStarImages.Add(3, starImagesForLvl3);
        levelToStarImages.Add(4, starImagesForLvl4);
        levelToStarImages.Add(5, starImagesForLvl5);
        levelToStarImages.Add(6, starImagesForLvl6);
        levelToStarImages.Add(7, starImagesForLvl7);
        levelToStarImages.Add(8, starImagesForLvl8);
        levelToStarImages.Add(9, starImagesForLvl9);
        levelToStarImages.Add(10, starImagesForLvl10);
        levelToStarImages.Add(11, starImagesForLvl11);
    // Add more entries for other levels

    // Update PlayerPrefs key for stars earned for the current level
        PlayerPrefs.SetInt("StarsEarned_Level_" + currentLevel, starsEarned);

        ActivateStars(currentLevel, starsEarned);
    }

    private void ActivateStars(int level, int starsEarned)
    {
        if (levelToStarImages.ContainsKey(level))
        {
            int levelIndex = level - 1;
            GameObject[] starImages = levelToStarImages[level];

            // Deactivate all star images first
            for (int i = 0; i < starImages.Length; i++)
            {
                starImages[i].SetActive(starsActivated[levelIndex, i]);
            }

            // Activate stars based on starsEarned
            for (int i = 0; i < starsEarned && i < starImages.Length; i++)
            {
                starImages[i].SetActive(true);
                // Update the starsActivated array
                starsActivated[levelIndex, i] = true;
                // Save the active state to PlayerPrefs
                PlayerPrefs.SetInt($"Star{i + 1}Active_Level_{level}", 1);
            }

            // Activate the correct star image for each activated star
            for (int k = 0; k < 11; k++)
            {
                for (int r = 0; r < 3; r++)
                {
                    if (starsActivated[k, r])
                    {
                        // Activate the correct star image for that level and star
                        levelToStarImages[k + 1][r].SetActive(true);
                    }
                }
            }

            // Save PlayerPrefs changes
            PlayerPrefs.Save();
        }
    }

    private void LoadStarsActivated()
    {
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                starsActivated[i, j] = PlayerPrefs.GetInt($"StarsActivated_Level_{i + 1}_Star{j + 1}", 0) == 1;
            }
        }
    }

    private void SaveStarsActivated()
    {
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                PlayerPrefs.SetInt($"StarsActivated_Level_{i + 1}_Star{j + 1}", starsActivated[i, j] ? 1 : 0);
            }
        }
    }

    private int GetCurrentLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        int levelNumber;
        if (int.TryParse(sceneName.Replace("Level ", ""), out levelNumber))
        {
            return levelNumber;
        }

        return 0;
    }

    public void OpenLevel(int levelId)
    {
        // Save starsActivated array before changing scenes
        SaveStarsActivated();

        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelName);
    }

}
