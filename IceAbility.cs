using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IceAbility : MonoBehaviour
{
    [SerializeField] EnemyMovement enemyMove;
    [SerializeField] private Animator animator;
    public GameObject freezeAnimationPrefab;
    LevelMusic lvlMusic;

    [Header("Ability 2")]
    public Button ability2;
    public float cooldown2;
    private bool isCooldown = false;
    public GameObject rangePrefab;
    private GameObject rangeInstance; // Instance of the rangePrefab
    private Camera mainCamera;
    public float slowSpeed;
    public float xOffset;
    public float yOffset;
    private bool canInstantiate = false;
    
    private void Awake()
    {
        lvlMusic = GameObject.FindGameObjectWithTag("Audio").GetComponent<LevelMusic>();

    }

    void Start()
    {
        ability2.onClick.AddListener(OnButtonClick);
        mainCamera = Camera.main;
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        Ability2();

        // Check for mouse click outside the ice button
        if (Input.GetMouseButtonDown(0) && !IsMouseOverIceButton())
        {
            HideRangePrefab();
            //Start the cooldown for Ability2.
        }
    }

    void Ability2()
    {
        if (isCooldown == false)
        {
            ability2.interactable = true;
        }

        if (isCooldown)
        {
            ability2.interactable = false;
            ability2.image.fillAmount -= 1 / cooldown2 * Time.deltaTime;

            if (ability2.image.fillAmount <= 0)
            {
                ability2.image.fillAmount = 1;
                isCooldown = false;
            }
        }
    }

    void OnButtonClick()
    {
        if (!isCooldown)
        {
            Debug.Log("Ability 2 Clicked.");
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
        yield return new WaitForSeconds(cooldown2);
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

    bool IsMouseOverIceButton()
    {
        // Cast a ray from the mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the ray hits the ice button
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject == ability2.gameObject;
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
            lvlMusic.PlaySFXModified(lvlMusic.ice, 1.4f); // 1.5f means 50% louder (adjust as needed)
            ActivateFreezeAnimation(rangeInstance.transform.position);
            //canInstantiate = false;
            // Get all colliders within the range of the rangePrefab
            Collider2D[] colliders = Physics2D.OverlapCircleAll(rangeInstance.transform.position, 1f, LayerMask.GetMask("Enemy"));

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    EnemyMovement enemy = collider.GetComponent<EnemyMovement>();
                    if (enemy != null)
                    {
                        enemy.ApplySlowdown(slowSpeed, 5f);

                    }
                }
            }


        }
    }
    void ActivateFreezeAnimation(Vector3 position)
    {
        // You can adjust these offsets as needed


        // Modify the position vector with offsets
        Vector3 adjustedPosition = new Vector3(position.x + xOffset, position.y + yOffset, position.z);

        // Instantiate the FreezeAnimation prefab at the adjusted position
        GameObject freezeAnimationInstance = Instantiate(freezeAnimationPrefab, adjustedPosition, Quaternion.identity);

        // Optionally, you can set parameters or trigger the animation here
        // freezeAnimationInstance.GetComponent<Animator>().SetTrigger("YourTriggerName");

        // Start a coroutine to handle animation activation and deactivation
        StartCoroutine(AnimateFreeze(freezeAnimationInstance));
        Destroy(freezeAnimationInstance, 5f);
    }


    IEnumerator AnimateFreeze(GameObject freezeAnimation)
    {
        // Activate the FreezeAnimation
        freezeAnimation.SetActive(true);

        // Optionally, you can set parameters or trigger the animation here
        // freezeAnimation.GetComponent<Animator>().SetTrigger("YourTriggerName");

        // Wait for the animation to finish
        yield return new WaitForSeconds(freezeAnimation.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        // Deactivate the FreezeAnimation
        freezeAnimation.SetActive(false);

        // Optionally, you can perform additional cleanup or actions after animation completion
    }
}