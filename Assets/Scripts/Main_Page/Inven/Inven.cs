using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inven : MonoBehaviour
{
    Button btn;
    public void BtnUse()
    {
        Debug.Log("사용");
    }
    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(BtnUse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
