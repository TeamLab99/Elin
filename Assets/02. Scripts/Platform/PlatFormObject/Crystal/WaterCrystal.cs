using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCrystal : Crystal
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("WaterProjectile") && !isCharge)
        {
            isCharge = true;
            ActionPuzzle();
        }
    }
}
