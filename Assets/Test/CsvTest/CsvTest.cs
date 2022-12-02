using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW;
using System.IO;

public class CsvTest : MonoBehaviour
{
    public TextAsset CsvFile;

    // Start is called before the first frame update
    void Start()
    {
        string[][] table = new string[][]
        {
            new string[]{"a1","a2","a3"},
            new string[]{"b1\nb1","b2,b2","b3"},
            new string[]{"\"c1","\"c2\"","c3\",\""}
        };
        var text = CsvHelper.ToCsvText(table);
        File.WriteAllText("D:/a.csv",text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
