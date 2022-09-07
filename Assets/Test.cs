using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAO_UI;
using System;

public class Test : MonoBehaviour
{
    public Sprite[] SelectSprites,UnselectSprites;
    public Sprite[] SelectIcons, UnselectIcons;
    public string[] Titles;
    public SecondaryMenuListComp SecondaryListComp;

    // Start is called before the first frame update
    void Start()
    {
        
        var list = GetComponent<MainMenuListComp>();
        list.OnItemShow += OnItemShow;
        list.OnItemSelect += OnItemSelect;
        list.OnItemUnselect += OnItemUnselect;
        list.Init(7);
    }

    private void OnItemUnselect(int arg1, LoopListItemBaseComp arg2)
    {
        
    }

    private void OnItemSelect(int arg1, LoopListItemBaseComp arg2)
    {
        SecondaryListComp.OnItemShow -= OnSecondaryItemShow;
        SecondaryListComp.OnItemShow += OnSecondaryItemShow;
        SecondaryListComp.Init(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnItemShow(int itemIndex,LoopListItemBaseComp item)
    {
        var comp = (MainMenuListItemComp)item;
        comp.gameObject.SetActive(true);
        comp.Select = SelectSprites[itemIndex];
        comp.Unselect = UnselectSprites[itemIndex];
    }

    void OnSecondaryItemShow(int itemIndex, LoopListItemBaseComp item)
    {
        var comp = (SecondaryMenuListItemComp)item;
        comp.gameObject.SetActive(true);
        comp.Select = SelectIcons[itemIndex];
        comp.Unselect = UnselectIcons[itemIndex];
        comp.Title.text = Titles[itemIndex];
    }
}
