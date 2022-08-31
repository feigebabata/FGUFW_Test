using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAO_UI;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var list = GetComponent<MainLoopList>();
        list.OnItemCreate += onItemCreate;
        list.Init(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onItemCreate(int itemIndex,ListItemSpriteComp comp)
    {
        comp.gameObject.SetActive(true);
    }
}
