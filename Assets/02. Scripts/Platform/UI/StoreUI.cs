using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    List<int> keys = new List<int>();
    [SerializeField] Image[] sellImages;
    // Update is called once per frame
    public void ShowSellItems(Dictionary<int, Items> keyValuePairs)
    {
        int i = 0;
        foreach (int k in keyValuePairs.Keys)
        {
            keys.Add(k);
        }

       for(int t=0; t<keys.Count; t++)
        {
            sellImages[i].sprite = keyValuePairs[keys[t]].itemIcon;

            Debug.Log(keyValuePairs[keys[t]].itemDescription);
            i+=1;
        }
    }
}
