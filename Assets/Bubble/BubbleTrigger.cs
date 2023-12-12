using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BubbleTrigger : MonoBehaviour
{

    Rigidbody my_rb;
    Rigidbody in_my_space;

    private List<Collider> currently_touching = new List<Collider>();
    private List<Renderer> my_bubble = new List<Renderer>();

    private float alpha;

    // Start is called before the first frame update
    void Start()
    {
        my_rb = transform.GetComponentInParent<Rigidbody>();

        // these overrides ensure the rigidbody is being used for triggers and not messing with physics
        
        // if there was already a rigidbody on the character, this wouldn't be necessary
        //my_rb.useGravity = false;
        //my_rb.isKinematic = true;

        // automate the list so I don't have to click too much
        foreach (Renderer r in transform.GetComponentsInChildren<Renderer>())
        {
            if (r.sharedMaterial.name.Equals("BubbleMaterial"))
            {
                alpha = r.sharedMaterial.GetColor("_TintColor").a;
                my_bubble.Add(r);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // we only want OUR bubble to be affected by their actual person, not their bubble
        if (other.isTrigger)
        {
            return;
        }

        // we know that ours is the collider, theirs is the trigger (our body in their space)
        Rigidbody other_rb = other.attachedRigidbody;

        // ignore self-collisions and repeated collisions
        if (other_rb == null || other_rb.CompareTag("Item") || my_rb == other_rb || in_my_space == other_rb)
        {
            return;
        }

        if (!(other_rb.name.Contains("XR") || other_rb.name.Contains("3rd")))
        {
            return;
        }

        // note who I am colliding with
        in_my_space = other_rb;
        // Debug.Log($"{transform.name}: {in_my_space.name} entered my bubble!");

        // just visualizations to color the renderers attached to the bubbles
        foreach (Renderer r in my_bubble)
        {
            r.material.SetColor("_TintColor", new Color(1, 0, 0, 4 * alpha));
        }

        if (!currently_touching.Contains(other))
        {
            currently_touching.Add(other);
        }
        // Data logging
        BaseTask.Instance.BeginIntersect(transform.name, in_my_space.name, Time.time);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    // non-efficient workaround to having a segment of the bubble exit while other bubbles are still touching the other

    //    OnTriggerEnter(other);
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        Rigidbody other_rb = other.attachedRigidbody;

        if (other_rb != null && !(other_rb.name.Contains("XR") || other_rb.name.Contains("3rd")))
        {
            return;
        }

        if (other_rb != null && other_rb == in_my_space)
        {
            if (currently_touching.Contains(other))
            {
                currently_touching.Remove(other);
            }
            // Data logging
            if (currently_touching.Count == 0)
            {
                try
                {
                    BaseTask.Instance.EndIntersect(transform.name, in_my_space.name, Time.time);
                }
                catch
                {
                    Debug.LogError("No BaseTask instance to log intersection to");
                }

                // Debug.Log($"{in_my_space.name} left my bubble.");

                in_my_space = null;

                // return visuals to normal
                foreach (Renderer r in my_bubble)
                {
                    r.material.SetColor("_TintColor", new Color(1, 1, 1, alpha));
                }
            }            
        }
    }
}
