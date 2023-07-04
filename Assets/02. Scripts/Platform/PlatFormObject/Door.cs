using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int crystalCnt = 0;
    public bool bothCharge = false;

    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        anim.SetBool("Active", true);
    }

    public void BothOpenDoor()
    {
        crystalCnt += 1;
        if (crystalCnt == 2)
        {
            bothCharge = true;
            anim.SetBool("Active", true);
        }
    }
}
