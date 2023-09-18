using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            CamerEffect.instance.camerEffectType = CamerEffectType.FirstEnd;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            CamerEffect.instance.camerEffectType = CamerEffectType.FollowPlayer;
    }
}

