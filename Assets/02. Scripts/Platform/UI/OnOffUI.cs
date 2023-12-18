using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffUI : Singleton<OnOffUI>
{ 
    [SerializeField] GameObject statObject;
    [SerializeField] GameObject invenObject;
    [SerializeField] GameObject exitObject;
    public GameObject endingObject;
    public bool onBattlePage;
    bool statActivate = false;
    bool invenActivate = false;
    bool exitActivate = false;
    private void Start()
    {
        BattleManager.PlatformUIControlForBattle += SetBool;
        BattleManager.PlatformUIControlForDialouge += SetBool;
    }

    private void OnDestroy()
    {
        BattleManager.PlatformUIControlForBattle -= SetBool;
        BattleManager.PlatformUIControlForDialouge -= SetBool;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && !onBattlePage)
        {
            statActivate = !statActivate;
            statObject.SetActive(statActivate);
        }

        if (Input.GetKeyDown(KeyCode.I) && !onBattlePage)
        {
            invenActivate = !invenActivate;
            invenObject.SetActive(invenActivate);
            InvenUI.instance.SetAum();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitActivate = !exitActivate;
            exitObject.SetActive(exitActivate);
            if (exitActivate)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }
    }

    void SetBool()
    {
        if (onBattlePage)
            onBattlePage = false;
        else
        {
            onBattlePage = true;
            invenObject.SetActive(false);
            statObject.SetActive(false);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        exitActivate = !exitActivate;
        exitObject.SetActive(exitActivate);
    }
}
