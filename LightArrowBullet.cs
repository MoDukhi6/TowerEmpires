using UnityEngine;

public class LightArrowBullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator; // Add Animator reference

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float archAngle = 45f; // Customizable arch angle
    [SerializeField] public float bulletDamage = 4f;
    public string shotName;

    private Transform target;
    private bool hitCondition = false;
    LevelMusic lvlMusic;


    private void Awake()
    {
        lvlMusic = GameObject.FindGameObjectWithTag("Audio").GetComponent<LevelMusic>();
    }
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void Start()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();

        // Schedule the destruction of the bullet after 1 second
        Invoke("DestroyBullet", 1.1f);
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            // If the target is null, just continue in a straight line
            rb.velocity = transform.right * bulletSpeed;
            return;
        }

        // Calculate the direction to the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Calculate the arc using Slerp
        Vector3 arc = Vector3.Slerp(direction, Vector3.up, archAngle / 180f);

        // Move the bullet along the arced path
        rb.velocity = arc * bulletSpeed;

        // Optionally, you can rotate the bullet to face the target
        float angle = GetAngleFromVectorFloat(direction);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    float GetAngleFromVectorFloat(Vector3 direction)
    {
        direction = direction.normalized;
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision with " + other.gameObject.name);
        hitCondition = true;

        if (shotName == "Stone")
        {
            lvlMusic.PlaySFX(lvlMusic.stoneImpact);
            animator.SetTrigger("Impact");
            Debug.Log("HitttttttT");
            // The bullet will be destroyed after 0.6 seconds
            Invoke("DestroyBullet", 0.7f);
            // Optionally, you can add logic here to handle other effects when the bullet hits
        }
        if (shotName != "Stone")
        {
            DestroyBullet();
        }

    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    public bool hitCon()
    {
        return hitCondition;
    }

    public float DamageValue()
    {
        return bulletDamage;
    }
}
