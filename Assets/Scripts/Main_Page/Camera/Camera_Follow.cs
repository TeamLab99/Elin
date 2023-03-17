using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed;
    public bool followPlayer;
    private Vector3 offSet;
    
    void Start()
    {
        followPlayer = true;
        offSet = transform.position - target.position;
    }

    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offSet;
        if (followPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothSpeed * Time.deltaTime);
        }
    }

    public void IsFollow()
    {
        if (followPlayer)
            followPlayer = false;
        else
            followPlayer = true;
    } 
}
