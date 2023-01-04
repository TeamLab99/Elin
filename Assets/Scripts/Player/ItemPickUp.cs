using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public int itemID;
    public int count;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //인벤토리 추가함
            Inventory.instance.GetAnItem(itemID, count);
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}
