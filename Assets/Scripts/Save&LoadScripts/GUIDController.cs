using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class GUIDController : MonoBehaviour
{
    public Dictionary<string, GameObject> tempRegistry = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        EvaluateGUID();
    }

    private void EvaluateGUID()
    {
        if (SceneManager.GetActiveScene().isLoaded)
        {
            GameObject[] objsInScene = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObject obj;

            //Loop through all scene objs & their children, then adds obj to
            //temp registry once and checks children if any
            for (int i = 0; i < objsInScene.Length; i++)
            {
                AddToTempRegistry(objsInScene[i]);

                //possible problem if child of child has needed script (no check),
                //but scene is set up such that this shouldn't happen
                if (objsInScene[i].transform.childCount != 0)
                {
                    for (int j = 0; j < objsInScene[i].transform.childCount; j++)
                    {
                        obj = objsInScene[i].transform.GetChild(j).gameObject;
                        AddToTempRegistry(obj);
                    }
                }
            }
        }
    }

    //Check if incoming obj has any of there scripts, if true then generate GUID &
    //add to registry (if not already contained in)
    private void AddToTempRegistry(GameObject obj)
    {
        if (obj.GetComponent<EnemyScript>() || obj.GetComponent<BarrelScript>() || 
                obj.GetComponent<PlayerStatsScript>() || obj.GetComponent<ItemPackScript>())
        {
            if (!tempRegistry.ContainsValue(obj))
            {
                tempRegistry.Add(GenerateGUID(), obj);
            }
        }
    }

    private void Start()
    {
        EvaluateGUID();

        if (Application.isPlaying == false)
        {
            return;
        }

        GUIDRegistry.Register(tempRegistry);
    }

    //makes identifier
    public string GenerateGUID()
    {
        return System.Guid.NewGuid().ToString();
    }

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
