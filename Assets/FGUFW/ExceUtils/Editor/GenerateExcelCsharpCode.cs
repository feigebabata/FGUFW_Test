using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEditor;
using UnityEngine;

namespace FGUFW.ExcelUtils
{
    public static class GenerateExcelCsharpCode
    {
        //[MenuItem("Assets/Create/GenerateCsharpCode",true)]
        static bool checkGenerateCsharpCode()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if(path.IsNull())return false;

            var exten = Path.GetExtension(path);

            if(exten==".xls" || exten==".xlsx")return true;

            return false;
        }

        //[MenuItem("Assets/Create/GenerateCsharpCode",false,80)]
        static void generateCsharpCode()
        {
            var path = Application.dataPath.Replace("Assets",AssetDatabase.GetAssetPath(Selection.activeObject));
            GenerateCsharpCode(path);

        }

        public static void GenerateCsharpCode(string path)
        {

            var configClass = new StringBuilder();

            var execl = new Excel(path);

            var className = Path.GetFileNameWithoutExtension(path);
            var directory = Path.GetDirectoryName(path);

            foreach (var sheet in execl)
            {
                setConfigText(sheet,configClass);
            }

string scriptText = 
@"using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ExcelConfig
{
    public class |CLASS_NAME|
    {

|CONFIG_CLASS|

    }
}
";

            execl.Dispose();
            scriptText = scriptText.Replace("|CLASS_NAME|",className);
            scriptText = scriptText.Replace("|CONFIG_CLASS|",configClass.ToString());
            var scriptPath = Path.Combine(directory,$"{className}.cs");
            File.WriteAllText(scriptPath,scriptText);

            // AssetDatabase.ImportAsset(path,ImportAssetOptions.ForceUpdate);

            // AssetDatabase.Refresh();

            EditorApplication.delayCall += delayRefresh;
        }

        static void delayRefresh()
        {
            EditorApplication.delayCall -= delayRefresh;
            AssetDatabase.Refresh();
        }

        static void setConfigText(ISheet sheet,StringBuilder configTexts)
        {
            var row = sheet.GetRow(0);
            if(row==default)return;

            //第一行第一列 校验标识
            if(row.GetCell(0)?.ToString() != "GenerateExcelCsharpCode")return;

            var collection = row.GetCell(1)?.ToString();
            if(collection.IsNull())return;

            var className = sheet.SheetName;
            string scriptText = default;
            if(collection == "List")
            {
                scriptText = 
@"
        public List<|CLASS_NAME|> |CLASS_NAME|s;
        public class |CLASS_NAME|
        {
|MENBERS|
        }
";    
            }
            else if(collection == "Dictionary")
            {
                scriptText = 
@"
        public Dictionary<|KEY|,|CLASS_NAME|> |CLASS_NAME|s;
        public class |CLASS_NAME|
        {
|MENBERS|
        }
";              
            }      

            
            //第二行字段注释
            //第三行字段类型
            var types = sheet.GetRow(2);
            //第四行字段名
            var names = sheet.GetRow(3);

            StringBuilder menbers = new StringBuilder();
            for (int i = 0; i < types.LastCellNum; i++)
            {
                menbers.AppendLine(
@$"            public {types.GetCell(i)} {names.GetCell(i)};"
);
            }

            var key = types.GetCell(0).ToString();

            scriptText = scriptText.Replace("|CLASS_NAME|",className);
            scriptText = scriptText.Replace("|KEY|",key);
            scriptText = scriptText.Replace("|MENBERS|",menbers.ToString());

            configTexts.AppendLine(scriptText);
 
        }

    }
}