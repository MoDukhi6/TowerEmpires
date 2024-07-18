using System.Collections;
using UnityEngine;

public class DefMovement : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] DefenderMission defMission;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 3f;

    private Transform target;
    private bool arrived = false;
    float health, maxHealth;


    private void Start()
    {
        SetInitialValues();
    }

    private void SetInitialValues()
    {
        // Set the target to the defender's destination point
        target = DefLvlManager.main.GetDefEndpoint(transform.name);
        SetRunAnimation();
        transform.Rotate(0f, -180f, 0f);
    }

    private void Update()
    {
        if (!arrived)
        {
            float distance = Vector2.Distance(target.position, transform.position);
            if (distance <= 0.01f)
            {
                SetIdleAnimation();
                arrived = true;
                rb.velocity = Vector2.zero; // Stop the defender when arrived
            }
        }

    }

    private void FixedUpdate()
    {
        if (!arrived)
        {
            // Calculate the direction without modifying the z component
            Vector3 direction3D = (target.position - (Vector3)transform.position).normalized;
            Vector2 direction = new Vector2(direction3D.x, direction3D.y);
            rb.velocity = direction * moveSpeed;

            // Ensure the z component of the Rigidbody2D's position stays the same
            rb.position = new Vector3(rb.position.x, rb.position.y, transform.position.z);
        }
    }


    public bool GetIfArriveOrNot()
    {
        return arrived;
    }

    private void SetRunAnimation()
    {
        // Set the "Run" animation state
        animator.SetTrigger("Run");
    }

    private void SetIdleAnimation()
    {
        // Set the "Idle" animation state
        animator.SetTrigger("Idle");
    }

}
