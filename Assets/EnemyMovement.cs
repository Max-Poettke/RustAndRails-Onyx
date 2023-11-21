using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform playerTransform; // Assign this in the editor
    public float moveSpeed = 3.0f;
    public float rotationSpeed = 10.0f;

    void Update()
    {
        if (playerTransform != null)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        // Rotate towards the player
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 90; // Keep the enemy upright
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        // Move towards the player
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
    }
}
