using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float moveSpeed = -2f; // Speed of movement

    void Update()
    {
        // Move the object forward in its local space
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
