using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public int itemID;
    public int count;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory.instance.GetAnItem(itemID, count);
        Destroy(gameObject);
    }

}
