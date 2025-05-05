using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayAudioAfterDestroy : MonoBehaviour
{
    //coroutine to remove visible aspects of object am keep audio and/or effects
    public static IEnumerator SoundAfterDestroy(GameObject thisObj, float clipLength)
    {
        for (int i = 0; i < thisObj.transform.childCount; i++)
        {
            if (thisObj.transform.GetChild(i).tag == "PrefabObjVisual")
            {
                thisObj.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        yield return new WaitForSecondsRealtime(clipLength);
        Destroy(thisObj.gameObject);
    }
    public static IEnumerator SoundAfterDisable(GameObject thisObj, float clipLength)
    {
        for (int i = 0; i < thisObj.transform.childCount; i++)
        {
            if (thisObj.transform.GetChild(i).tag == "PrefabObjVisual")
            {
                thisObj.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        yield return new WaitForSecondsRealtime(clipLength);
        thisObj.gameObject.SetActive(false);
    }
}
