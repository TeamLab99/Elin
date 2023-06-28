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
}
