using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShieldTower : MonoBehaviour
{
    [Header("Events")]
    public static UnityEvent onDefDestroy = new UnityEvent();

    [Header("References")]
    [SerializeField] private GameObject[] defPrefabs;
    public GameObject[] defArr = null;
    public DefenderMission defenderMission;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [SerializeField] private int numDefAlive = 0;
    [SerializeField] public int defCount = 0;
    private bool spawnAllowed = true;
    public float spawnDelay;
    private int index;

    private bool isTowerActive = true; // New variable to track tower activity.
    public bool tActive = false; // if the tower is active or not.
    public int c = 0; // 
    private bool spawnedBefore = false;
    private float timeSinceDead;

    void Start()
    {
        defArr = new GameObject[defPrefabs.Length];
        StartCoroutine(SpawnDefenderCoroutine());
        c++;
    }

    void Update()
    {
        //Debug.Log("ShieldTowerIsNowActiveAndShouldSpawnDefenders." + tActive);

        tActive = true;
        if(tActive & spawnedBefore == false)
        {
            spawnedBefore = true;
            StartCoroutine(SpawnDefenderCoroutine());
        }

        if (!isTowerActive)
        {
            DestroyAllDefenders();
            return; // No need to continue checking if the tower is not active
        }

        if (numDefAlive == 1)
        {
            spawnAllowed = false;
        }
        else
        {
            spawnAllowed = true;
        }

        if (defArr[0] == null && spawnedBefore && Time.time - timeSinceDead > 7f)
        {
            SpawnDefenderCoroutine();
        }
        
    }

    private void DestroyAllDefenders()
    {
        for (int i = 0; i < defArr.Length; i++)
        {
            if (defArr[i] != null)
            {
                Destroy(defArr[i]);
                numDefAlive--;
                defCount--;
            }
        }
    }


    private IEnumerator SpawnDefenderCoroutine()
    {
        yield return new WaitForSeconds(0.1f); // Wait for 0.5 seconds before spawning

        if (!isTowerActive)
        {
            yield break; // If the tower is not active, don't spawn defenders
        }

        if (defArr[0] == null)
        {
            GameObject prefabToSpawn = defPrefabs[0];
            //Debug.Log("Spawning Defender");
            GameObject defender = Instantiate(prefabToSpawn, DefLvlManager.main.DefStart1.position, Quaternion.identity);
            defender.name = "Defender1";
            defArr[0] = defender;
            numDefAlive++;
            defCount++;
        }
    }
    
    public void SetTActiveToTrue()
    {
        tActive = true;
    }

    public void SetTActiveToFalse()
    {
        tActive = false;
    }

    public int GetDefAliveNum()
    {
        return numDefAlive;
    }

    public void DecreaseDefAlive()
    {
        numDefAlive--;
        timeSinceDead = Time.time;
        defCount--;
        c = 0;
    }

    public GameObject[] GetDefendersArray()
    {
        return defArr;
    }

    public void SetTowerActive(bool isActive)
    {
        isTowerActive = isActive;
        if (!isTowerActive)
        {
            DestroyAllDefenders();
        }
    }
}
