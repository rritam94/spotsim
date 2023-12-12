using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement

    void Update()
    {
        // Move the object forward in its local space
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
