using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Detection")]
    [SerializeField] public float detectionRange;

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] Text currencyInText;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] float health, maxHealth;
    [SerializeField] float Damage;
    [SerializeField] int killCoins;
    [SerializeField] int CashFromDie;
    [SerializeField] private GameObject lArrow;
    [SerializeField] public DefenderMission defenderToWork;
    private DefenderMission currentDefender;
    //[SerializeField] private GameObject lightStone;
    //[SerializeField] private GameObject lightElect;
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] LightArrowBullet lightArrow;
    [SerializeField] CashCounter cashcnt;
    private EnemyMovement enemyMovement;

    LevelMusic lvlMusic;
    private float damageAmount;
    private bool hasDied = false;
    private bool arrowHit = false;
    private bool defHit = false;
    public bool defenderFound = false;
    private float DefDmg;
    //private bool electHit = false;
    //private bool stoneHit = false;
    private List<GameObject> defendersInRange = new List<GameObject>();
    private float attackDelay = 0.7f;
    private EnemyState currentState = EnemyState.Moving;

    private enum EnemyState
    {
        Moving,
        Attacking,
        Dead
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.gameObject.SetActive(false);
        enemySpawner = FindObjectOfType<EnemySpawner>();
        cashcnt = FindObjectOfType<CashCounter>();
        enemyMovement = GetComponent<EnemyMovement>();
        lvlMusic = GameObject.FindGameObjectWithTag("Audio").GetComponent<LevelMusic>();
        //setTowerx = FindObjectOfType<SetTowerX>();

    }

    private void Update()
    {
        string txtString = currencyInText.text;
        int intCurrency = int.Parse(txtString);
        //Debug.Log(intCurrency);

        DetectDefenders();
        
        // Add logic for regular movement here...

    }

    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (currentState == EnemyState.Moving)
        {
            animator.SetTrigger("Walk");
        }

    }

    public float GetKillCoins() { return killCoins; }

    public float GetEDamage() { return Damage; }

    private void DetectDefenders()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        defenderFound = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Defender"))
            {
                defenderFound = true;
                Debug.Log("Defender Found!");
                defendersInRange.Add(collider.gameObject);
                //HandleDefenderCollision();
                break;
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        animator.SetTrigger("Hurt");
        healthBar.gameObject.SetActive(true);
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Debug.Log("Die!");
            Die();
        }
    }

    public void Die()
    {
        if (!hasDied)
        {
            hasDied = true;
            lvlMusic.PlaySFX(lvlMusic.death);
            animator.SetTrigger("Die");
            DestroySelf();
            currentState = EnemyState.Dead;
            enemySpawner.DecreaseEnemiesAlive(killCoins);// enemiesAlive--;
            cashcnt.AddCash(CashFromDie);// currencyInText += killCoins;
            //setTowerx.IncreaseCurrency(killCoins);
        }

    }

    private void DestroySelf()
    {
        // Destroy the GameObject after a delay (you can adjust the delay time)
        Destroy(gameObject, 1.1f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the collision is with a Bullet
        if (other.gameObject.CompareTag("Bullet"))
        {
            // Handle bullet-specific logic
            HandleBulletCollision(other);
        }
        // Check if the collision is with a Defender
        else if (other.gameObject.CompareTag("Defender"))
        {
            Debug.Log("Collision with defender.");
            // Handle defender-specific logic
            HandleDefenderCollision(other);
        }
    }

    private void HandleBulletCollision(Collision2D other)
    {
        lightArrow = other.gameObject.GetComponent<LightArrowBullet>();
        if (lightArrow != null)
        {
            healthBar.gameObject.SetActive(true);
            arrowHit = true;
            float damageAmount = lightArrow.DamageValue();
            
            TakeDamage(damageAmount);
        }
    }

    private void HandleDefenderCollision(Collision2D other)
    {
        DefenderMission defender = other.gameObject.GetComponent<DefenderMission>();
        if (defender != null)
        {
            Debug.Log("Handle a collision with a defender.");
            StartCoroutine(AttackDefender(defender));
        }
    }

    private IEnumerator AttackDefender(DefenderMission defender)
    {
        currentDefender = defender;  // Store the current defender for later use
        currentState = EnemyState.Attacking;
        enemyMovement.StartAttack();  // Set the attack status to true
        Debug.Log("Want to start attack.");

        // Check if there are defenders in range
        if (defendersInRange.Count == 0)
        {
            currentState = EnemyState.Moving;
            Debug.Log("No defenders in range. Exiting AttackDefender coroutine.");
            yield break;
        }

        while (health > 0 && currentDefender != null && currentDefender.GetHealth() > 0)
        {
            if (!defenderFound)
            {
                currentState = EnemyState.Moving;
                Debug.Log("Defender not found. Exiting AttackDefender coroutine.");
                yield break;
            }

            Debug.Log("Attack Started.");
            yield return new WaitForSeconds(attackDelay);
            healthBar.gameObject.SetActive(true);
            animator.SetTrigger("Attack");

            if (currentDefender != null)
            {
                currentDefender.DefenderHurt(Damage);
            }
        }

        if (health > 0)
        {
            enemyMovement.EndAttack();  // Set the attack status to false
            currentState = EnemyState.Moving;
            Debug.Log("Defender killed let's move.");
            animator.SetTrigger("Move");
            defenderFound = false;
        }
    }


    public float GetHealth()
    {
        return health;
    }
    
    public bool GetdefenderFound()
    {
        return defenderFound;
    }

    public void HurtFromDefFunc(float DefDmg)
    {
        animator.SetTrigger("Hurt");
        TakeDamage(DefDmg);
    }


}

