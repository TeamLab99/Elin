using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{
    void FixedUpdate()
    {
        if (dirX == 1)
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        else if(dirX==-1)
            gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
    protected override void Think()
    {
        dirX = Random.Range(-1, 2);
        Invoke("Think", 3f);
    }
}
