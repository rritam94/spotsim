using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandwichTask : BaseTask
{
    public new static SandwichTask Instance;

    // Instance variables
    [SerializeField]
    private int target_number_of_sandwiches;

    [SerializeField]
    private TMPro.TMP_Text timer;
    [SerializeField]
    private TMPro.TMP_Text physical_timer;

    public class SandwichCreationCycle
    {
        public float num_intersections;
        public float intersection_time;

        public float num_ingredient_grabs;

        public float start_time_of_event;
        public float end_time_of_event;
        public float net_time_of_event;

        public override string ToString()
        {
            return $"{num_intersections}, {intersection_time}, {num_ingredient_grabs}, {start_time_of_event}, {end_time_of_event}, {net_time_of_event}";
        }

    }
    public List<SandwichCreationCycle> sandwichCreationCycles = new List<SandwichCreationCycle>();

    // TODO: Link this with a start button (or high five interaction)
    public SandwichCreationCycle currentSandwichCycle;
    private int num_intersections_at_cycle_start;
    private int num_ingredient_grabs_at_cycle_start;

    public class HighFiveWindow
    {
        public float start_time_of_window;
        public float end_time_of_window;
        public float net_time_of_window;

        public override string ToString()
        {
            return $"{start_time_of_window}, {end_time_of_window}, {net_time_of_window}";
        }

    }
    public List<HighFiveWindow> highFiveWindows = new List<HighFiveWindow>();
    public HighFiveWindow currentHighFive;

    protected override void Awake()
    {
        base.Awake();
        Instance = this as SandwichTask;
    }

    private void Start()
    {
        // InitializeHighFiveWindow();
    }

    private void Update()
    {
        if (currentSandwichCycle == null && currentHighFive == null)
        {
            timer.text = "No SW/HF Active";
            physical_timer.text = "";
        }
        else if (currentSandwichCycle == null)
        {
            timer.text = "High Five!";
            physical_timer.text = "High Five!";
        }
        else
        {
            timer.text = $"{sandwichCreationCycles.Count}, {Time.time - currentSandwichCycle.start_time_of_event:000.00}";
            physical_timer.text = $"{Time.time - currentSandwichCycle.start_time_of_event:000.00}";
        }

        if (sandwichCreationCycles.Count >= target_number_of_sandwiches)
        {
            // TODO: Logic to end sandwich task
            timer.text = "Finished!";
            physical_timer.text = "Finished!";
        }
    }

    public void InitializeCurrentSandwichCycle()
    {
        currentSandwichCycle = new SandwichCreationCycle();
        currentSandwichCycle.start_time_of_event = Time.time;

        num_intersections_at_cycle_start = intersectEvents.Count;
        num_ingredient_grabs_at_cycle_start = grabEvents.Count;
    }

    public void EndCurrentSandwichCycle()
    {
        currentSandwichCycle.end_time_of_event = Time.time;
        currentSandwichCycle.net_time_of_event = currentSandwichCycle.end_time_of_event - currentSandwichCycle.start_time_of_event;

        // compute number of intersections
        currentSandwichCycle.num_intersections = intersectEvents.Count - num_intersections_at_cycle_start;
        // how long were we intersected?
        float elapsed_intersect_time = 0f;
        for (int i = num_intersections_at_cycle_start; i < intersectEvents.Count; i++)
        {
            elapsed_intersect_time += intersectEvents[i].net_time_of_event;
        }
        currentSandwichCycle.intersection_time = elapsed_intersect_time;
        // number of grab actions
        currentSandwichCycle.num_ingredient_grabs = grabEvents.Count - num_ingredient_grabs_at_cycle_start;

        sandwichCreationCycles.Add(currentSandwichCycle);
        LogList("sandwichCreationCycles", sandwichCreationCycles);
        DataRecorder.Instance.WriteRaw("Sandwich Created", currentSandwichCycle.ToString());

        currentSandwichCycle = null;
        InitializeHighFiveWindow();
    }

    public void ResetCurrentSandwichCycle()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        for (int i = items.Length - 1; i >= 0; i--)
        {
            GameObject item = items[i];
            Destroy(item);
        }

        DataRecorder.Instance.WriteRaw("Reset Sandwich Cycle", "");
        InitializeCurrentSandwichCycle();
    }

    public void InitializeHighFiveWindow()
    {
        currentHighFive = new HighFiveWindow();
        currentHighFive.start_time_of_window = Time.time;
    }

    public void EndHighFiveWindow()
    {
        currentHighFive.end_time_of_window = Time.time;
        currentHighFive.net_time_of_window = currentHighFive.end_time_of_window - currentHighFive.start_time_of_window;

        highFiveWindows.Add(currentHighFive);
        LogList("highFiveWindows", highFiveWindows);
        DataRecorder.Instance.WriteRaw("High Five Window Ended", currentHighFive.ToString());

        currentHighFive = null;
    }

    public override void HighFive(string name_of_self, string name_of_other, float time)
    {
        base.HighFive(name_of_self, name_of_other, time);

        if (currentHighFive != null)
        {
            EndHighFiveWindow();
        }
    }

    private void OnDestroy()
    {
        /* Metrics for sandwich task are:
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
        foreach(CoordinatesEvent coords in coordinatesEvents)
        {
            if (coords.name.Contains("Head"))
            {
                headset_events.Add(coords);
            }
        }


        foreach (SandwichCreationCycle swCycle in sandwichCreationCycles)
        {
            completion_time += swCycle.net_time_of_event;

            float start_time = swCycle.start_time_of_event;
            float end_time = swCycle.end_time_of_event;

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
        }

        List<string> metrics = new List<string> { completion_time.ToString(), num_violations.ToString(), duration_violations.ToString(), num_grabs.ToString(), path_length.ToString() };

        DataRecorder.Instance.WriteTask("Sandwich", metrics);


        /* Metrics for high five task are:
         * 
         *      Completion time
         *      Number of bubble violations (for participant)
         *      Duration of bubble violations (for participant)
         *      Num grabs
         *      Path length
         *      
         * */

        completion_time = 0;
        num_violations = 0;
        duration_violations = 0;
        num_grabs = 0;
        path_length = 0;

        foreach (HighFiveWindow hfWindow in highFiveWindows)
        {
            completion_time += hfWindow.net_time_of_window;

            float start_time = hfWindow.start_time_of_window;
            float end_time = hfWindow.end_time_of_window;

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
        }

        metrics = new List<string> { completion_time.ToString(), num_violations.ToString(), duration_violations.ToString(), num_grabs.ToString(), path_length.ToString() };

        DataRecorder.Instance.WriteTask("High Five", metrics);


    }

}
