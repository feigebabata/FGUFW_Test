using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

namespace SAO_UI
{
    public class SecondaryMenuListItemComp : LoopListItemBaseComp
    {
        public static readonly Color Select_BG_Color = new Color32(236,166,0,230), Unselect_BG_Color = new Color32(255, 255, 255, 128), Select_Text_Color = new Color32(255, 255, 255, 255), Unselect_Text_Color = new Color32(76, 76, 76, 255);
        public Sprite Unselect,Select;
        public Image Img_BG, Img_Select, Img_Icon;
        public Text Title;
        public CanvasGroup Img_Group;

        public override void SwitchState(LoopListItemState state)
        {
            // Debug.LogWarning(state);
            if(state==_prevState)return;
            StopAllCoroutines();
            switch (state)
            {
                case LoopListItemState.Moving:
                    {
                        this.Img_Icon.sprite = this.Unselect;
                        this.Img_BG.color = new Color32(255, 255, 255, 255);
                        this.Img_Select.color = new Color32(255, 255, 255, 255);
                        this.Title.color = Unselect_Text_Color;
                        this.Img_Group.alpha = 1;
                    }
                    break;
                case LoopListItemState.Selecting:
                    {
                        StartCoroutine(selecting());
                    }
                    break;
                case LoopListItemState.Unselecting:
                    {
                        StartCoroutine(unselecting());
                    }
                    break;
            }
            base.SwitchState(state);
        }

        private IEnumerator selecting()
        {
            do
            {
                var deltaTime = Time.unscaledDeltaTime;
                if (this.Img_Icon.sprite == this.Select)
                {
                    var bg_color = this.Img_BG.color;
                    float speed = (1 - Select_BG_Color.a) / 0.25f;
                    this.Img_Group.alpha = Mathf.MoveTowards(this.Img_Group.alpha, Select_BG_Color.a, speed * deltaTime);
                    
                }
                else
                {
                    var bg_color = this.Img_BG.color;
                    float speed = (1 - Unselect_BG_Color.a) / 0.25f;
                    this.Img_Group.alpha = Mathf.MoveTowards(this.Img_Group.alpha, 1, speed * deltaTime);

                    if (this.Img_Group.alpha == 1)
                    {
                        this.Img_Icon.sprite = this.Select;
                        bg_color = Select_BG_Color;
                        bg_color.a = 1;
                        this.Img_BG.color = bg_color;
                        this.Img_Select.color = bg_color;
                        this.Title.color = Select_Text_Color;
                    }
                }
                yield return null;
            } 
            while (this.Img_Group.alpha != Select_BG_Color.a);
        }

        private IEnumerator unselecting()
        {
            do
            {
                var deltaTime = Time.unscaledDeltaTime;
                if (this.Img_Icon.sprite == this.Unselect)
                {
                    var color = this.Img_BG.color;
                    float speed = (1 - Unselect_BG_Color.a) / 0.25f;
                    this.Img_Group.alpha = Mathf.MoveTowards(this.Img_Group.alpha, Unselect_BG_Color.a, speed * deltaTime);
                }
                else
                {
                    var color = this.Img_BG.color;
                    float speed = (1 - Select_BG_Color.a) / 0.25f;
                    this.Img_Group.alpha = Mathf.MoveTowards(this.Img_Group.alpha, 1, speed * deltaTime);
                    color.a = 1 - color.a;
                    this.Img_Select.color = color;
                    // Debug.Log($"item{ItemIndex}_{this.Img_Group.alpha}");
                    if (this.Img_Group.alpha == 1)
                    {
                        this.Img_Icon.sprite = this.Unselect;
                        this.Img_BG.color = new Color32(255, 255, 255, 255);
                        this.Img_Select.color = new Color32(255, 255, 255, 255);
                        this.Title.color = Unselect_Text_Color;
                    }
                }
                yield return null;
            } 
            while (this.Img_Group.alpha != Unselect_BG_Color.a);
        }
    }

}
