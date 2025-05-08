using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadControl : MonoBehaviour
{
    public static SaveLoadControl instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    /*
    public enum SaveNum
    {
        save1,
        save2,
        save3
    }

    private string currSave,
        save1 = "GameSave1",
        save2 = "GameSave2",
        save3 = "GameSave3";
    */
    public Action saveGame;
    public Action gameLoad;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            saveGame?.Invoke();//used for setting some vars instead of updating every frame
            
            //eventually change for curSave variable?
            SaveLoadScript.Save("GameSave1", new GameData());
            Debug.Log("Game data saved");
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            GameData gameData = SaveLoadScript.Load("GameSave1");
            gameData?.LoadData();
            gameLoad?.Invoke();
            Debug.Log("Game data loaded");
        }
    }
    /*
    public void OnSelectSave(int saveNum)
    {
        switch (saveNum)
        {
            case 0:
                currSave = save1;
                break;
            case 1:
                currSave = save2;
                break;
            case 2:
                currSave = save3;
                break;
            default:
                Debug.Log("button enum val out of range");
                break;
        }
    }
    */
}
