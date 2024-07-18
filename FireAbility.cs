using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FireAbility : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] private Animator animator;
    public GameObject fireAnimationPrefab;
    LevelMusic lvlMusic;

    [Header("Ability 3")]
    public Button ability3;
    public float cooldown3;
    private bool isCooldown = false;
    public GameObject rangePrefab;
    private GameObject rangeInstance; // Instance of the rangePrefab
    private Camera mainCamera;
    public float Damage;
    public float xOffset;
    public float yOffset;
    private bool canInstantiate = false;
    
    private void Awake()
    {
        lvlMusic = GameObject.FindGameObjectWithTag("Audio").GetComponent<LevelMusic>();

    }
    void Start()
    {
        ability3.onClick.AddListener(OnButtonClick);
        mainCamera = Camera.main;
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        Ability3();

        // Check for mouse click outside the ice button
        if (Input.GetMouseButtonDown(0) && !IsMouseOverFireButton())
        {
            HideRangePrefab();
            //Start the cooldown for Ability3.
        }
    }

    void Ability3()
    {
        if (isCooldown == false)
        {
            ability3.interactable = true;
        }

        if (isCooldown)
        {
            ability3.interactable = false;
            ability3.image.fillAmount -= 1 / cooldown3 * Time.deltaTime;

            if (ability3.image.fillAmount <= 0)
            {
                ability3.image.fillAmount = 1;
                isCooldown = false;
            }
        }
    }

    void OnButtonClick()
    {
        if (!isCooldown)
        {
            Debug.Log("Ability 3 Clicked.");
            //StartCoroutine(StartCooldown());
            canInstantiate = false;
            // Instantiate the rangePrefab and make it follow the mouse
            rangeInstance = Instantiate(rangePrefab);
            StartCoroutine(FollowMouse());
        }
    }

    IEnumerator StartCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldown3);
        isCooldown = false;

        // Destroy the rangePrefab instance after cooldown
        if (rangeInstance != null)
        {
            Destroy(rangeInstance);
        }
    }

    IEnumerator FollowMouse()
    {
        while (rangeInstance != null)
        {
            // Get the mouse position in world coordinates
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Clamp the position based on camera's orthographic size and aspect ratio
            float aspectRatio = mainCamera.aspect;
            float orthoSize = mainCamera.orthographicSize;
            float x = Mathf.Clamp(mousePosition.x, -orthoSize * aspectRatio, orthoSize * aspectRatio);
            float y = Mathf.Clamp(mousePosition.y, -orthoSize, orthoSize);

            rangeInstance.transform.position = new Vector3(x, y, 0f);

            yield return null;
        }
    }

    bool IsMouseOverFireButton()
    {
        // Cast a ray from the mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the ray hits the ice button
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject == ability3.gameObject;
        }

        return false;
    }

    void HideRangePrefab()
    {
        // Hide the rangePrefab
        if (rangeInstance != null)
        {
            // Take range position.
            rangeInstance.SetActive(false);
            ActivatePower();
        }
    }


    void ActivatePower()
    {
        if (!canInstantiate)
        {
            // Set the flag to true
            canInstantiate = true;
            StartCoroutine(StartCooldown());
            lvlMusic.PlaySFX(lvlMusic.fire);
            ActivateFireAnimation(rangeInstance.transform.position);
            // Get all colliders within the range of the rangePrefab
            Collider2D[] colliders = Physics2D.OverlapCircleAll(rangeInstance.transform.position, 1f, LayerMask.GetMask("Enemy"));

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        //Debug.Log(enemy);
                        enemy.TakeDamage(Damage);

                    }
                }
            }
        }
    }
    void ActivateFireAnimation(Vector3 position)
    {
        //enemy.TakeDamage(Damage);
        // You can adjust these offsets as needed


        // Modify the position vector with offsets
        Vector3 adjustedPosition = new Vector3(position.x + xOffset, position.y + yOffset, position.z);

        // Instantiate the fireAnimation prefab at the adjusted position
        GameObject fireAnimationInstance = Instantiate(fireAnimationPrefab, adjustedPosition, Quaternion.identity);

        // Optionally, you can set parameters or trigger the animation here
        // fireAnimationInstance.GetComponent<Animator>().SetTrigger("YourTriggerName");

        // Start a coroutine to handle animation activation and deactivation
        StartCoroutine(AnimateFire(fireAnimationInstance));
        Destroy(fireAnimationInstance, 4f);
    }


    IEnumerator AnimateFire(GameObject fireAnimation)
    {
        // Debug.Log("Animation started");
        fireAnimation.SetActive(true);

        // Wait for the animation to finish
        yield return new WaitForSeconds(fireAnimation.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        // Debug.Log("Animation finished");

        fireAnimation.SetActive(false);
    }

}