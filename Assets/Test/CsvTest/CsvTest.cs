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
        var array = NameSpace.ClassTest.ToArray(CsvFile.text);
        var dict = NameSpace.ClassTest.ToDict(CsvFile.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
