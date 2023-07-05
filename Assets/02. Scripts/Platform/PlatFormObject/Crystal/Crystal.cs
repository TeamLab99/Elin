using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    Animator anim;
    public bool isCharge = false;
    public bool bothCharge = false;
    public Door door;

    protected void Awake()
    {
        anim = GetComponent<Animator>();    
    }

    public void ActionPuzzle()
    {
        if (bothCharge)
        {
            door.BothOpenDoor();
            anim.SetBool("Active", true);
            StartCoroutine("TurnOffCrystal");
        }
        else {
            door.OpenDoor();
            anim.SetBool("Active", true);
        }
    }

    public IEnumerator TurnOffCrystal()
    {
        yield return new WaitForSeconds(1f);
        if (!door.bothCharge)
        {
            isCharge = false;
            anim.SetBool("Active", false);
            door.crystalCnt -= 1;
        }
    }
}
