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
        if(Managers.Data.equipGem == EGems.FireGem)
        {
            Managers.Data.equipGem = EGems.none;
        }
        else
        {
            Managers.Data.equipGem = EGems.FireGem;
        }
    }

    public void setWaterGem()
    {
        if (Managers.Data.equipGem == EGems.WaterGem)
        {
            Managers.Data.equipGem = EGems.none;
        }
        else
        {
            Managers.Data.equipGem = EGems.WaterGem;
        }
    }

    public void setWindGem()
    {
        if (Managers.Data.equipGem == EGems.WindGem)
        {
            Managers.Data.equipGem = EGems.none;
        }
        else
        {
            Managers.Data.equipGem = EGems.WindGem;
        }
    }

    public void setEarthGem()
    {
        if (Managers.Data.equipGem == EGems.EarthGem)
        {
            Managers.Data.equipGem = EGems.none;
        }
        else
        {
            Managers.Data.equipGem = EGems.EarthGem;
        }
    }
    #endregion
}
