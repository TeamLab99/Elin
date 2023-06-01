using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatForm_Manager : MonoBehaviour
{
    static public PlatForm_Manager instance;
  
    public int currentHP;
    public int attackPower;
    public int maxCost;
    public float recoveryCost;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
}
