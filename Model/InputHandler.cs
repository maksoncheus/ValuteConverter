using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValuteConverter
{
    internal static class InputHandler
    {
        public static string Unify(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            input = input.Replace('.', ',');
            if (input.StartsWith(','))
                return string.Empty;
            if (input.Count(c => c == ',') > 1)
                input = new string(input.Take(input.Length - 1).ToArray());
            return input;
        }
    }
}
