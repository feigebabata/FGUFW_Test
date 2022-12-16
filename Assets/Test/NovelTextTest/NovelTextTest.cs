using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW;

public class NovelTextTest : MonoBehaviour
{
    public string Line;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(NovelTextHelper.LineIsChapter(Line));
    }
    
}
