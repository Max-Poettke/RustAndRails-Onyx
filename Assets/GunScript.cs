using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float shootingRange = 100.0f; // Adjust as needed
    public float shootingRate = 0.5f; // Time between shots
    private float nextShootTime = 0.0f;

    void Update()
    {
        AimAtMouse();

        if (Input.GetMouseButton(0) && Time.time >= nextShootTime) // Left mouse button to shoot
        {
            nextShootTime = Time.time + shootingRate;
            Shoot();
        }
    }

    void AimAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            Vector3 direction = targetPosition - transform.position;

            if (direction.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10.0f);
            }
        }
    }

    void Shoot()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, shootingRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject); // Destroy the enemy
            }
        }
    }
}