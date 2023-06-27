using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public bool isCharge = false;
    public Bridge bridge;
    public void ActionPuzzle()
    {
        bridge.LayBridge();
    }
}
