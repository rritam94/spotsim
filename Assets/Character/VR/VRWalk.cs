using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRWalk : MonoBehaviour
{
    [SerializeField]
    private float move_speed = 1f;
    private Vector2 input;

    [SerializeField]
    private Transform cam;

    void FixedUpdate()
    {

        Vector3 direction = cam.forward * input.y + cam.right * input.x;

        Vector3 movement = Vector3.Scale(direction, new Vector3(1, 0, 1));

        transform.Translate(movement * move_speed * Time.deltaTime, Space.World);
    }

    public void OnMove(InputValue context)
    {
        input = context.Get<Vector2>();
    }

    
}
