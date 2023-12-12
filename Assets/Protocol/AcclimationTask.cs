using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcclimationTask : BaseTask
{
    public new static AcclimationTask Instance;

    // Instance variables
    [SerializeField] private GameObject dot_origin;
    [SerializeField] private GameObject dot_0_5;
    [SerializeField] private GameObject dot_1;
    [SerializeField] private GameObject dot_2;

    [SerializeField] private GameObject VR_Character;
    private GameObject VR_Camera;
    [SerializeField] private GameObject AI_character;

    [SerializeField]
    private TMPro.TMP_Text timer;
    [SerializeField]
    private int target_number_of_sandwiches;
    private int condition = 0;

    private Transform AI_dot;
    private Transform VR_dot;

    public class ComfortTask
    {
        public float distance;
        public float start_time_of_event;
        public float end_time_of_event;
        public float net_time_of_event;
        public float likert_response;

        public override string ToString()
        {
            return $"{distance}, {likert_response}, {start_time_of_event}, {end_time_of_event}, {net_time_of_event}";
        }
    }
    public List<ComfortTask> comfortTasks = new List<ComfortTask>();
    public ComfortTask currentComfortTask;
    public class WalkingComfortTask
    {
        public float distance;
        public float start_time_of_event;
        public float end_time_of_event;
        public float net_time_of_event;

        public override string ToString()
        {
            return $"{distance}, {start_time_of_event}, {end_time_of_event}, {net_time_of_event}";
        }
    }
    public List<WalkingComfortTask> walkingComfortTasks = new List<WalkingComfortTask>();
    public WalkingComfortTask currentWalkingComfortTask;

    public class AcclimationSandwichCreationCycle
    {
        public float num_ingredient_grabs;

        public float start_time_of_event;
        public float end_time_of_event;
        public float net_time_of_event;

        public override string ToString()
        {
            return $"{num_ingredient_grabs}, {start_time_of_event}, {end_time_of_event}, {net_time_of_event}";
        }

    }
    public List<AcclimationSandwichCreationCycle> sandwichCreationCycles = new List<AcclimationSandwichCreationCycle>();

    public AcclimationSandwichCreationCycle currentSandwichCycle;
    private int num_ingredient_grabs_at_cycle_start;

    protected override void Awake()
    {
        base.Awake();
        Instance = this as AcclimationTask;
    }

    private void Start()
    {
        VR_Camera = VR_Character.GetComponentInChildren<Camera>().gameObject;
    }

    private void Update()
    {
        InitializeComfortTask(this.condition);
        //moveSpot();
        if (currentSandwichCycle == null)
        {
            timer.text = "Press to start SW task";
        }
        else
        {
            timer.text = $"{sandwichCreationCycles.Count} SW made, {Time.time - currentSandwichCycle.start_time_of_event:000.00}";
        }


        if (sandwichCreationCycles.Count >= target_number_of_sandwiches)
        {
            // TODO: Logic to end sandwich task
            timer.text = "Finished SW task";
        }
    }

    public void InitializeComfortTask(int condition)

    {

        // prevents overlap
        //CancelWalkingComfortTask();
        GameObject spot1 = GameObject.Find("spot1");
        GameObject base_link = GameObject.Find("base_link");
        GameObject body = GameObject.Find("body");
        GameObject rear_rail = GameObject.Find("rear_rail");
        GameObject front_rail = GameObject.Find("front_rail");
        GameObject rear_right_hip = GameObject.Find("rear_right_hip");
        GameObject rear_left_hip = GameObject.Find("rear_left_hip");
        GameObject rear_right_upper_leg = GameObject.Find("rear_right_upper_leg");
        GameObject rear_right_lower_leg = GameObject.Find("rear_right_lower_leg");
        GameObject rear_left_upper_leg = GameObject.Find("rear_left_upper_leg");
        GameObject rear_left_lower_leg = GameObject.Find("rear_left_lower_leg");
        GameObject front_right_hip = GameObject.Find("front_right_hip");
        GameObject front_lower_hip = GameObject.Find("front_lower_hip");
        GameObject front_right_upper_leg = GameObject.Find("front_right_upper_leg");
        GameObject front_right_lower_leg = GameObject.Find("front_right_lower_leg");
        GameObject front_left_upper_leg = GameObject.Find("front_left_upper_leg");
        GameObject front_left_lower_leg = GameObject.Find("front_left_lower_leg");
        GameObject unnamed = GameObject.Find("unnamed");



        currentComfortTask = new ComfortTask();
        currentComfortTask.start_time_of_event = Time.time;

        VR_dot = dot_origin.transform;
        if (condition == 0)
        {
            spot1.transform.position = new Vector3(0, 1, 2);
            base_link.transform.position = new Vector3(0, 1, 2);
            body.transform.position = new Vector3(0, 1, 2);
            rear_rail.transform.position = new Vector3(0, 1, 2);
            front_rail.transform.position = new Vector3(0, 1, 2);
            rear_right_hip.transform.position = new Vector3(0, 1, 2);
            rear_left_hip.transform.position = new Vector3(0, 1, 2);
            rear_right_upper_leg.transform.position = new Vector3(0, 1, 2);
            rear_right_lower_leg.transform.position = new Vector3(0, 1, 2);
            rear_left_upper_leg.transform.position = new Vector3(0, 1, 2);
            rear_left_lower_leg.transform.position = new Vector3(0, 1, 2);
            front_right_hip.transform.position = new Vector3(0, 1, 2);
            front_lower_hip.transform.position = new Vector3(0, 1, 2);
            front_right_upper_leg.transform.position = new Vector3(0, 1, 2);
            front_right_lower_leg.transform.position = new Vector3(0, 1, 2);
            front_left_upper_leg.transform.position = new Vector3(0, 1, 2);
            front_left_lower_leg.transform.position = new Vector3(0, 1, 2);
            unnamed.transform.position = new Vector3(0, 1, 2);
        }

        if (condition == 1)
        {
            Debug.Log("in here");

            spot1.transform.position = new Vector3(0, 1, -0.5f);
            base_link.transform.position = new Vector3(0, 1, -0.5f);
            body.transform.position = new Vector3(0, 1, -0.5f);
            rear_rail.transform.position = new Vector3(0, 1, -0.5f);
            front_rail.transform.position = new Vector3(0, 1, -0.5f);
            rear_right_hip.transform.position = new Vector3(0, 1, -0.5f);
            rear_left_hip.transform.position = new Vector3(0, 1, -0.5f);
            rear_right_upper_leg.transform.position = new Vector3(0, 1, -0.5f);
            rear_right_lower_leg.transform.position = new Vector3(0, 1, -0.5f);
            rear_left_upper_leg.transform.position = new Vector3(0, 1, -0.5f);
            rear_left_lower_leg.transform.position = new Vector3(0, 1, -0.5f);
            front_right_hip.transform.position = new Vector3(0, 1, -0.5f);
            front_lower_hip.transform.position = new Vector3(0, 1, -0.5f);
            front_right_upper_leg.transform.position = new Vector3(0, 1, -0.5f);
            front_right_lower_leg.transform.position = new Vector3(0, 1, -0.5f);
            front_left_upper_leg.transform.position = new Vector3(0, 1, -0.5f);
            front_left_lower_leg.transform.position = new Vector3(0, 1, -0.5f);
            unnamed.transform.position = new Vector3(0, 1, -0.5f);
        }
        if (condition == 2)
        {
            spot1.transform.position = new Vector3(0, 1, -1);
            base_link.transform.position = new Vector3(0, 1, -1);
            body.transform.position = new Vector3(0, 1, -1);
            rear_rail.transform.position = new Vector3(0, 1, -1);
            front_rail.transform.position = new Vector3(0, 1, -1);
            rear_right_hip.transform.position = new Vector3(0, 1, -1);
            rear_left_hip.transform.position = new Vector3(0, 1, -1);
            rear_right_upper_leg.transform.position = new Vector3(0, 1, -1);
            rear_right_lower_leg.transform.position = new Vector3(0, 1, -1);
            rear_left_upper_leg.transform.position = new Vector3(0, 1, -1);
            rear_left_lower_leg.transform.position = new Vector3(0, 1, -1);
            front_right_hip.transform.position = new Vector3(0, 1, -1);
            front_lower_hip.transform.position = new Vector3(0, 1, -1);
            front_right_upper_leg.transform.position = new Vector3(0, 1, -1);
            front_right_lower_leg.transform.position = new Vector3(0, 1, -1);
            front_left_upper_leg.transform.position = new Vector3(0, 1, -1);
            front_left_lower_leg.transform.position = new Vector3(0, 1, -1);
            unnamed.transform.position = new Vector3(0, 1, -1);
        }


        // obj.transform.position = AI_dot.position;

        //obj.transform.LookAt(VR_dot);
        // LockCharacter();
    }

    public void getCondition(int condition)

    {
        this.condition = condition;
    }

    public void moveSpot()
    {
        // prevents overlap
        
        GameObject obj = GameObject.Find("spot1");
       
        if(obj != null)
        {
            obj.transform.position = new Vector3(0, 0, 0);
        }
    }

    public void EndComfortTask(int likert_reponse)
    {
        currentComfortTask.end_time_of_event = Time.time;
        currentComfortTask.net_time_of_event = currentComfortTask.end_time_of_event - currentComfortTask.start_time_of_event;

        currentComfortTask.likert_response = likert_reponse;

        comfortTasks.Add(currentComfortTask);

        // temp
        LogList("comfortTasks", comfortTasks);
        DataRecorder.Instance.WriteRaw("Comfort Task", currentComfortTask.ToString());

        currentComfortTask = null;

        AI_character.transform.position = new Vector3(100, 0, 0);
        // We stay locked until the walking task begins
        // UnlockCharacter();
    }

    public void CancelComfortTask()
    {
        currentComfortTask = null;

        AI_character.transform.position = new Vector3(100, 0, 0);
        UnlockCharacter();
    }

    public void BeginWalkingComfortTask()
    {
        // prevents overlap
        CancelComfortTask();

        VR_dot = dot_origin.transform;
        AI_dot = dot_2.transform;

        AI_character.transform.position = AI_dot.position;
        AI_character.transform.LookAt(VR_dot);

        LockCharacter();
        UnlockCharacter();

        currentWalkingComfortTask = new WalkingComfortTask();
        currentWalkingComfortTask.start_time_of_event = Time.time;
    }
    public void EndWalkingComfortTask()
    {
        currentWalkingComfortTask.end_time_of_event = Time.time;
        currentWalkingComfortTask.net_time_of_event = currentWalkingComfortTask.end_time_of_event - currentWalkingComfortTask.start_time_of_event;

        Vector3 VR_position = new Vector3(VR_Camera.transform.position.x, 0, VR_Camera.transform.position.z);
        currentWalkingComfortTask.distance = Vector3.Magnitude(AI_dot.position - VR_position);

        walkingComfortTasks.Add(currentWalkingComfortTask);

        // temp
        LogList("walkingComfortTasks", walkingComfortTasks);
        DataRecorder.Instance.WriteRaw("Walking Comfort Task", currentWalkingComfortTask.ToString());

        currentWalkingComfortTask = null;

        AI_character.transform.position = new Vector3(100, 0, 0);
    }

    public void CancelWalkingComfortTask()
    {
        currentWalkingComfortTask = null;

        AI_character.transform.position = new Vector3(100, 0, 0);
        UnlockCharacter();
    }

    private void LockCharacter()
    {
        foreach (UnityEngine.SpatialTracking.TrackedPoseDriver tpd in VR_Character.GetComponentsInChildren<UnityEngine.SpatialTracking.TrackedPoseDriver>())
        {
            tpd.trackingType = UnityEngine.SpatialTracking.TrackedPoseDriver.TrackingType.RotationOnly;
        }
        VR_Camera.transform.localPosition = new Vector3(0, 1.5f, 0);
        VR_Camera.transform.parent.localPosition = Vector3.zero;


        VR_Character.transform.position = new Vector3(
                VR_dot.position.x,
                VR_Character.transform.position.y,
                VR_dot.position.z);
    }

    private void UnlockCharacter()
    {
        foreach (UnityEngine.SpatialTracking.TrackedPoseDriver tpd in VR_Character.GetComponentsInChildren<UnityEngine.SpatialTracking.TrackedPoseDriver>())
        {
            tpd.trackingType = UnityEngine.SpatialTracking.TrackedPoseDriver.TrackingType.RotationAndPosition;
        }
    }

    public void InitializeCurrentSandwichCycle()
    {
        currentSandwichCycle = new AcclimationSandwichCreationCycle();
        currentSandwichCycle.start_time_of_event = Time.time;

        num_ingredient_grabs_at_cycle_start = grabEvents.Count;
    }

    public void EndCurrentSandwichCycle()
    {
        currentSandwichCycle.end_time_of_event = Time.time;
        currentSandwichCycle.net_time_of_event = currentSandwichCycle.end_time_of_event - currentSandwichCycle.start_time_of_event;

        // number of grab actions
        currentSandwichCycle.num_ingredient_grabs = grabEvents.Count - num_ingredient_grabs_at_cycle_start;

        sandwichCreationCycles.Add(currentSandwichCycle);
        LogList("sandwichCreationCycles", sandwichCreationCycles);
        DataRecorder.Instance.WriteRaw("Acclimation Sandwich Created", currentSandwichCycle.ToString());

        InitializeCurrentSandwichCycle();
    }

    public void ResetCurrentSandwichCycle()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        for (int i = items.Length - 1; i >= 0; i--)
        {
            GameObject item = items[i];
            Destroy(item);
        }

        DataRecorder.Instance.WriteRaw("Acclimation Reset Sandwich Cycle", "");
        InitializeCurrentSandwichCycle();
    }

    private void OnDestroy()
    {
        /* Metrics are:
         * 
         *      Likert response @ 0.5M
         *      Likert response @ 1M
         *      Likert response @ 2M
         *      Distance @ Walking task
         *      
         * */

        float likert05 = -1;
        float likert1 = -1;
        float likert2 = -1;

        // the way this logic is set up, used LAST recorded tasks (if any weirdness happens where there are more than there should be)
        
        foreach (ComfortTask task in comfortTasks)
        {
            if (task.distance == 0.5f) { likert05 = task.likert_response; }
            if (task.distance == 1f) { likert1 = task.likert_response; }
            if (task.distance == 2f) { likert2= task.likert_response; }
        }

        float walking_distance = -1;

        foreach (WalkingComfortTask task in walkingComfortTasks)
        {
            walking_distance = task.distance;
        }

        List<string> metrics = new List<string> { likert05.ToString(), likert1.ToString(), likert2.ToString(), walking_distance.ToString() };

        DataRecorder.Instance.WriteTask("Acclimation", metrics);
    }

}