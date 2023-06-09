using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffUI : MonoBehaviour
{ 
    [SerializeField] GameObject statObject;
    [SerializeField] GameObject invenObject;
    bool statActivate = false;
    bool invenActivate = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            statActivate = !statActivate;
            statObject.SetActive(statActivate);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            invenActivate = !invenActivate;
            invenObject.SetActive(invenActivate);
        }
    }
}
