using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Slider fuelSlider;
    public float fuel = 100.0f;

    private Rigidbody rb;
    private Vector3 movement;
    
    public TMP_Text interactionText;

    private bool hasCoal = false;
    public GameObject coal;
    public GameObject gun;
    public GameObject camera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Input
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        fuel -= Time.deltaTime * 3;
        fuelSlider.value = fuel;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (nextToCoal && hasGun == false)
            {
                hasCoal = true;
                coal.SetActive(true);
            }

            if (nextToFurnace && hasCoal)
            {
                fuel = Mathf.Clamp(fuel + 50.0f, 0.0f, 100.0f);
                fuelSlider.value = fuel;
                hasCoal = false;
                coal.SetActive(false);
            }

            if (nextToGun && hasCoal == false)
            {
                gun.SetActive(true);
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Q)){
            if (hasGun)
            {
                gun.SetActive(false);
                hasGun = false;
            }
            
            if(hasCoal)
            {
                coal.SetActive(false);
                hasCoal = false;
            }
        }

        // Movement vector
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        RotateTowardsMouse();
    }
    
    void RotateTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            targetPosition.y = transform.position.y; // Keep player upright by not changing the Y position
            Vector3 direction = targetPosition - transform.position;
            if (direction.magnitude > 0.1f) // Check if the direction is significant enough to rotate
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10.0f); // Rotate the player
            }
        }
    }

    void FixedUpdate()
    {
        // Move the player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }


    private bool nextToCoal = false;
    private bool nextToFurnace = false;
    private bool nextToGun = false;
    private bool hasGun = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gun"))
        {
            if (hasGun == false)
            {
                nextToGun = true;
                interactionText.gameObject.SetActive(true);
                interactionText.text = "Press 'E' to pick up";
            }
        }
        
        if (other.gameObject.CompareTag("Furnace"))
        {
            nextToFurnace = true;
            interactionText.gameObject.SetActive(true);
            interactionText.text = "Press 'E' to interact";
        }

        if (other.gameObject.CompareTag("Coal"))
        {
            nextToCoal = true;
            interactionText.gameObject.SetActive(true);
            interactionText.text = "Press 'E' to pick up coal";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Furnace"))
        {
            nextToFurnace = false;
            interactionText.gameObject.SetActive(false);
        }
        
        if (other.gameObject.CompareTag("Coal"))
        {
            nextToCoal = false;
            interactionText.gameObject.SetActive(false);
        }
    }
}
