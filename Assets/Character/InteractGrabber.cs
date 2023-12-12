using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractGrabber : MonoBehaviour
{

    [SerializeField]
    private bool grabbing;
    [SerializeField]
    private bool am_i_VR_character;

    private Collider trigger;

    private Transform heldObject;

    private void Start()
    {
        trigger = GetComponent<Collider>();
    }

    private void Update()
    {
        // grabbed by another

        if (heldObject != null && heldObject.parent != transform)
        {
            heldObject = null;
        }


        if (!grabbing && heldObject != null)
        {
            heldObject.SetParent(null);
            if (heldObject.GetComponent<Rigidbody>())
            {
                heldObject.GetComponent<Rigidbody>().mass = 1f;
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            heldObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (grabbing && heldObject == null)
        {
            if (am_i_VR_character && other.name.Contains("Fries"))
            {
                return;
            }
            if (!am_i_VR_character && other.name.Contains("Drink"))
            {
                return;
            }

            if (other.CompareTag("Item"))
            {
                heldObject = other.transform;
                heldObject.SetParent(transform, true);
                heldObject.position = transform.position;

                if (heldObject.GetComponent<Rigidbody>())
                {
                    heldObject.GetComponent<Rigidbody>().mass = 0.00001f;
                    heldObject.GetComponent<Rigidbody>().isKinematic = true;
                }

                
                BaseTask.Instance.Grab(GetParentName(), heldObject.name, Time.time);
            }
            else if (null != other.GetComponent<ItemSpawner>())
            {
                heldObject = other.GetComponent<ItemSpawner>().SpawnMyItem().transform;
                heldObject.SetParent(transform, true);
                heldObject.position = transform.position;

                if (heldObject.GetComponent<Rigidbody>())
                {
                    heldObject.GetComponent<Rigidbody>().mass = 0.00001f;
                    heldObject.GetComponent<Rigidbody>().isKinematic = true;
                }

                BaseTask.Instance.Grab(GetParentName(), heldObject.name, Time.time);
            }
        }
    }

    public void OnGrab(InputValue value)
    {
        Debug.Log(value);
        grabbing = value.Get<float>() == 1f;

        trigger.enabled = grabbing;
    }

    private string GetParentName()
    {
        Transform parent = transform;
        while (parent.parent != null)
        {
            parent = parent.parent;
        }
        return parent.name;
    }
}
