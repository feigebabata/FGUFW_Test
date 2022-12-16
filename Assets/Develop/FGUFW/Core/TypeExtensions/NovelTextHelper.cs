using System.Text;
using System.Text.RegularExpressions;

namespace FGUFW
{
    public static class NovelTextHelper
    {
        private const string CHAPER_END = "章卷篇节回话部集季";
        private const int CHAPER_MAX_LENGTH = 25;
        private const string LINE_END = ";；。!！」…”\"》）\\-※";
        private const string NUMBER = "0123456789零〇一二三四五六七八九十百千ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅪⅫⅬⅭⅮⅯⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹⅺⅻⅼⅽⅾⅿ";
        private static Regex regexChapter = new Regex($"第[{NUMBER}]+?[{CHAPER_END}]{{1}}");
        private static Regex regexLineEnd = new Regex($"[{LINE_END}]$");
        private static Regex regexChapter2_1 = new Regex($@"\([{NUMBER}]+\)$");
        private static Regex regexChapter2_2 = new Regex($@"\([{NUMBER}]+-[{NUMBER}]+\)$");
        private static Regex regexChapter3_1 = new Regex($"（[{NUMBER}]+）$");
        private static Regex regexChapter3_2 = new Regex($"（[{NUMBER}]+-[{NUMBER}]+）$");


        public static string[][] SplitChapter(string[] lines)
        {
            
            return null;
        }

        public static bool LineIsChapter(string line)
        {
            if(line.Length>CHAPER_MAX_LENGTH)return false;
            return regexChapter.IsMatch(line) 
            || regexChapter2_1.IsMatch(line)
            || regexChapter2_2.IsMatch(line)
            || regexChapter3_1.IsMatch(line)
            || regexChapter3_2.IsMatch(line);
        }

        public static bool LineIsEnd(string line)
        {
            return regexLineEnd.IsMatch(line);
        }


    }
}