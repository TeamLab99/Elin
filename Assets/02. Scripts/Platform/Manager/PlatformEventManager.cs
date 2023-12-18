using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEventManager : Singleton<PlatformEventManager>
{
    [SerializeField] GameObject appearPlatform;
    [SerializeField] GameObject movePlatform;
    [SerializeField] GameObject enhanceUI;
    [SerializeField] Vector3[] ericaSpawnPos;
    GameObject player;
    GameObject brokeBranch;
    PlayerController playerController;
    int idx = -1;
    int ericaTf = 0;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        brokeBranch = GameObject.FindGameObjectWithTag("Branch");
        playerController = player.GetComponent<PlayerController>();
    }

    public void SetEvent()
    {
        idx += 1;
        switch (idx)
        {
            case 0:
                AppearPlatformEvent();
                break;
            case 1:
                DialogueManager.instance.StartDialogue("plusText");
                break;
            case 2:
                movePlatform.GetComponent<MovePlatform>().movePlatform = true;
                Debug.Log(idx);
                break;
            case 3:
                SeeAppleEvent();
                Debug.Log(idx);
                break;
            case 4:
                FallEvent();
                Debug.Log(idx);
                break;
            case 5:
                //NextEricaDialogue();
                Debug.Log(idx);
                enhanceUI.SetActive(true);
                break;
            case 6:
                Debug.Log(idx);
                enhanceUI.SetActive(false);
                NextEricaDialogue();
                break;
        }
    }

    public void AppearPlatformEvent()
    {
        appearPlatform.SetActive(true);
    }

    public void SeeAppleEvent()
    {
        CamerEffect.instance.ChangeGotoCameraMode();
    }

    public void FallEvent()
    {
        brokeBranch.SetActive(false);
        CamerEffect.instance.ChangeLateFollowCamerMode();
    }

    public void EricaSpawn()
    {
        if (ericaSpawnPos[ericaTf] == null)
            return;
        GameObject erica = GameObject.Find("Erica");
        erica.transform.position = ericaSpawnPos[ericaTf];
        ericaTf += 1;
    }

    public void NextEricaDialogue()
    {
        DialogueManager.instance.NextDialogue("Erica");
        DialogueManager.instance.StartDialogue("Erica");
    }
}
