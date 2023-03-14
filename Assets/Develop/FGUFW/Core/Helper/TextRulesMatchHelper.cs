using System;
using System.Collections.Generic;

namespace FGUFW
{
/*
想做代码文本解析功能 最终返回bool值
数据分:变量(数据源),常量,运算符,优先级(括号)
数据源用于用反射还是整形Id(#123)
常量解析后存储为固定值免得每次调用都Parse字符串
但目前这些符号只能做数值比较
&(且)的优先级高于|(或)
*/
    /// <summary>
    /// 规则匹配 用于配表中的触发判定
    /// </summary>
    public static class TextRulesMatchHelper
    {
        public const string Greater = ">";
        public const string Less = "<";
        public const string Equal = "=";
        public const string NotEqual = "≠";
        public const string GreaterAndEqual = "≥";
        public const string LessAndEqual = "≤";
        public const string And = "&";
        public const string Or = "|";
        public const string DomainStart = "(";
        public const string DomainEnd = ")";


        public enum Mode
        {
            Number,//>12,<20
        }

    }

    public interface ITextRulesMatch
    {
        bool Match(string rules,string target,string value);
    }

    public class TextRules
    {
        string rules,value;
        IList<TextRules> childs;

        public bool Equals()
        {
            return false;
        }

        public void AddChild()
        {

        }

        public static implicit operator bool(TextRules exists)
        {
            return exists.Equals();
        }
    }



}