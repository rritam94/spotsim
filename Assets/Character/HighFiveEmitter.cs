using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HighFiveEmitter : MonoBehaviour
{
    [SerializeField]
    //private AudioSource source;
    private float last_high_five_time;



    private void OnCollisionEnter(Collision collision)
    {
        Collider mine = collision.GetContact(0).thisCollider;
        Collider theirs = collision.GetContact(0).otherCollider;

        if (mine.name.Contains("Hand") && theirs.name.Contains("Hand"))
        {
            if (Time.time - last_high_five_time > 1)
            {
                last_high_five_time = Time.time;
                // we slap
                //source.Play();

                BaseTask.Instance.HighFive(GetParentName(mine.transform), GetParentName(theirs.transform), Time.time);
            }
            
            /* dirty check that 3rd person is doing high five animation
            if (Input.GetKey(KeyCode.H) || Input.GetKey(KeyCode.J))
            {
                
                // simple check that it's been at least a second since we've high fived, prevents rapid fire events 
                /*if (Time.time - last_high_five_time > 1)
                {
                    
                }
            }*/
        }    
    }

    private string GetParentName(Transform starter)
    {
        Transform parent = starter;
        while (parent.parent != null)
        {
            parent = parent.parent;
        }
        return parent.name;
    }
}
