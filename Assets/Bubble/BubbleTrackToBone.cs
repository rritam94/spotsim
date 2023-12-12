using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTrackToBone : MonoBehaviour
{
    [SerializeField] private Transform trackTo;

    private Vector3 offset;

    private void Start()
    {
        offset = trackTo.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = trackTo.position - offset;
    }
}
