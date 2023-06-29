using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBuffManager : MonoBehaviour
{
    public List<BuffDebuffMagic> buffDebuffList;

    private void Start()
    {
        buffDebuffList = new List<BuffDebuffMagic>();
    }

    public void DeleteBuff(BuffDebuffMagic magic)
    {
        buffDebuffList.Remove(magic);
    }

    public float CheckDamageImpactBuff(float value)
    {
        if (buffDebuffList.Exists(x => x is Rolling))
        {
            buffDebuffList.Find(x => x is Rolling).Delete();
            return 0;
        }
        else if (buffDebuffList.Exists(x => x is Defense))
        {
            buffDebuffList.Find(x => x is Defense).Delete();

            value -= 5;

            if (value < 0)
                value = 0;

            return value;
        }

        return value;
    }
}