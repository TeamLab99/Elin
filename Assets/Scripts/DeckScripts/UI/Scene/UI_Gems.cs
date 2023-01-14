using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Gems : UI_Base
{

    void Start()
    {
        Init();
    }

    public override void Init()
    {

    }

    #region SetGem
    public void setFireGem()
    {
        if(Managers.Data.equipGem == Define.Gems.FireGem)
        {
            Managers.Data.equipGem = Define.Gems.none;
        }
        else
        {
            Managers.Data.equipGem = Define.Gems.FireGem;
        }
    }

    public void setWaterGem()
    {
        if (Managers.Data.equipGem == Define.Gems.WaterGem)
        {
            Managers.Data.equipGem = Define.Gems.none;
        }
        else
        {
            Managers.Data.equipGem = Define.Gems.WaterGem;
        }
    }

    public void setWindGem()
    {
        if (Managers.Data.equipGem == Define.Gems.WindGem)
        {
            Managers.Data.equipGem = Define.Gems.none;
        }
        else
        {
            Managers.Data.equipGem = Define.Gems.WindGem;
        }
    }

    public void setEarthGem()
    {
        if (Managers.Data.equipGem == Define.Gems.EarthGem)
        {
            Managers.Data.equipGem = Define.Gems.none;
        }
        else
        {
            Managers.Data.equipGem = Define.Gems.EarthGem;
        }
    }
    #endregion
}
