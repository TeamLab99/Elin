using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class son : parent
{
    int a = 0;
    // Start is called before the first frame update
    private void Update()
    {
        a++;
        if(a<10)
            Debug.Log(a);
    }
}
