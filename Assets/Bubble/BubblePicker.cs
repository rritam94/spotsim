using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class BubblePicker : MonoBehaviour
{
    public List<GameObject> prefabs;
    private int currPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        if(prefabs.Count > 1)
        {
            prefabs[0].SetActive(true);
            prefabs[1].SetActive(true);
            currPrefab = 0;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("down");
            prefabs[currPrefab++].SetActive(false);
            Debug.Log(currPrefab + " ");
            prefabs[currPrefab++].SetActive(false);
            if (currPrefab >= prefabs.Count) { currPrefab = 0; }
            prefabs[currPrefab].SetActive(true);
            Debug.Log(currPrefab + " ");
            prefabs[currPrefab + 1].SetActive(true);
        }
    }

    public void OnQuestionsAnswered()
    {
        
    }
}
