using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tttest : MonoBehaviour
{
    public Transform trf;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(trf.position);
    }
}
