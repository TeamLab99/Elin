using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inven : MonoBehaviour
{
    public Button[] btn;
   
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<btn.Length; i++)
        {
            int temp = i;
            //btn[i] = GetComponent<Button>();
            btn[i].onClick.AddListener(()=>BtnUse(temp));
        }
    }

    public void BtnUse(int num)
    {
        Debug.Log(num);
    }
    
}
