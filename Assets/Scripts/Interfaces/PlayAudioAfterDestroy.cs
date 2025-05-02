using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioAfterDestroy : MonoBehaviour
{
    //coro to key base empty obj until audio clip is finished
    public static IEnumerator SoundAfterDestroy(GameObject thisObj, float clipLength)
    {
        for (int i = 0; i < thisObj.transform.childCount; i++)
        {
            Destroy(thisObj.transform.GetChild(i).gameObject);
        }

        yield return new WaitForSecondsRealtime(clipLength);
        Destroy(thisObj.gameObject);
    }
    public static IEnumerator SoundAfterDisable(GameObject thisObj, float clipLength)
    {
        for (int i = 0; i < thisObj.transform.childCount; i++)
        {
            thisObj.transform.GetChild(i).gameObject.SetActive(false);
        }

        yield return new WaitForSecondsRealtime(clipLength);
        thisObj.SetActive(false);
    }
}
