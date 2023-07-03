using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverEffect : MonoBehaviour
{
    [SerializeField] GameObject activeBtns;
    [SerializeField] GameObject warningMessage;
    [SerializeField] Image panelImage;
    public float targetAlpha = 0.3f; 
    public float transitionDuration = 1.0f; 
   
    private float currentAlpha=0f; 
    private float timer=0f;
    private bool active = true;
 
    private void Update()
    {
        if (active)
        {
            timer += Time.deltaTime;
            float interpolatedAlpha = Mathf.Lerp(currentAlpha, targetAlpha, timer / transitionDuration);
            Color panelColor = panelImage.color;
            panelColor.a = interpolatedAlpha;
            panelImage.color = panelColor;

            if (timer >= transitionDuration)
            {
                activeBtns.SetActive(true);
                active = false;
            }
        }
    }

    private void OnEnable()
    {
        currentAlpha = 0f;
        timer = 0f;
        active = true;
    }

    private void OnDisable()
    {
        activeBtns.SetActive(false);
        warningMessage.SetActive(false);
    }
}
