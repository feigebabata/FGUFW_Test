using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSortingOrder : MonoBehaviour
{
    public int SortingOrder;
    public Color[] Pixels;
    public Texture2D tex2D;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        GetComponent<MeshRenderer>().sortingOrder = SortingOrder;
        tex2D = new Texture2D(Pixels.Length,1,TextureFormat.RGBA32,0,true);
        tex2D.SetPixels(Pixels);
        tex2D.Apply();
        GetComponent<MeshRenderer>().material.mainTexture = tex2D;
    }
}
