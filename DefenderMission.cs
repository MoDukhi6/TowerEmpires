using System.Collections;
using UnityEngine;

public class DefenderMission : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] float health, maxHealth;
    [SerializeField] float autoHealTime;
    [SerializeField] float autoHealBy;
    [SerializeField] float Damage;
    [SerializeField] float damageDelay; // Adjust the delay as needed
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] ShieldTower shieldTower;
    [SerializeField] DefMovement DefMoveScript;

    [Header("Defender Range")]
    [SerializeField] float defenderRange;  // Adjust the range as needed
    [SerializeField] bool isEnemyInRange = false;
    bool AttackHappenedBefore = false;

    private float dmgToTake;
    private bool hasDied = false;
    private bool enemyHit = false;
    private bool gotDMG = false;
    private float EnimDmg;
    private float attackDelay = 1.5f;
    private float autoHealTimer = 0f;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    public int enemyinrangeCounter = 0;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        healthBar.gameObject.SetActive(false);
        shieldTower = FindObjectOfType<ShieldTower>();
        DefMoveScript = FindObjectOfType<DefMovement>();
    }

    void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        // Check if there is an enemy in defender's range
        CheckEnemyInRange();

        if (gotDMG)
        {
            healthBar.gameObject.SetActive(true);
        }

        if (health < maxHealth && health != 0)
        {
            autoHealTimer += Time.deltaTime;

            if (autoHealTimer >= autoHealTime)
            {
                // Add health
                health += autoHealBy;
                // Ensure health doesn't exceed maxHealth
                health = Mathf.Min(health, maxHealth);
                // Update the health bar
                healthBar.UpdateHealthBar(health, maxHealth);

                // Reset the timer
                autoHealTimer = 0f;
            }
        }

        // Update the attack timer
        //if (isAttacking)
        //{
        //attackTimer += Time.deltaTime;

        // Check if enough time has passed for the next attack

        //}
    }

    private void CheckEnemyInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, defenderRange);

        // Check if there is at least one enemy in range
        isEnemyInRange = false;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                isEnemyInRange = true;
                enemyinrangeCounter++;
                //DefenderAttackEnemy(collider.GetComponent<Enemy>());
                Debug.Log("Enemy Is In Range!");
                break;  // No need to check further if one enemy is found
            }
        }
    }


    public void TakeDamage(float dmgToTake)
    {
        if (!hasDied)
        {
            health -= dmgToTake;
            animator.SetTrigger("Hurt");
            gotDMG = true;
            healthBar.UpdateHealthBar(health, maxHealth);
            //healthBar.gameObject.SetActive(true);
            if (health <= 0)
            {
                Debug.Log("Die!");
                Die();
            }
            healthBar.UpdateHealthBar(health, maxHealth);
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public void Die()
    {
        if (!hasDied)
        {
            hasDied = true;
            animator.SetTrigger("Die");
            //numDefAlive = shieldTower.GetDefAliveNum();
            Debug.Log("11Number of Defenders Alive is: " + shieldTower.GetDefAliveNum());
            DestroySelf();
            shieldTower.DecreaseDefAlive();
            Debug.Log("22Number of Defenders Alive is: " + shieldTower.GetDefAliveNum());
        }
    }

    private void DestroySelf()
    {
        // Destroy the GameObject after a delay (you can adjust the delay time)
        Destroy(gameObject, 0.7f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Assuming Enemy script has a method GetDamage() that returns the damage value
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (!isAttacking)
            {
                // Set isAttacking to true to prevent starting another attack until this one finishes
                isAttacking = true;
                Debug.Log("collisionHappend!!!");
                // Start the attack
                DefenderAttackEnemy(enemy);
            }
        }
    }

    private void DefenderAttackEnemy(Enemy enemy)
    {
        StartCoroutine(AttackCoroutine(enemy));
    }

    private IEnumerator AttackCoroutine(Enemy enemy)
    {
        int DMGc = 0;

        isEnemyInRange = true;
        while (health > 0 && isEnemyInRange)
        {
            // Check if enough time has passed for the next attack
            yield return new WaitForSeconds(damageDelay);

            animator.SetTrigger("Attack");

            // Check if there are still enemies in range
            if (enemy != null && enemy.GetHealth() > 0)
            {
                enemy.HurtFromDefFunc(Damage);
                DMGc++;
                Debug.Log("DMG " + DMGc);
            }
            else
            {
                // If the current enemy is dead, check for other enemies in range
                CheckEnemyInRange();

                // If no enemies are in range, exit the coroutine
                if (!isEnemyInRange)
                {
                    isAttacking = false;
                    DefGoingIdle();
                    yield break;
                }
                else
                {
                    // Get a new reference to an enemy in range
                    enemy = FindEnemyInRange();
                }
            }

            animator.SetTrigger("Idle");
        }
    }

    private Enemy FindEnemyInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, defenderRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                return collider.GetComponent<Enemy>();
            }
        }

        return null;
    }


    public void DefGoingIdle()
    {
        animator.SetTrigger("Idle");
        Debug.Log("Going Idle");

    }

    public void DefenderHurt(float EnimDmg)
    {
        TakeDamage(EnimDmg);
    }

    public bool IfGotDMG()
    {
        return gotDMG;
    }
}
