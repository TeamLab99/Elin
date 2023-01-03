using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{

    [SerializeField] Image effect;

    // Start is called before the first frame update

    public void SetColor(string str)
    {
        switch (str)
        {
            case "r":
                effect.color = new Color32(255, 0, 0, 255);
                break;
            case "g":
                effect.color = new Color32(0, 255, 0, 255);
                break;
            case "b":
                effect.color = new Color32(0, 0, 255, 255);
                break;
        }
    }


}
