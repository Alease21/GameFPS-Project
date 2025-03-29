using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPickUp : MonoBehaviour
{
    Rect rect;
    Texture texture;

    // Start is called before the first frame update
    void Start()
    {
        float size = Screen.width * 0.1f;
        rect = new Rect(Screen.width / 2 - size / 2, Screen.height * 0.7f, size, size);
        texture = Resources.Load("Textures/ShieldLargeT2") as Texture;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (GameVariables.ShieldLargeT2DisplayTime > 0)
        {
            GameVariables.ShieldLargeT2DisplayTime = Time.deltaTime;
        }

    }

    private void OnGUI()
    {
        if (GameVariables.ShieldLargeT2DisplayTime > 0)
        {
            GUI.DrawTexture(rect, texture);
        }
    }
}
