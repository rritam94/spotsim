using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseTask : AbstractTask<BaseTask>
{
    public class IntersectEvent
    {
        public string name_of_self;
        public string name_of_other;
        public float start_time_of_event;
        public float end_time_of_event;
        public float net_time_of_event;

        public override string ToString()
        {
            return $"{name_of_self}, {name_of_other}, {start_time_of_event}, {end_time_of_event}, {net_time_of_event}";
            //return $"[{name_of_self}, {name_of_other}] {net_time_of_event}s";
        }
    }
    public List<IntersectEvent> intersectEvents = new List<IntersectEvent>();
    public List<IntersectEvent> nextIntersectEvents = new List<IntersectEvent>();

    public class GrabEvent
    {
        public string name_of_self;
        public string name_of_item;
        public float time_of_event;

        public override string ToString()
        {
            return $"{name_of_self}, {name_of_item}, {time_of_event}";
        }
    }
    public List<GrabEvent> grabEvents = new List<GrabEvent>();

    public class HighFiveEvent
    {
        public string name_of_self;
        public string name_of_other;
        public float time_of_event;

        public override string ToString()
        {
            return $"{name_of_self}, {name_of_other}, {time_of_event}";
        }
    }
    public List<HighFiveEvent> highFiveEvents = new List<HighFiveEvent>();

    public class CoordinatesEvent
    {
        public string name;
        public float timestamp;
        public float position_x;
        public float position_y;
        public float position_z;
        public float rotation_x;
        public float rotation_y;
        public float rotation_z;

        public override string ToString()
        {
            return $"{name}, {timestamp}, {position_x}, {position_y}, {position_z}, {rotation_x}, {rotation_y}, {rotation_z}";
        }
    }
    public List<CoordinatesEvent> coordinatesEvents = new List<CoordinatesEvent>();

    public void BeginIntersect(string name_of_self, string name_of_other, float time)
    {
        // check to see if this event has already been recorded (due to a second body part sending a message)
        foreach (IntersectEvent ie in nextIntersectEvents)
        {
            if (ie.name_of_self.Equals(name_of_self) && ie.name_of_other.Equals(name_of_other))
            {
                // do not log
                return;
            }
        }

        IntersectEvent newIntersect = new IntersectEvent();
        newIntersect.name_of_self = name_of_self;
        newIntersect.name_of_other = name_of_other;
        newIntersect.start_time_of_event = time;

        nextIntersectEvents.Add(newIntersect);
    }

    public void EndIntersect(string name_of_self, string name_of_other, float time)
    {
        // look for the appropriate message
        for (int i = 0; i < nextIntersectEvents.Count; i++)
        {
            IntersectEvent ie = nextIntersectEvents[i];
            if (ie.name_of_self.Equals(name_of_self) && ie.name_of_other.Equals(name_of_other))
            {
                ie.end_time_of_event = time;
                ie.net_time_of_event = ie.end_time_of_event - ie.start_time_of_event;

                intersectEvents.Add(ie);
                nextIntersectEvents.RemoveAt(i);
                LogList("nextIntersectEvents", nextIntersectEvents);
                LogList("intersectEvents", intersectEvents);

                DataRecorder.Instance.WriteRaw("Intersect", ie.ToString());

                return;
            }
        }
    }

    public void Grab(string name_of_self, string name_of_item, float time)
    {
        GrabEvent gE = new GrabEvent();
        gE.name_of_self = name_of_self;
        gE.name_of_item = name_of_item;
        gE.time_of_event = time;

        grabEvents.Add(gE);

        LogList("grabEvents", grabEvents);
        DataRecorder.Instance.WriteRaw("Grab", gE.ToString());

    }

    public virtual void HighFive(string name_of_self, string name_of_other, float time)
    {
        HighFiveEvent hFE = new HighFiveEvent();
        hFE.name_of_self = name_of_self;
        hFE.name_of_other = name_of_other;
        hFE.time_of_event = time;

        highFiveEvents.Add(hFE);

        LogList("highFiveEvents", highFiveEvents);
        DataRecorder.Instance.WriteRaw("High Five", hFE.ToString());

    }

    public void CoordinateEvent(string name, float timestamp, Vector3 position, Vector3 rotation)
    {
        CoordinatesEvent coE = new CoordinatesEvent();
        coE.name = name;
        coE.timestamp = timestamp;
        coE.position_x = position.x;
        coE.position_y = position.y;
        coE.position_z = position.z;
        coE.rotation_x = rotation.x;
        coE.rotation_y = rotation.y;
        coE.rotation_z = rotation.z;

        coordinatesEvents.Add(coE);

        DataRecorder.Instance.WriteRaw("Coordinates", coE.ToString());
    }

    public void LoadScene(string name)
    {
        DataRecorder.Instance.WriteRaw("Scene change", name);
        SceneManager.LoadScene(name);
    }

    protected void LogList<T>(string title, List<T> li)
    {
        string result = $"{title}: ";
        foreach (var item in li)
        {
            result += item.ToString();
        }
        Debug.Log(result);
    }
}
