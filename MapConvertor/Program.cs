using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapConvertor
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"E:\My\DOTNET\Klotski\Klotski2\level.txt";
            var maps = File.ReadAllLines(path);
            var newMaps = maps.Select(ConvertMap);
            File.WriteAllLines(@"E:\My\DOTNET\Klotski\Klotski2\maps.txt", newMaps);
        }


        static string ConvertMap(string input)
        {
            var blocks = input.Split('|').Select(ConvertBlock);
            return "6*6;r2;" + string.Join(",", blocks);
        }
        static string ConvertBlock(string input)
        {
            var es = input.Replace(",", "");
            if (es.EndsWith("02"))
            {
                return "0" + es.Substring(0, 2);
            }
            if (es.EndsWith("03"))
            {
                return "1" + es.Substring(0, 2);
            }
            if (es.EndsWith("12"))
            {
                return "2" + es.Substring(0, 2);
            }
            if (es.EndsWith("13"))
            {
                return "3" + es.Substring(0, 2);
            }

            return "";
        }
    }
}
