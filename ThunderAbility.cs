using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThunderAbility : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] private Animator animator;
    public GameObject thunderAnimationPrefab;
    LevelMusic lvlMusic;

    [Header("Ability 1")]
    public Button ability1;
    public float cooldown1;
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
        ability1.onClick.AddListener(OnButtonClick);
        mainCamera = Camera.main;
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        Ability1();

        // Check for mouse click outside the ice button
        if (Input.GetMouseButtonDown(0) && !IsMouseOverThunderButton())
        {
            HideRangePrefab();
            //Start the cooldown for Ability1.
        }
    }

    void Ability1()
    {
        if (isCooldown == false)
        {
            ability1.interactable = true;
        }

        if (isCooldown)
        {
            ability1.interactable = false;
            ability1.image.fillAmount -= 1 / cooldown1 * Time.deltaTime;

            if (ability1.image.fillAmount <= 0)
            {
                ability1.image.fillAmount = 1;
                isCooldown = false;
            }
        }
    }

    void OnButtonClick()
    {
        if (!isCooldown)
        {
            Debug.Log("Ability 1 Clicked.");
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
        yield return new WaitForSeconds(cooldown1);
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

    bool IsMouseOverThunderButton()
    {
        // Cast a ray from the mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the ray hits the ice button
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject == ability1.gameObject;
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
            lvlMusic.PlaySFX(lvlMusic.thunder);
            ActivateThunderAnimation(rangeInstance.transform.position);
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
    void ActivateThunderAnimation(Vector3 position)
    {
        
        //enemy.TakeDamage(Damage);
        // You can adjust these offsets as needed


        // Modify the position vector with offsets
        Vector3 adjustedPosition = new Vector3(position.x + xOffset, position.y + yOffset, position.z);

        // Instantiate the thunderAnimation prefab at the adjusted position
        GameObject thunderAnimationInstance = Instantiate(thunderAnimationPrefab, adjustedPosition, Quaternion.identity);

        // Optionally, you can set parameters or trigger the animation here
        // thunderAnimationInstance.GetComponent<Animator>().SetTrigger("YourTriggerName");

        // Start a coroutine to handle animation activation and deactivation
        StartCoroutine(AnimateThunder(thunderAnimationInstance));
        Destroy(thunderAnimationInstance, 4f);
    }


    IEnumerator AnimateThunder(GameObject thunderAnimation)
    {
        // Debug.Log("Animation started");
        thunderAnimation.SetActive(true);

        // Wait for the animation to finish
        yield return new WaitForSeconds(thunderAnimation.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

        // Debug.Log("Animation finished");

        thunderAnimation.SetActive(false);
    }
}
