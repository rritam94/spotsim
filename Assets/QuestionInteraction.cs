using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Question
{
    public string question;
    public int min;
    public int max;
    public string minDescription;
    public string maxDescription;

    public string response;


    public Question(string q, int min, int max, string minDescription, string maxDescription)
    {
        question = q;
        this.min = min;
        this.max = max;
        this.minDescription = minDescription;
        this.maxDescription = maxDescription;

        response = "Not answered";
    }
}

public class QuestionInteraction : MonoBehaviour
{
    private List<Question> questions;
    private int currQuestion;
    public Canvas questionCanvas;
    public TextMeshProUGUI displayQuestion;
    public TextMeshProUGUI min;
    public TextMeshProUGUI max;
    public GameObject plane;
    private string selection;
    private float lastTime;
    private GameObject lastHit;
    public bool proceed;

    private bool input;
    private Color defaultColor;
    private Color selectedColor;
    private Color startColor;

    private LineRenderer rayLine;


    public bool buttonDown = false;
    void Start()
    {
        defaultColor = new Color(0, 0.8f, 0.4f);
        selectedColor = new Color(0.4f, 1.0f, 0.4f);
        startColor = new Color(1.0f, 1.0f, 1.0f);

        lastTime = 0.0f;
        proceed = false;
        selection = "";
        currQuestion = 0;

        questionCanvas.enabled = false;
        questions = new List<Question>();
        //nasa tlx
        questions.Add(new Question("How mentally demanding was the task?", 1, 10, "Low", "High"));
        questions.Add(new Question("How physically demanding was the task?", 1, 10, "Low", "High"));
        questions.Add(new Question("How hurried or rushed was the pace of the overall experience?", 1, 10, "Low", "High"));
        questions.Add(new Question("How successful were you in accomplishing what you were asked to do in the overall experience?", 1, 10, "Low", "High"));
        questions.Add(new Question("How hard did you have to work to accomplish your level of performance in the overall experience?", 1, 10, "Low", "High"));
        questions.Add(new Question("How insecure, discouraged, irritated, stressed and annoyed were you with the overall experience?", 1, 10, "Low", "High"));
        //psial
        questions.Add(new Question("I felt self-confident when interacting with the other avatar at a close distance.", 1,5, "Strongly Disagree", "Strongly Agree"));
        questions.Add(new Question("I dreaded it whenever the other avatar moved towards me.", 1, 5, "Strongly Disagree", "Strongly Agree"));
        questions.Add(new Question("It made me feel apprehensive when I saw the other avatar close by. ", 1, 5, "Strongly Disagree", "Strongly Agree"));
        questions.Add(new Question("The other avatar made me feel nervous when they approached me. ", 1, 5, "Strongly Disagree", "Strongly Agree"));
        questions.Add(new Question("I felt uneasy being next to the other avatar.", 1, 5, "Strongly Disagree", "Strongly Agree"));
        questions.Add(new Question("It was irritating when the other avatar was close to me. ", 1, 5, "Strongly Disagree", "Strongly Agree"));
        questions.Add(new Question("I felt annoyed when the other avatar stood next to me.", 1, 5, "Strongly Disagree", "Strongly Agree"));
        //naturalness
        questions.Add(new Question("How natural did your interactions with the other avatar seem?", 1, 7, "Extremely artificial", "Completely natural"));
        //physical presence
        questions.Add(new Question("When moving through the virtual environment I feel as if I am actually there.", 1, 7, "Do not agree", "Strongly agree"));
        questions.Add(new Question("I had reactions to events and characters in the virtual environment as if they were real.", 1, 7, "Do not agree", "Strongly agree"));
        //Social presence
        questions.Add(new Question("To what extent was this like you were in the same room with the virtual character?", 1, 7, "Not like being in the same room at all", "A lot like being in the same room"));
        questions.Add(new Question("To what extent did you feel you could establish a working relationship with someone that you met only through this system?", 1, 7, "Not at all", "Very well"));
        //team cohesion
        questions.Add(new Question("I enjoyed working with my teammates.", 1,5, "Strongly Disagree", "Strongly Agree"));
        questions.Add(new Question("The team worked well together.", 1, 5, "Strongly Disagree", "Strongly Agree"));
        questions.Add(new Question("Everyone contributed to the discussion.", 1, 5, "Strongly Disagree", "Strongly Agree"));
        questions.Add(new Question("The team wasted a lot of time. ", 1, 5, "Strongly Disagree", "Strongly Agree"));

        displayQuestion.text = questions[currQuestion].question;

        rayLine = GetComponent<LineRenderer>();
        rayLine.startWidth = 0.05f;
        rayLine.endWidth = 0.05f;
        rayLine.enabled = false;
    }
    public void OnSelect(InputValue context)
    {
        Debug.Log("button pressed");
        if (questionCanvas.enabled)
        {
            input = context.Get<float>() == 1f;
            Debug.Log(input);
            buttonDown = input;

            Ray ray = new Ray(transform.position, transform.forward);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Text" && questionCanvas.enabled)
                {
                    Debug.Log(hit.transform.gameObject.name);
                    //if (hit.transform.gameObject.name == "Plane" && selection != "" && Time.time > lastTime + 0.5f)
                    if (hit.transform.gameObject.name == "Plane" && selection != "" && Time.time > lastTime + 0.25f)
                    {
                        //check if they have answered the question and time has passed, if so proceed
                        Debug.Log("canvas pressed");
                        questions[currQuestion].response = selection;

                        IncrementQuestion();
                    }
                    else if (hit.transform.gameObject.name != "Plane" && Time.time > lastTime + 0.25f)
                    {
                        lastTime = Time.time;
                        selection = hit.transform.gameObject.name;
                        displayQuestion.text = questions[currQuestion].question + " You selected " + selection + ". Click the whiteboard to proceed, or change your selection.";
                    }

                }
            }
        }
       
    }

    void Update()
    {
        /*Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 10);
        */
        //experimenter determines tasks are completed
        if (Input.GetKeyDown(KeyCode.RightArrow) && !questionCanvas.enabled)
        {
            questionCanvas.enabled = true;
            currQuestion = -1;
            IncrementQuestion();
                
            selection = "";
            proceed = false;
            rayLine.enabled = true;

            SetButtonColors(defaultColor);
        }

        if (questionCanvas.enabled && !rayLine.enabled)
            rayLine.enabled = true;

        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(rayLine.enabled)
                rayLine.SetPositions(new Vector3[] { ray.origin, hit.point });
            if (hit.transform.gameObject.tag == "Text" && hit.transform.gameObject.name != "Plane" && questionCanvas.enabled)
            {
                
                if (lastHit != null)
                    lastHit.GetComponent<Renderer>().material.SetColor("_Color", defaultColor);
                lastHit = hit.transform.gameObject;
                var planeRenderer = lastHit.GetComponent<Renderer>();
                planeRenderer.material.SetColor("_Color", selectedColor);
            }
            else if(lastHit != null && selection != lastHit.name) //change to: else if(lastHit != null && selection != lastHit.name)
            {
                //reset lastHit to old color
                var planeRenderer = lastHit.GetComponent<Renderer>();
                planeRenderer.material.SetColor("_Color", defaultColor);
                lastHit = null;
            }
        }

    }

    private void IncrementQuestion()
    {
        currQuestion++;
        if (currQuestion < questions.Count)
        {
            displayQuestion.text = questions[currQuestion].question;
            min.text = questions[currQuestion].minDescription;
            max.text = questions[currQuestion].maxDescription;
            selection = "";

            //set children active or inactive, fix spacing
            for (int i = 0; i < questionCanvas.transform.childCount; i++)
            {
                GameObject number = questionCanvas.transform.GetChild(i).gameObject;

                if (number.tag == "Text" && number.name != "Plane")
                {
                    if (int.Parse(number.name) <= questions[currQuestion].max)
                    {
                        number.SetActive(true);
                        // left-aligned
                        number.transform.localPosition = new Vector3(
                            -850 + (int.Parse(number.name) * 1500 / questions[currQuestion].max), number.transform.localPosition.y, number.transform.localPosition.z);
                    }
                    else
                    {
                        number.SetActive(false);
                    }
                }
            }
        }
        else
        {
            SetButtonColors(startColor);

            //flush data here
            proceed = true;
            questionCanvas.enabled = false;
            rayLine.enabled = false;

            WriteResponses();
        }
    }

    private void SetButtonColors(Color color)
    {
        for (int i = 0; i < questionCanvas.transform.childCount; i++)
        {
            if (questionCanvas.transform.GetChild(i).gameObject.tag == "Text" && questionCanvas.transform.GetChild(i).gameObject.name != "Plane")
            {
                var planeRenderer = questionCanvas.transform.GetChild(i).gameObject.GetComponent<Renderer>();
                planeRenderer.material.SetColor("_Color", color);
            }
        }
    }

    private void WriteResponses()
    {
        foreach(Question q in questions)
        {
            DataRecorder.Instance.WriteRaw(q.question, q.response);
        }
    }

    private void OnDestroy()
    {
        List<string> metrics = new List<string>();
        foreach(Question q in questions)
        {
            metrics.Add(q.response);
        }

        DataRecorder.Instance.WriteTask("Questionnaire", metrics);

        // We are going to the sandwich task with a new bubble!
        DataRecorder.Instance.increment_condition();
    }
}

