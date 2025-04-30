using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadControl : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F5))
        {
            //eventually change for curSave variable?
            SaveLoadScript.Save("GameSave1", new GameData());
            Debug.Log("Game data saved");
        }
        if (Input.GetKeyUp(KeyCode.F8))
        {
            GameData gameData = SaveLoadScript.Load("GameSave1");
            gameData?.LoadData();
            Debug.Log("Game data loaded");
        }

    }
}
