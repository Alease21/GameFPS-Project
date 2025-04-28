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

            for (int i = 0; i < objsInScene.Length; i++)
            {
                if (objsInScene[i].GetComponent<EnemyScript>() || objsInScene[i].GetComponent<BarrelScript>()
                    || objsInScene[i].GetComponent<PlayerStatsScript>() || objsInScene[i].GetComponent<ItemPackScript>())
                {
                    if (!tempRegistry.ContainsValue(objsInScene[i]))
                    {
                        tempRegistry.Add(GenerateGUID(), objsInScene[i]);
                    }
                }
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
