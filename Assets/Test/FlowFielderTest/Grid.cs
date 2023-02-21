using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public GridType Grid_Type=GridType.Normal;
    
    void Awake()
    {
        Grid_Type = GetComponent<Image>().color!=Color.white?GridType.Obstacle:GridType.Normal;
        GetComponent<Image>().color = Grid_Type==GridType.Obstacle?Color.yellow:Color.white;
    }




    public enum GridType
    {
        None=0,
        Normal=1,
        Obstacle=3,
    }

}


