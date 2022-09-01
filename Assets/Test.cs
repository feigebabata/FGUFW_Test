using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAO_UI;

public class Test : MonoBehaviour
{
    public Sprite[] SelectSprites,UnselectSprites;
    // Start is called before the first frame update
    void Start()
    {
        
        var list = GetComponent<MainLoopList>();
        list.OnItemShow += OnItemShow;
        list.Init(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnItemShow(int itemIndex,ListItemSpriteComp comp)
    {
        comp.gameObject.SetActive(true);
        comp.Select = SelectSprites[itemIndex];
        comp.Unselect = UnselectSprites[itemIndex];
    }
}
