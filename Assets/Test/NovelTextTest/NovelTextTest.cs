using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW;
using UnityEngine.UI;

public class NovelTextTest : MonoBehaviour
{
    public Text textComp;
    
    // Start is called before the first frame update
    void Start()
    {
        textComp.font.RequestCharactersInTexture(textComp.text,textComp.fontSize);
        foreach (var item in textComp.text)
        {
            CharacterInfo info;
            if(textComp.font.GetCharacterInfo(item,out info,textComp.fontSize))
            {
                Debug.Log(info.width);
            }
        }
    }
    
}
