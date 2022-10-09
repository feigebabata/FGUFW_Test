using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAO_UI;
using System;
using FGUFW;

namespace SAO_UI
{
    public class Test : MonoBehaviour
    {
        public Sprite[] SelectSprites, UnselectSprites;
        public Sprite[] SelectIcons, UnselectIcons;
        public string[] Titles;
        public SecondaryMenuListComp SecondaryListComp;
        private int _secondarySelectIndex = int.MaxValue;

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
            if (_secondarySelectIndex == arg1)
            {
                SecondaryListComp.Close();
            }
        }

        private void OnItemSelect(int arg1, LoopListItemBaseComp arg2)
        {
            _secondarySelectIndex = arg1;
            // Debug.Log($"OnItemSelect:{arg1}");
            SecondaryListComp.OnItemShow -= OnSecondaryItemShow;
            SecondaryListComp.OnItemShow += OnSecondaryItemShow;
            SecondaryListComp.Init(3);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnItemShow(int itemIndex, LoopListItemBaseComp item)
        {
            var comp = (MainMenuListItemComp)item;
            comp.gameObject.SetActive(true);
            comp.Select = SelectSprites[itemIndex];
            comp.Unselect = UnselectSprites[itemIndex];

            comp.Img.sprite = comp.Unselect;
        }

        void OnSecondaryItemShow(int itemIndex, LoopListItemBaseComp item)
        {
            var comp = (SecondaryMenuListItemComp)item;
            comp.gameObject.SetActive(true);
            comp.Select = SelectIcons[itemIndex];
            comp.Unselect = UnselectIcons[itemIndex];
            comp.Title.text = Titles[itemIndex];


            comp.Img_Icon.sprite = comp.Unselect;
            comp.Img_BG.color = new Color32(255, 255, 255, 255);
            comp.Img_Select.color = new Color32(255, 255, 255, 255);
            comp.Title.color = SecondaryMenuListItemComp.Unselect_Text_Color;
            comp.Img_Group.alpha = 1;
        }
    }
}
