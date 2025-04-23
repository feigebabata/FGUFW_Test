using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace FGUFW.ExcelUtils
{
    [ScriptedImporter(0,new string[]{"xls","xlsx"})]
    public class ExcelScriptedImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var path = ctx.assetPath;

            // var exten = Path.GetExtension(path);
            // if(exten==".xlsx")
            // {
            //     Debug.LogWarning(path);
            // }

            // if(exten!=".xls" && exten!=".xlsx")return;

            

            GenerateExcelCsharpCode.GenerateCsharpCode(path);
        }

    }
}