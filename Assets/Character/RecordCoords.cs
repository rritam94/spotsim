using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordCoords : MonoBehaviour
{
    [SerializeField]
    private string name = "name";

    private float step = 0.1f;
    private float next_time;

    private void Start()
    {
        next_time = Mathf.Ceil(Time.time) + step;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > next_time)
        {
            BaseTask.Instance.CoordinateEvent(name, next_time, transform.position, transform.rotation.eulerAngles);
            next_time += step;
        }
    }
}
