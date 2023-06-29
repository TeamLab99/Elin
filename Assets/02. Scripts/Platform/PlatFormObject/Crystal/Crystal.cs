using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public bool isCharge = false;
    public bool bothCharge = false;
    public Bridge bridge;
    public void ActionPuzzle()
    {
        if (bothCharge)
        {
            bridge.BothLayBridge();
            StartCoroutine("TurnOffCrystal");
        }
        else
            bridge.LayBridge();
    }

    public IEnumerator TurnOffCrystal()
    {
        yield return new WaitForSeconds(1f);
        if (!bridge.bothCharge)
        {
            isCharge = false;
            Debug.Log("실행됨");
            bridge.crystalCnt -= 1;
        }
    }
}
