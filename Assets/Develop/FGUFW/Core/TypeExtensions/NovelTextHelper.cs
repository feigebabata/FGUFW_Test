using System.Text;
using System.Text.RegularExpressions;

namespace FGUFW
{
    public static class NovelTextHelper
    {
        private const string CHAPER_END = "章卷篇节回话部集季";
        private const string LINE_END = "";
        private const string NUMBER = "0123456789零〇一二三四五六七八九十百千ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅪⅫⅬⅭⅮⅯⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹⅺⅻⅼⅽⅾⅿ";
        private static Regex regexChapter = new Regex($"第([{NUMBER}]??)[{CHAPER_END}]");

        public static string[][] SplitChapter(string[] lines)
        {
            
            return null;
        }

        public static bool LineIsChapter(string line)
        {
            return false;
        }


    }
}