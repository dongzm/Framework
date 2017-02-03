using System;
using System.Text;
using System.Text.RegularExpressions;


namespace Framework.Support
{

    public class RegularExpressionsHelper
    {
        public static StringBuilder Replace(StringBuilder sb, string oldchar, string newchar)
        {
            MatchCollection matchs = Regex.Matches(sb.ToString(), "(" + oldchar + @")([^\w])");
            foreach (Match match in matchs)
            {
                sb.Remove(match.Groups[1].Index, match.Groups[1].Length);
                sb.Insert(match.Groups[1].Index, newchar);
            }
            return sb;
        }
    }
}

