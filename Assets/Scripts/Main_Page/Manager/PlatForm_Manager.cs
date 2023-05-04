using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatForm_Manager : MonoBehaviour
{
    static public PlatForm_Manager instance1;
  
    public int currentHP;
    public int attackPower;
    public int maxCost;
    public float recoveryCost;

    private void Awake()
    {
        if (instance1 != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance1 = this;
        }
    }
}
