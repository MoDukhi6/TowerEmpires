using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystemCollider : MonoBehaviour
{// Super class for all the Towers in this game.
    private string ToShow = null;
    private string otherName = null;
    private Queue<GameObject> enemies = new Queue<GameObject>();
    private float towerAttackRadius = 3f;
    public LayerMask enemyLayer;
    private bool flag = false;
    [SerializeField] private string TowerType;
    [SerializeField] private string TowerTypeAndLvl;
    private Transform target;
    private float timeUntilFire;
    public int numEnemiesEnteredTargetArea;
    LevelMusic lvlMusic;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float bps = 1f; // Bullets Per Second
    
    private void Awake()
    {
        lvlMusic = GameObject.FindGameObjectWithTag("Audio").GetComponent<LevelMusic>();

    }
    private void Update()
    {
        Debug.Log(ToShow);
        if (ToShow != null && ToShow != "X")
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / bps)
            {
                // Set the target before shooting
                SetTarget();
                animator.SetTrigger("Attack");
                Shoot();
                if (TowerType == "Arched")
                {
                    lvlMusic.PlaySFX(lvlMusic.archedShoot);
                }
                if (TowerType == "Shadow")
                {
                    lvlMusic.PlaySFX(lvlMusic.shadowShoot);
                }
                if (TowerType == "Magic")
                {
                    lvlMusic.PlaySFX(lvlMusic.magicShoot);
                }
                if (TowerType == "Stone")
                {
                    lvlMusic.PlaySFX(lvlMusic.stoneShoot);
                }
                timeUntilFire = 0f;
            }
        }
    }

    // Set the target to the first enemy in the queue
    private void SetTarget()
    {
        if (enemies.Count > 0 && enemies.Peek() != null) // Add null check
        {
            target = enemies.Peek().transform;
        }
    }

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void OnEnable()
    {
        // This method is called every time the script is enabled (e.g., when the tower is placed or activated)
        PlayPlacementSound();
    }

    void PlayPlacementSound()
    {
        if (TowerTypeAndLvl == "Arched1")
        {
            lvlMusic.PlaySFX(lvlMusic.archedSet1);
        }
        else if (TowerTypeAndLvl == "Shadow1")
        {
            lvlMusic.PlaySFX(lvlMusic.shadowSet1);
        }
        else if (TowerTypeAndLvl == "Magic1")
        {
            lvlMusic.PlaySFX(lvlMusic.magicSet1);
        }
        else if (TowerTypeAndLvl == "Stone1")
        {
            lvlMusic.PlaySFX(lvlMusic.stoneSet1);
        }
        else if (TowerTypeAndLvl == "Arched2")
        {
            lvlMusic.PlaySFX(lvlMusic.archedSet2);
        }
        else if (TowerTypeAndLvl == "Shadow2")
        {
            lvlMusic.PlaySFX(lvlMusic.shadowSet2);
        }
        else if (TowerTypeAndLvl == "Magic2")
        {
            lvlMusic.PlaySFX(lvlMusic.magicSet2);
        }
        else if (TowerTypeAndLvl == "Stone2")
        {
            lvlMusic.PlaySFX(lvlMusic.stoneSet2);
        }
        else if (TowerTypeAndLvl == "Arched3")
        {
            lvlMusic.PlaySFX(lvlMusic.archedSet3);
        }
        else if (TowerTypeAndLvl == "Shadow3")
        {
            lvlMusic.PlaySFX(lvlMusic.shadowSet3);
        }
        else if (TowerTypeAndLvl == "Magic3")
        {
            lvlMusic.PlaySFX(lvlMusic.magicSet3);
        }
        else if (TowerTypeAndLvl == "Stone3")
        {
            lvlMusic.PlaySFX(lvlMusic.stoneSet3);
        }
        else if (TowerTypeAndLvl == "Arched4")
        {
            lvlMusic.PlaySFX(lvlMusic.archedSet4);
        }
        else if (TowerTypeAndLvl == "Shadow4")
        {
            lvlMusic.PlaySFX(lvlMusic.shadowSet4);
        }
        else if (TowerTypeAndLvl == "Magic4")
        {
            lvlMusic.PlaySFX(lvlMusic.magicSet4);
        }
        else if (TowerTypeAndLvl == "Stone4")
        {
            lvlMusic.PlaySFX(lvlMusic.stoneSet4);
        }
    }

    private void Shoot()
    {
        if (firingPoint != null && target != null) // Add null checks
        {
            // Instantiate the bullet and set its target
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            LightArrowBullet bulletScript = bulletObj.GetComponent<LightArrowBullet>();
            bulletScript.SetTarget(target);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Enqueue the GameObject into the enemies queue
            enemies.Enqueue(other.gameObject);
            numEnemiesEnteredTargetArea++;
            // Optionally, set ToShow to the name of the first enemy in the queue
            if ((ToShow == null || ToShow == "X") && enemies.Count > 0)
            {
                ToShow = enemies.Peek().name;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // If the exiting enemy is at the front of the queue, dequeue it
            if (enemies.Count > 0 && (enemies.Peek() == other.gameObject || flag == true))
            {
                
                enemies.Dequeue();
                flag = false;
                if (enemies.Count > 0)
                {
                    if (EnemyInRange(enemies.Peek()))// check the next enemy (that should be the first now in the queue) //have to change (other).
                    {// Optionally, update ToShow to the name of the next enemy in the queue
                        //otherName = other.gameObject.name;
                        ToShow = enemies.Peek().name;
                        Debug.Log("count > 0 and enemy in range, 97, " + ToShow);
                    }
                    else
                    {
                        ToShow = "X";
                        Debug.Log("count > 0 but enemy is not in range, 102");
                        enemies.Dequeue();
                    }
                }
                else
                {
                    Debug.Log("No other enemies in range, 108");
                    ToShow = "X";
                }
            }
            else
            {
                if (enemies.Count <= 0)
                {
                    Debug.Log("No other enemies in range, 116");
                    //enemies.Dequeue();
                    ToShow = "X";
                }
                else
                {
                    //Debug.Log("enemies.Peek(): " + enemies.Peek());
                    //Debug.Log("other.gameObject: " + other.gameObject);
                    Debug.Log("enemies.Peek() != other.gameObject, 122");
                    if(enemies.Peek().name != null)
                    {
                        ToShow = enemies.Peek().name;
                    }
                    Debug.Log("Targeting: " + ToShow);
                    enemies.Dequeue();
                    flag = true;
                }
            }
        }
    }


    // Check if the distance between the enemy and the center of the range is not more than the range itself
    private bool EnemyInRange(GameObject other)
    {
        if (other != null && other.gameObject != null)
        {
            // Check if the object has been destroyed before accessing it
            if (other.gameObject.activeSelf)
            {
                float distance = Vector3.Distance(transform.position, other.transform.position);
                return distance <= towerAttackRadius;
            }
        }
        return false;
    }



}
