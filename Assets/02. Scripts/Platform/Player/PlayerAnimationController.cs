using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    

    public void ChangeFallGravity()
    {
        playerController.SetFallGravity();
    }

    public void ChangeJumpGravity()
    {
        playerController.SetJumpGravity();
    }
}
