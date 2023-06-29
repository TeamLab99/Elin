using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCrystal : Crystal
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NormalProjectile") && !isCharge)
        {
            isCharge = true;
            ActionPuzzle();
        }
        else if (collision.gameObject.CompareTag("WaterProjectile") && !isCharge)
        {
            isCharge = true;
            ActionPuzzle();
        }
    }
}
