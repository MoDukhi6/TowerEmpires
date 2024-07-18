using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class EnemyPrefabConfig
{
    [Header("Enemy Prefab Settings")]
    [Tooltip("The enemy prefab to be spawned.")]
    public GameObject enemyPrefab;

    [Header("Enemies Per Wave")]
    [Tooltip("Specify the number of enemies to spawn for each wave.")]
    public int[] enemiesPerWave;
}


public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyPrefabConfig[] enemyPrefabsConfigPath1;
    [SerializeField] private EnemyPrefabConfig[] enemyPrefabsConfigPath2;
    [SerializeField] CurencyScript currScript;
    [SerializeField] CashCounter cashScript;

    LevelMusic lvlMusic;

    [Header("Attributes")]
    //[SerializeField] private int baseEnemies;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    //[SerializeField] private float difficultyScalingFactor = 5f;
    [SerializeField] private Text waveNumTxt;
    [SerializeField] private Text waveLivesTxt;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject victoryMenu1star;
    [SerializeField] private GameObject victoryMenu2stars;
    [SerializeField] private GameObject victoryMenu3stars;
    [SerializeField] private int MaxWaveNum;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    private int tempCounter = 0;
    public int currentWave = 0;
    private string myString;
    private string myStringForLives;
    private float timeSinceLastSpawn;
    private int enemyNumCount = 0;
    public int enemiesAlive;
    public int enemiesLeftToSpawn;
    private bool isSpawning = false;
    private int EDestroyedCount = 0;
    public int EnemyLeftToLose = 20;
    private bool gameOver = false;
    private bool victory = false;
    private int coinsToAdd;
    private bool flagToTake = false;
    private bool canSayNextLevel = true;
    private int prefabIndex = 0;
    private int prefabIndex2 = 0;
    public int enemiesSpawned = 0;
    public int enemiesSpawned2 = 0;
    private int sum = 0;
    private bool nextLevelUnlocked = false;
    private int lvlUnlockedCount = 0;

    private int[,] enemiesPerWavePath1;
    private int[,] enemiesPerWavePath2;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
        lvlMusic = GameObject.FindGameObjectWithTag("Audio").GetComponent<LevelMusic>();
        InitializeEnemiesPerWave();
    }

    private void Start()
    {
        myString = currentWave.ToString();
        waveNumTxt.text = string.Format(myString);
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive <= 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
        EDestroyedCount++;
        EnemyLeftToLose--;
        lvlMusic.PlaySFX(lvlMusic.endPoint);
        if (EnemyLeftToLose <= 0)
        {
            EnemyLeftToLose = 0;
            AudioListener.volume = 1f;
            canSayNextLevel = false;
            GameOver();
        }
        myStringForLives = EnemyLeftToLose.ToString();
        waveLivesTxt.text = string.Format(myStringForLives);
    }

    public void DecreaseEnemiesAlive(int killCurr)
    {
        if (enemiesAlive > 0 && enemiesAlive != null)
        {
            enemiesAlive--;
            coinsToAdd = killCurr;
            ToSendToCurr();
            flagToTake = true;
            checkFlagToTake();
            currScript.AddEnemyKillCurr(coinsToAdd);
        }

        if (currentWave >= MaxWaveNum && enemiesAlive <= 0 && enemiesLeftToSpawn == 0)
        {
            AudioListener.volume = 1f;
            Victory();
        }

        coinsToAdd = 0;
        flagToTake = false;
    }


    public bool checkFlagToTake()
    {
        return flagToTake;
    }

    public int ToSendToCurr()
    {
        return coinsToAdd;
    }

    private IEnumerator StartWave()
    {
        if (!gameOver && !victory)
        {
            
            yield return new WaitForSeconds(timeBetweenWaves);
            isSpawning = true;
            enemiesLeftToSpawn = EnemiesPerWave();
            enemyNumCount = 0;
            enemiesSpawned = 0;
            enemiesSpawned2 = 0;
            prefabIndex = 0;
            prefabIndex2 = 0;
        }
    }

    public void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        if (currentWave <= MaxWaveNum)
        {
            myString = currentWave.ToString();
            waveNumTxt.text = string.Format(myString);
        }


        if (!gameOver)
        {
            if (currentWave > MaxWaveNum && enemiesAlive <= 0 && enemiesLeftToSpawn == 0)
            {
                Victory();
            }

            if (currentWave > 1 && currentWave <= MaxWaveNum && canSayNextLevel)
            {
                lvlMusic.PlaySFX(lvlMusic.startWave);
            }

            StartCoroutine(StartWave());
        }
    }


    private void SpawnEnemy()
    {
        int waveIndex = currentWave - 1;
        int i = 0, k=0, enemiesToSpawnPath1 = 0, enemiesToSpawnPath2 = 0;
        int enemiesPerPrefab = 0, enemiesPerPrefab2 = 0;

        // Check if there are still enemies to spawn
        if (enemiesLeftToSpawn > 0)
        {
            // First Path Spawn.
            for (i = 0; i < enemyPrefabsConfigPath1.Length; i++)
            {
                enemiesToSpawnPath1 += enemyPrefabsConfigPath1[i].enemiesPerWave[waveIndex];
            }
            Debug.Log("enemiesToSpawnPath1 " +  enemiesToSpawnPath1);
            Debug.Log("prefabIndex, waveIndex " + prefabIndex + " " + waveIndex);

            enemiesPerPrefab = enemiesPerWavePath1[waveIndex, prefabIndex];
            Debug.Log("enemiesPerPrefab " + enemiesPerPrefab);

            if (enemiesSpawned < enemiesToSpawnPath1)
            {
                GameObject prefabToSpawnPath1 = enemyPrefabsConfigPath1[prefabIndex].enemyPrefab;
                GameObject enemyPath1 = Instantiate(prefabToSpawnPath1, LevelManager.main.startPoint.position, Quaternion.identity);
                enemiesAlive++;
                enemiesLeftToSpawn--;
                enemyNumCount++;
                enemiesSpawned++;
                enemyPath1.name = prefabToSpawnPath1.name + enemyNumCount;
                if (enemiesSpawned == enemiesPerPrefab)
                {
                    prefabIndex++;
                }
            }

            // Second Path Spawn.
            for (k = 0; k < enemyPrefabsConfigPath2.Length; k++)
            {
                enemiesToSpawnPath2 += enemyPrefabsConfigPath2[k].enemiesPerWave[waveIndex];
                
            }
            Debug.Log("enemiesToSpawnPath2 " + enemiesToSpawnPath2);

            enemiesPerPrefab2 = enemiesPerWavePath2[waveIndex, prefabIndex2];
            Debug.Log("enemiesPerPrefab2 " + enemiesPerPrefab2);

            if (enemiesSpawned2 < enemiesToSpawnPath2)
            {
                GameObject prefabToSpawnPath2 = enemyPrefabsConfigPath2[prefabIndex2].enemyPrefab;
                GameObject enemyPath2 = Instantiate(prefabToSpawnPath2, LevelManager.main.startPoint2.position, Quaternion.identity);
                enemiesAlive++;
                enemiesLeftToSpawn--;
                enemyNumCount++;
                enemiesSpawned2++;
                enemyPath2.name = prefabToSpawnPath2.name + enemyNumCount;
                if(enemiesSpawned2 == enemiesPerPrefab2)
                {
                    prefabIndex2++;
                }
            }

            // Check if there are more enemyPrefabsConfig for the first path
            if (enemiesSpawned == enemiesToSpawnPath1 && prefabIndex < enemyPrefabsConfigPath1.Length - 1)
                prefabIndex++;

            // Check if there are more enemyPrefabsConfig for the second path
            if (enemiesSpawned2 == enemiesToSpawnPath2 && prefabIndex2 < enemyPrefabsConfigPath2.Length - 1)
                prefabIndex2++;

        }
    }








    private void InitializeEnemiesPerWave()
    {
        int numWaves = MaxWaveNum;

        // Initialize for path 1
        enemiesPerWavePath1 = new int[numWaves, enemyPrefabsConfigPath1.Length];
        for (int waveIndex = 0; waveIndex < numWaves; waveIndex++)
        {
            for (int prefabIndex = 0; prefabIndex < enemyPrefabsConfigPath1.Length; prefabIndex++)
            {
                enemiesPerWavePath1[waveIndex, prefabIndex] = enemyPrefabsConfigPath1[prefabIndex].enemiesPerWave[waveIndex];
            }
        }

        // Initialize for path 2
        enemiesPerWavePath2 = new int[numWaves, enemyPrefabsConfigPath2.Length];
        for (int waveIndex = 0; waveIndex < numWaves; waveIndex++)
        {
            for (int prefabIndex = 0; prefabIndex < enemyPrefabsConfigPath2.Length; prefabIndex++)
            {
                enemiesPerWavePath2[waveIndex, prefabIndex] = enemyPrefabsConfigPath2[prefabIndex].enemiesPerWave[waveIndex];
            }
        }
    }


    private int EnemiesPerWave()
    {
        int waveIndex = currentWave - 1;

        if (waveIndex >= 0 && waveIndex < enemiesPerWavePath1.GetLength(0))
        {
            int sumPath1 = 0;
            int sumPath2 = 0;

            for (int prefabIndex = 0; prefabIndex < enemiesPerWavePath1.GetLength(1); prefabIndex++)
            {
                sumPath1 += enemiesPerWavePath1[waveIndex, prefabIndex];
            }

            for (int prefabIndex = 0; prefabIndex < enemiesPerWavePath2.GetLength(1); prefabIndex++)
            {
                sumPath2 += enemiesPerWavePath2[waveIndex, prefabIndex];
            }

            return sumPath1 + sumPath2;
        }
        else
        {
            return 0;
        }
    }


    public void GameOver()
    {
        if (!gameOver)
        {
            lvlMusic.PlaySFX(lvlMusic.Lose);
            gameOver = true;
            cashScript.GoToMethod(1);
            gameOverMenu.SetActive(true);
        }
        canSayNextLevel = true;
    }

    // Add a return type to the Victory method to return the current level number
    public (int, int) Victory()
    {
        if (!victory)
        {
            victory = true;
            
            lvlMusic.StopBackgroundMusic();
            UnlockNewLevel();
            int currentLevel = GetCurrentLevel();
            int starsEarned = CalculateStarsEarned();
            PlayerPrefs.SetInt("StarsEarned_" + currentLevel, starsEarned);
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            PlayerPrefs.Save();
            
            ActivateStarsInMainMap(currentLevel, starsEarned);




            cashScript.GoToMethod(1);
            return (currentLevel, starsEarned); // Return the current level number
        }
        canSayNextLevel = true;
        return (0, 0); // Return 0 if there is no victory
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


    private int CalculateStarsEarned()
    {
        // Implement your logic to calculate stars based on the victory conditions.
        // For example, you might use EnemyLeftToLose or other criteria.
        // Return the number of stars earned.
        if (EnemyLeftToLose >= 15 && EnemyLeftToLose <= 20)
        {
            lvlMusic.PlaySFX(lvlMusic.Win3Stars);
            victoryMenu3stars.SetActive(true);
            return 3;
        }
        else if (EnemyLeftToLose >= 9 && EnemyLeftToLose <= 14)
        {
            lvlMusic.PlaySFX(lvlMusic.Win2Stars);
            victoryMenu2stars.SetActive(true);
            return 2;
        }
        else if (EnemyLeftToLose >= 1 && EnemyLeftToLose <= 8)
        {
            lvlMusic.PlaySFX(lvlMusic.Win1Star);
            victoryMenu1star.SetActive(true);
            return 1;
        }
        else
        {
            return 0; // No stars earned
        }
    }

    private void ActivateStarsInMainMap(int level, int starsEarned)
    {
        // You can pass this information to the MainMap scene using PlayerPrefs or another method.
        // Here we are using PlayerPrefs to store the stars earned.
        Debug.Log("EstarsEarned " + starsEarned);
        PlayerPrefs.SetInt("StarsEarned_" + level, starsEarned);
        PlayerPrefs.Save();
    }


    public void UnlockNewLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        int levelNumber;
        if (int.TryParse(sceneName.Replace("Level ", ""), out levelNumber))
        {
            if (lvlUnlockedCount < 1 && levelNumber >= 1 && levelNumber <= 11)
            {
                int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
                unlockedLevel = Mathf.Max(unlockedLevel, levelNumber + 1); // Update only if the current level is higher
                PlayerPrefs.SetInt("UnlockedLevel", unlockedLevel);
                PlayerPrefs.Save(); // Save the PlayerPrefs to make the change persistent
                nextLevelUnlocked = true;
                lvlUnlockedCount++;
            }
        }
    }


    public bool GetIfCanSayNextLevel()
    {
        return canSayNextLevel;
    }

}