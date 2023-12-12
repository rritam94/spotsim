using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardMovement : MonoBehaviour
{

    [SerializeField]
    private float move_speed = 1f;
    private Vector3 movement;

    [SerializeField]
    private float rotate_speed = 1f;
    private float rotateH;
    private float rotateV;

    [SerializeField]
    private float lock_y_position = 1.5f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    void FixedUpdate()
    {
        transform.Translate(movement * move_speed * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.up, rotateH * rotate_speed * Time.deltaTime, Space.World);
        transform.Rotate(transform.right, - rotateV * rotate_speed * Time.deltaTime, Space.World);

        transform.position = new Vector3(transform.position.x, lock_y_position, transform.position.z);
    }

    public void OnMove(InputValue context)
    { 
        Vector2 input = context.Get<Vector2>();
        movement = new Vector3(input.x, 0, input.y);
    }

    public void OnRotateH(InputValue context)
    {
        rotateH = context.Get<float>();
    }

    public void OnRotateV(InputValue context)
    {
        rotateV = context.Get<float>();
    }


}
