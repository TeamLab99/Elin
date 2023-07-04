using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public bool isCharge = false;
    public bool bothCharge = false;
    public Door door;
    public void ActionPuzzle()
    {
        if (bothCharge)
        {
            door.BothOpenDoor();
            StartCoroutine("TurnOffCrystal");
        }
        else
            door.OpenDoor();
    }

    public IEnumerator TurnOffCrystal()
    {
        yield return new WaitForSeconds(1f);
        if (!door.bothCharge)
        {
            isCharge = false;
            door.crystalCnt -= 1;
        }
    }
}
