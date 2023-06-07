using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffUI : MonoBehaviour
{ 
    [SerializeField] GameObject StatObject;
    bool statActivate = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            statActivate = !statActivate;
            StatObject.SetActive(statActivate);
        }
    }
}
