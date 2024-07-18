using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Enemy eniemyScr;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;


    private float originalMoveSpeed;
    private Transform target;
    private int pathIndex = 0;
    public bool isAttacking = false;
    public bool IsAttacking => isAttacking;
    public bool goingPath1 = false;
    public bool goingPath2 = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        eniemyScr = GetComponent<Enemy>();

        // Determine the appropriate path based on the spawn point
        if (transform.position == LevelManager.main.startPoint.position)
        {
            goingPath1 = true;
            target = LevelManager.main.path[pathIndex];
        }
        else if (transform.position == LevelManager.main.startPoint2.position)
        {
            goingPath2 = true;
            target = LevelManager.main.path2[pathIndex];
        }

        originalMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        if (goingPath1)
        {
            if (Vector2.Distance(target.position, transform.position) <= 0.1f)
            {
                pathIndex++;

                if (pathIndex >= LevelManager.main.path.Length)
                {
                    EnemySpawner.onEnemyDestroy.Invoke();
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    target = LevelManager.main.path[pathIndex];
                }
            }
        }
        if (goingPath2)
        {
            if (Vector2.Distance(target.position, transform.position) <= 0.1f)
            {
                pathIndex++;

                if (pathIndex >= LevelManager.main.path2.Length)
                {
                    EnemySpawner.onEnemyDestroy.Invoke();
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    target = LevelManager.main.path2[pathIndex];
                }
            }
        }

    }

    private void FixedUpdate()
    {
        //Debug.Log("eniemyScr.GetdefenderFound()" + eniemyScr.GetdefenderFound());
        if (!isAttacking)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            // Stop moving during the attack or if defender is found
            rb.velocity = Vector2.zero;
        }
    }

    public void ApplySlowdown(float percent, float duration)
    {
        // Check if the enemy is already slowed down
        if (IsSlowedDown())
        {
            // Reset the coroutine and restart the slowdown duration
            StopCoroutine("ResetSpeed");
        }

        // Calculate the new move speed based on the percentage
        moveSpeed = originalMoveSpeed * (percent / 100);

        // Start a coroutine to reset the speed after the specified duration
        StartCoroutine(ResetSpeed(duration));
    }

    private IEnumerator ResetSpeed(float delay)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(delay);

        // Reset the move speed to the original value
        moveSpeed = originalMoveSpeed;
    }

    // Check if the enemy is currently slowed down
    private bool IsSlowedDown()
    {
        return moveSpeed != originalMoveSpeed;
    }

    public void SetEnemyScript(Enemy enemyScript)
    {
        eniemyScr = enemyScript;
    }


    public void StartAttack()
    {
        isAttacking = true;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

}
