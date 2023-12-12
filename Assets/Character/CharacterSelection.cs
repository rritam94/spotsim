using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private GameObject VR_Cylinder;
    [SerializeField] private GameObject VR_Suit;
    [SerializeField] private GameObject VR_Polygon;

    [SerializeField] private GameObject trd_Cylinder;
    [SerializeField] private GameObject trd_Suit;
    [SerializeField] private GameObject trd_Polygon;

    private void Start()
    {

        string condition = DataRecorder.Instance.GetCondition();

        if (condition.Equals("Cylinder"))
        {
            VR_Cylinder.SetActive(true);
            trd_Cylinder.SetActive(true);
        }

        if (condition.Equals("Polygon"))
        {
            VR_Polygon.SetActive(true);
            trd_Polygon.SetActive(true);
        }

        if (condition.Equals("Suit"))
        {
            VR_Suit.SetActive(true);
            trd_Suit.SetActive(true);
        }

    }
}

