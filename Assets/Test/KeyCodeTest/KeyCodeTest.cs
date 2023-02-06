using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW;
using System;

public class KeyCodeTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        KeyboardHelper.AddListener_KeyDown(onKeyDown,0,KeyCode.Space);
    }

    private void onKeyDown()
    {
        Debug.Log("Esc");
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        KeyboardHelper.RemoveListener_KeyDown(onKeyDown,KeyCode.Escape);
    }

}
