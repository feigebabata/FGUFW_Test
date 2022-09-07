using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using FGUFW;

namespace SAO_UI
{
    public class MainMenuListItemComp : LoopListItemBaseComp
    {
        public Image Img;
        public Sprite Unselect,Select;
        public float Select_Alpha=0.9f,Unselect_Alpha=0.5f;

        public override void SwitchState(LoopListItemState state)
        {
            // Debug.Log($"SwitchState {transform.FullPath()}:{state}");
            if(state==_prevState)return;
            StopAllCoroutines();
            switch (state)
            {
                case LoopListItemState.Moving:
                    {
                        var color = this.Img.color;
                        color.a=1;
                        this.Img.color = color;
                        this.Img.sprite = Unselect;
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
                if (this.Img.sprite == this.Select)
                {
                    var color = this.Img.color;
                    float speed = (1 - Select_Alpha) / 0.25f;
                    color.a = Mathf.MoveTowards(color.a, Select_Alpha, speed * deltaTime);
                    this.Img.color = color;
                }
                else
                {
                    var color = this.Img.color;
                    float speed = (1 - Unselect_Alpha) / 0.25f;
                    color.a = Mathf.MoveTowards(color.a, 1, speed * deltaTime);
                    this.Img.color = color;
                    if (color.a == 1) this.Img.sprite = this.Select;
                }
                yield return null;
            } 
            while (this.Img.color.a != Select_Alpha);
        }

        private IEnumerator unselecting()
        {
            do
            {
                var deltaTime = Time.unscaledDeltaTime;
                if (this.Img.sprite == this.Unselect)
                {
                    var color = this.Img.color;
                    float speed = (1 - Unselect_Alpha) / 0.25f;
                    color.a = Mathf.MoveTowards(color.a, Unselect_Alpha, speed * deltaTime);
                    this.Img.color = color;
                }
                else
                {
                    var color = this.Img.color;
                    float speed = (1 - Select_Alpha) / 0.25f;
                    color.a = Mathf.MoveTowards(color.a, 1, speed * deltaTime);
                    this.Img.color = color;
                    if (color.a == 1) this.Img.sprite = this.Unselect;
                }
                yield return null;
            } 
            while (this.Img.color.a != Unselect_Alpha);
        }
    }

}
