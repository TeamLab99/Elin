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
        if(Managers.Data.equipGem == Gems.FireGem)
        {
            Managers.Data.equipGem = Gems.none;
        }
        else
        {
            Managers.Data.equipGem = Gems.FireGem;
        }
    }

    public void setWaterGem()
    {
        if (Managers.Data.equipGem == Gems.WaterGem)
        {
            Managers.Data.equipGem = Gems.none;
        }
        else
        {
            Managers.Data.equipGem = Gems.WaterGem;
        }
    }

    public void setWindGem()
    {
        if (Managers.Data.equipGem == Gems.WindGem)
        {
            Managers.Data.equipGem = Gems.none;
        }
        else
        {
            Managers.Data.equipGem = Gems.WindGem;
        }
    }

    public void setEarthGem()
    {
        if (Managers.Data.equipGem == Gems.EarthGem)
        {
            Managers.Data.equipGem = Gems.none;
        }
        else
        {
            Managers.Data.equipGem = Gems.EarthGem;
        }
    }
    #endregion
}
