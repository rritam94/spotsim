using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriesAndDrinkTask : BaseTask
{

    [SerializeField] private float number_of_trays;

    [SerializeField]
    private TMPro.TMP_Text timer;
    [SerializeField]
    private TMPro.TMP_Text physical_timer;
    private float start_time;

    public new static FriesAndDrinkTask Instance;

    protected override void Awake()
    {
        base.Awake();
        Instance = this as FriesAndDrinkTask;
    }

    public class TrayCompletion
    {
        public float index;
        public float start_time_of_event;
        public float end_time_of_event;
        public float net_time_of_event;

        public override string ToString()
        {
            return $"{start_time_of_event}, {end_time_of_event}, {net_time_of_event}";
        }

    }
    private List<TrayCompletion> in_progress_trays = new List<TrayCompletion>();
    private List<TrayCompletion> completed_trays = new List<TrayCompletion>();

    public void BeginTask()
    {
        start_time = Time.time;
        for (int i = 0; i < number_of_trays; i++)
        {
            TrayCompletion t = new TrayCompletion();
            t.index = i;
            t.start_time_of_event = Time.time;
            in_progress_trays.Add(t);
        }
    }

    private void Update()
    {
        if (in_progress_trays.Count == 0 && completed_trays.Count == 0)
        {
            timer.text = "Not Started";
            physical_timer.text = "Not Started";
        }
        else if (in_progress_trays.Count == 0)
        {
            timer.text = "High Five!";
            physical_timer.text = "High Five!";
        }
        else
        {
            timer.text = $"{completed_trays.Count}, {Time.time - start_time:000.00}";
            physical_timer.text = $"{Time.time - start_time:000.00}";
        }
    }


    public void EndATrayTask(int index)
    {
        TrayCompletion to_remove = null;
        foreach (TrayCompletion t in in_progress_trays)
        {
            if (t.index == index)
            {
                to_remove = t;
            }
        }

        if (to_remove != null)
        {
            to_remove.end_time_of_event = Time.time;
            to_remove.net_time_of_event = to_remove.end_time_of_event - to_remove.start_time_of_event;

            completed_trays.Add(to_remove);
            in_progress_trays.Remove(to_remove);

            LogList("completed_trays", completed_trays);
            DataRecorder.Instance.WriteRaw("Completed Tray", to_remove.ToString());
        }
    }

    public void ResetItems()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            Destroy(item);
        }

        DataRecorder.Instance.WriteRaw("Reset Items", "");
    }

    private void OnDestroy()
    {
        /* Metrics for arranging task are:
         * 
         *      Completion time
         *      Number of bubble violations (for participant)
         *      Duration of bubble violations (for participant)
         *      Num grabs
         *      Path length
         *      
         * */

        float completion_time = 0;
        float num_violations = 0;
        float duration_violations = 0;
        float num_grabs = 0;
        float path_length = 0;

        List<CoordinatesEvent> headset_events = new List<CoordinatesEvent>();
        foreach (CoordinatesEvent coords in coordinatesEvents)
        {
            if (coords.name.Contains("Head"))
            {
                headset_events.Add(coords);
            }
        }

        // we know start time, find end time
        float end_time = 0;
        foreach(TrayCompletion tc in completed_trays)
        {
            if (tc.end_time_of_event > end_time)
            {
                end_time = tc.end_time_of_event;
            }
        }

        completion_time = end_time - start_time;

        foreach (IntersectEvent intersect in intersectEvents)
        {
            if (intersect.start_time_of_event > start_time && intersect.start_time_of_event < end_time)
            {
                if (intersect.name_of_self.Contains("XR"))
                {
                    num_violations++;
                    duration_violations += intersect.net_time_of_event;
                }
            }
        }

        foreach (GrabEvent grab in grabEvents)
        {
            if (grab.time_of_event > start_time && grab.time_of_event < end_time)
            {
                if (grab.name_of_self.Contains("XR"))
                {
                    num_grabs++;
                }
            }
        }

        for (int i = 0; i < headset_events.Count - 1; i++)
        {
            CoordinatesEvent coords = headset_events[i];
            if (coords.timestamp > start_time && coords.timestamp < end_time)
            {
                Vector2 coords_xz = new Vector2(coords.position_x, coords.position_z);
                CoordinatesEvent next_coords = coordinatesEvents[i + 1];
                Vector2 next_coords_xz = new Vector2(next_coords.position_x, next_coords.position_z);

                path_length += Vector2.Distance(coords_xz, next_coords_xz);
            }
        }
        

        List<string> metrics = new List<string> { completion_time.ToString(), num_violations.ToString(), duration_violations.ToString(), num_grabs.ToString(), path_length.ToString() };

        DataRecorder.Instance.WriteTask("Arranging", metrics);
    }

}
