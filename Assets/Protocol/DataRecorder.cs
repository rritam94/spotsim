//using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class DataRecorder : MonoBehaviour
{
    // Other classes can access this one's methods with DataRecorder.Instance.<method>(<args>)
    public static DataRecorder Instance { get; private set; }

    // Define what data structures we are keeping track of here:
    [SerializeField] private string ID = "";
    string raw_file_name = "";
    string task_file_name = "";
    // ----

    // How I'm randomizing the bubble conditions
    private string[] bubble_conditions = { "Cylinder", "Suit", "Polygon" };
    private int condition_idx;

    public string GetCondition() { return bubble_conditions[condition_idx]; }

    // This gets called in post-survey, condition is incremented for the next cycle of tasks
    public void increment_condition() {
        condition_idx++;
        WriteRaw("Bubble Type", bubble_conditions[condition_idx]);
    }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("More than one instance of DataRecorder.  Destroying the non-static copy.");
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);

        CreateCSVs();

        string[] reshuffle(string[] texts)
        {
            // Knuth shuffle algorithm :: courtesy of Wikipedia :)
            for (int t = 0; t < texts.Length; t++)
            {
                string tmp = texts[t];
                int r = Random.Range(t, texts.Length);
                texts[t] = texts[r];
                texts[r] = tmp;
            }
            return texts;
        }

        bubble_conditions = reshuffle(bubble_conditions);
        WriteRaw("Bubble Type", bubble_conditions[condition_idx]);
    }

    // Define handler methods here:

    // ----

    private void CreateCSVs()
    {
        if (ID == "")
        {
            string dir = "./Data";
            string[] data_files = Directory.GetFiles(dir);
            HashSet<int> usedIDs = new HashSet<int>();
            //int max_id = -1;
            foreach (string path in data_files)
            {
                try
                {
                    string[] temp = path.Split('/', '\\');
                    string file = temp[temp.Length - 1];
                    string participant = file.Split('.', '_')[0];
                    int id = int.Parse(participant);
                    usedIDs.Add(id);
                    /*
                    if (id > max_id)
                    {
                        max_id = id;
                    }*/
                }
                catch
                {
                    Debug.LogError($"{path} can not be parsed");
                }
            }
            /*max_id++;

            ID = max_id.ToString();
            generate random ids till an unused one is found
             */

            System.Random rnd = new System.Random();
            int tryId = rnd.Next(1, 100);
            while (usedIDs.Contains(tryId))
            {
                tryId = rnd.Next(1, 100);
            }
            ID = tryId.ToString();
        }
        Debug.Log(ID);

        raw_file_name = $"./Data/{ID}_raw.csv";
        task_file_name = $"./Data/{ID}_tasks.csv";

        if (!File.Exists(raw_file_name))
        {
            using (StreamWriter raw_file_SW = File.CreateText(raw_file_name))
            {
                //Write header
                raw_file_SW.WriteLine("Participant ID, Bubble Type, Time, Event Type, Event Data");
                raw_file_SW.Flush();
            }
        }
        if (!File.Exists(task_file_name))
        {
            using (StreamWriter task_file_SW = File.CreateText(task_file_name))
            {
                //Write header
                task_file_SW.WriteLine("Participant ID, Bubble Type, Task, Metric 1, Metric 2, Metric 3, Metric 4, Metric 5");
                task_file_SW.Flush();
            }
        }
    }

    public void WriteRaw(string event_type, string data)
    {
        string output = $"{ID},{GetCondition()},{Time.time},{event_type},\"{data}\"";
        using (StreamWriter raw_file_SW = File.AppendText(raw_file_name))
        {
            raw_file_SW.WriteLine(output);
            raw_file_SW.Flush();
        }
    }

    public void WriteTask(string event_type, List<string> metrics)
    {
        string m;
        if (metrics.Count == 0)
        {
            m = "";
        }
        else
        {
            m = metrics[0];
        }
        for (int i = 1; i < metrics.Count; i++)
        {
            m += $",{metrics[i]}";
        }

        string output = $"{ID},{GetCondition()},{event_type},{m}";

        using (StreamWriter task_file_SW = File.AppendText(task_file_name))
        {
            task_file_SW.WriteLine(output);
            task_file_SW.Flush();
        }
    }

    // Test method.  If another class calls "DataRecorder.Instance.TestLog(this, <message>);", we will log to console who they are and their message.
    public void TestLog(Object obj, string msg)
    {
        Debug.Log($"[{obj.name}]  {msg}");

    }
}
